using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Analytics;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;

    private DatabaseReference dbRef;
    private FirebaseAuth auth;
    private FirebaseUser user;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

    }

    public void OnClickSignIn()
    {
        StartCoroutine(UserSignIn());
    }

    public IEnumerator UserSignIn()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                dbRef = FirebaseDatabase.DefaultInstance.RootReference;
                SignInAnonymously();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
        yield return null;
    }

    void SignInAnonymously()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
            {
                var authResult = task.Result;
                user = authResult.User;
                Debug.Log("Firebase Signed In: " + user.UserId);
                SceneManager.LoadScene("Gameplay");
            }
            else
            {
                Debug.LogError("Firebase sign-in failed: " + task.Exception?.Message);
            }
        });
    }

    public void SaveSessionData(int score, string sessionId, string startTime, string endTime)
    {
        GameEvents.FirebaseSyncStartedEvent("SaveSession");

        string path = $"sessions/{user.UserId}/{sessionId}";
        Dictionary<string, object> sessionData = new()
        {
            {"score", score},
            {"startTime", startTime},
            {"endTime", endTime}
        };

        dbRef.Child(path).SetValueAsync(sessionData).ContinueWith(task =>
        {
            bool success = !(task.IsCanceled || task.IsFaulted);
            GameEvents.FirebaseSyncCompletedEvent("SaveSession", success);
        });
    }

    public void LoadTopScores()
    {
        GameEvents.FirebaseSyncStartedEvent("LoadLeaderboard");

        dbRef.Child("leaderboard").OrderByChild("score").LimitToLast(5).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                List<LeaderboardPlayerData> topScores = new();
                foreach (var item in task.Result.Children)
                {
                    LeaderboardPlayerData entry = JsonUtility.FromJson<LeaderboardPlayerData>(item.GetRawJsonValue());
                    topScores.Add(entry);
                }
                topScores.Reverse();
                GameEvents.LeaderboardLoadedEvent(topScores.ToArray());
                GameEvents.FirebaseSyncCompletedEvent("LoadLeaderboard", true);
            }
            else
            {
                GameEvents.FirebaseSyncCompletedEvent("LoadLeaderboard", false);
            }
        });
    }

    public void SetUserProperties(int playerLevel, int sessionCount, string lastActiveDate, string preferredPotionType)
    {
        FirebaseAnalytics.SetUserProperty("player_level", playerLevel.ToString());
        FirebaseAnalytics.SetUserProperty("session_count", sessionCount.ToString());
        FirebaseAnalytics.SetUserProperty("last_active_date", lastActiveDate);
        FirebaseAnalytics.SetUserProperty("preferred_potion_type", preferredPotionType);
    }
}
