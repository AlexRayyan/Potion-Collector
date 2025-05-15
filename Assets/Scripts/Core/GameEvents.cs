using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    // --- Game Lifecycle Events ---
    public static event Action<float, string> OnGameStarted;
    public static event Action<float> OnGamePaused;
    public static event Action<float> OnGameResumed;
    public static event Action<float, int> OnGameEnded;

    // --- Potion Events ---
    public static event Action<string, Vector3> OnPotionSpawned;
    public static event Action<string, int, float, GameObject> OnPotionCollected;

    // --- Score Event ---
    public static event Action<int, int> OnScoreUpdated;

    // --- Leaderboard Event ---
    public static event Action<LeaderboardPlayerData[]> OnLeaderboardLoaded;

    // --- Firebase Sync Events ---
    public static event Action<string> OnFirebaseSyncStarted;
    public static event Action<string, bool> OnFirebaseSyncCompleted;


    public static void GameStartedEvent(float timestamp, string sessionId)
    {
        Debug.Log($"GameStartedEvent Triggered at {timestamp} with SessionId: {sessionId}");
        OnGameStarted?.Invoke(timestamp, sessionId);
    }

    public static void GamePausedEvent(float timestamp)
    {
        Debug.Log($"GamePausedEvent Triggered at {timestamp}");
        OnGamePaused?.Invoke(timestamp);
    }

    public static void GameResumedEvent(float timestamp)
    {
        Debug.Log($"GameResumedEvent Triggered at {timestamp}");
        OnGameResumed?.Invoke(timestamp);
    }

    public static void GameEndedEvent(float timestamp, int totalScore)
    {
        Debug.Log($"GameEndedEvent Triggered at {timestamp} with TotalScore: {totalScore}");
        OnGameEnded?.Invoke(timestamp, totalScore);
    }

    public static void PotionSpawnedEvent(string potionType, Vector3 position)
    {
        Debug.Log($"PotionSpawnedEvent Triggered: {potionType} at {position}");
        OnPotionSpawned?.Invoke(potionType, position);
    }

    public static void PotionCollectedEvent(string potionType, int potency, float timestamp, GameObject obj)
    {
        OnPotionCollected?.Invoke(potionType, potency, timestamp, obj);
    }

    public static void ScoreUpdatedEvent(int newScore, int scoreDelta)
    {
        Debug.Log($"ScoreUpdatedEvent Triggered: NewScore={newScore}, Delta={scoreDelta}");
        OnScoreUpdated?.Invoke(newScore, scoreDelta);
    }

    public static void LeaderboardLoadedEvent(LeaderboardPlayerData[] topScores)
    {
        Debug.Log("LeaderboardLoadedEvent Triggered");
        OnLeaderboardLoaded?.Invoke(topScores);
    }

    public static void FirebaseSyncStartedEvent(string operationType)
    {
        Debug.Log($"FirebaseSyncStartedEvent Triggered: {operationType}");
        OnFirebaseSyncStarted?.Invoke(operationType);
    }

    public static void FirebaseSyncCompletedEvent(string operationType, bool success)
    {
        Debug.Log($"FirebaseSyncCompletedEvent Triggered: {operationType}, Success: {success}");
        OnFirebaseSyncCompleted?.Invoke(operationType, success);
    }
}
