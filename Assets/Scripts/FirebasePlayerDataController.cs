using Firebase.Extensions;
using Firebase.Firestore;
using System;
using UnityEngine;

public class FirebasePlayerDataController : MonoBehaviour
{
    public static FirebasePlayerDataController Instance { get; private set; }

    private FirebaseFirestore db;
    private Timestamp sessionStart;
    private Timestamp sessionEnd;
    private int finalScore;
    private string sessionId;

    public FirebaseGameDataService firebaseGameData;
    public PlayerData playerData;
    public event Action<PlayerData> OnPlayerDataReady;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        GameEvents.OnGameStarted += HandleGameStarted;
        GameEvents.OnGameEnded += HandleGameEnded;
    }

    void OnDisable()
    {
        GameEvents.OnGameStarted -= HandleGameStarted;
        GameEvents.OnGameEnded -= HandleGameEnded;
    }


    public void Init()
    {
        db = FirebaseFirestore.DefaultInstance;
        firebaseGameData = new FirebaseGameDataService();

        firebaseGameData.LoadPlayerData(OnPlayerDataLoaded);
    }

    private void OnPlayerDataLoaded(PlayerData data, bool success)
    {
        if (success && data != null)
        {
            Debug.Log($"[Firebase] Player Data Loaded: Score = {data.PlayerScore}");
            playerData = data;
            OnPlayerDataReady?.Invoke(playerData);
        }
        else
        {
            Debug.LogWarning("[Firebase] Failed to load player data or no data found.");
        }
    }


    private void HandleGameStarted(float timestamp, string id)
    {
        sessionStart = Timestamp.FromDateTime(DateTime.UtcNow);
        sessionId = id;

        GameEvents.FirebaseSyncStartedEvent("GameSessionStart");
    }

    private void HandleGameEnded(float timestamp, int score)
    {
        sessionEnd = Timestamp.FromDateTime(DateTime.UtcNow);
        finalScore = score;

        PlayerData playerData = new PlayerData
        {
            PlayerScore = finalScore,
            SessionStart = sessionStart,
            SessionEnd = sessionEnd
        };

        firebaseGameData.SaveLeaderboardScore(finalScore, success =>
        {
            if (success) Debug.Log("Leaderboard score uploaded.");
        });

        firebaseGameData.SavePlayerData(playerData, success =>
        {
            if (success) Debug.Log("Player data uploaded.");
        });
    }
}
