using Firebase.Firestore;
using Firebase.Extensions;
using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class FirebaseGameDataService : IFirebaseGameDataService
{
    private FirebaseFirestore db;
    public PlayerData playerData;
    string userId;
    public FirebaseGameDataService()
    {
        db = FirebaseFirestore.DefaultInstance;
        
        userId = GoogleSignInManager.Instance.userId;
    }

    public void SaveLeaderboardScore(int score, Action<bool> callback)
    {
        var docRef = db.Collection("Leaderboard").Document(userId);

        var data = new Dictionary<string, object>
    {
        { "UserName", GoogleSignInManager.Instance.userDisplayName },
        { "Score", score }
    };

        GameEvents.FirebaseSyncStartedEvent("LeaderboardScore");

        docRef.SetAsync(data).ContinueWithOnMainThread(task =>
        {
            bool success = task.IsCompletedSuccessfully;

            if (success)
                Debug.Log($"[Firebase] Leaderboard score saved for {userId}");
            else
                Debug.LogWarning($"[Firebase] Failed to save leaderboard score for {userId}");

            GameEvents.FirebaseSyncCompletedEvent("LeaderboardScore", success);
            callback?.Invoke(success);
        });
    }


    public void SavePlayerData(PlayerData playerData, Action<bool> callback)
    {
        var docRef = db.Collection("Players").Document(userId);

        GameEvents.FirebaseSyncStartedEvent("PlayerData");

        docRef.SetAsync(playerData, SetOptions.MergeAll).ContinueWithOnMainThread(task =>
        {
            bool success = task.IsCompletedSuccessfully;

            if (success)
                Debug.Log($"[Firebase] Player data saved for {userId}");
            else
                Debug.LogWarning($"[Firebase] Failed to save player data for {userId}");

            GameEvents.FirebaseSyncCompletedEvent("PlayerData", success);
            callback?.Invoke(success);
        });
    }
    public void LoadLeaderboardData()
    {
        GameEvents.FirebaseSyncStartedEvent("LoadLeaderboard");

        db.Collection("Leaderboard")
          .OrderByDescending("Score")
          .Limit(5)
          .GetSnapshotAsync()
          .ContinueWithOnMainThread(task =>
          {
              if (task.IsCompletedSuccessfully)
              {
                  List<LeaderboardPlayerData> topScores = new();

                  foreach (var doc in task.Result.Documents)
                  {
                      Dictionary<string, object> data = doc.ToDictionary();

                      string userId = data.ContainsKey("UserId") ? data["UserId"].ToString() : "Unknown";
                      string userName = data.ContainsKey("UserName") ? data["UserName"].ToString() : "Anonymous";
                      int score = data.ContainsKey("Score") ? Convert.ToInt32(data["Score"]) : 0;

                      topScores.Add(new LeaderboardPlayerData(userId, userName, score));
                  }

                  GameEvents.LeaderboardLoadedEvent(topScores.ToArray());
                  GameEvents.FirebaseSyncCompletedEvent("LoadLeaderboard", true);
              }
              else
              {
                  Debug.LogError("[Firebase] Failed to load leaderboard.");
                  GameEvents.FirebaseSyncCompletedEvent("LoadLeaderboard", false);
              }
          });
    }

    public void LoadPlayerData(Action<PlayerData, bool> callback)
    {
        var docRef = db.Collection("Players").Document(userId);

        GameEvents.FirebaseSyncStartedEvent("LoadPlayerData");

        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            bool success = task.IsCompletedSuccessfully;
            playerData = null;

            if (success && task.Result.Exists)
            {
                try
                {
                    playerData = task.Result.ConvertTo<PlayerData>();
                    Debug.Log($"[Firebase] Loaded PlayerData for {userId}");
                    GameEvents.FirebaseSyncCompletedEvent("LoadPlayerData", true);
                    callback?.Invoke(playerData, true);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[Firebase] Failed to convert PlayerData: {e.Message}");
                    GameEvents.FirebaseSyncCompletedEvent("LoadPlayerData", false);
                    callback?.Invoke(null, false);
                }
            }
            else
            {
                Debug.LogWarning($"[Firebase] No PlayerData found for {userId}, initializing new data...");

                PlayerData playerData = new PlayerData
                {
                    PlayerScore = 0,
                    SessionStart = Timestamp.FromDateTime(DateTime.UtcNow),
                    SessionEnd = Timestamp.FromDateTime(DateTime.UtcNow)
                };

                SavePlayerData(playerData, saveSuccess =>
                {
                    GameEvents.FirebaseSyncCompletedEvent("LoadPlayerData", saveSuccess);
                    if (saveSuccess)
                    {
                        Debug.Log($"[Firebase] Default PlayerData created for {userId}");
                        callback?.Invoke(playerData, true);
                    }
                    else
                    {
                        Debug.LogError($"[Firebase] Failed to initialize PlayerData for {userId}");
                        callback?.Invoke(null, false);
                    }
                });
            }
        });
    }

}
