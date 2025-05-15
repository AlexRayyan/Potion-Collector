using System;
using System.Collections.Generic;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;

public class LeaderboardService : ILeaderboardService
{
    private FirebaseFirestore db;

    public LeaderboardService()
    {
        db = FirebaseFirestore.DefaultInstance;
    }

    public void GetTopPlayers(int count, Action<List<LeaderboardPlayerData>> callback)
    {
        // Query Firestore collection "leaderboard", order by "score" descending, limit by count
        Query query = db.Collection("leaderboard")
                        .OrderByDescending("score")
                        .Limit(count);

        query.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogWarning("[LeaderboardService] Failed to fetch leaderboard.");
                callback?.Invoke(new List<LeaderboardPlayerData>()); // return empty list on failure
                return;
            }

            QuerySnapshot snapshot = task.Result;
            List<LeaderboardPlayerData> topPlayers = new List<LeaderboardPlayerData>();

            foreach (DocumentSnapshot doc in snapshot.Documents)
            {
                if (doc.Exists)
                {
                    // Deserialize document data into PlayerData
                    string userId = doc.Id;
                    Dictionary<string, object> data = doc.ToDictionary();

                    string userName = data.ContainsKey("userName") ? data["userName"].ToString() : "Unknown";
                    int score = 0;
                    if (data.ContainsKey("score"))
                    {
                        // Firestore stores numbers as long or double, convert safely
                        if (data["score"] is long longScore)
                            score = (int)longScore;
                        else if (data["score"] is double doubleScore)
                            score = (int)doubleScore;
                    }

                    LeaderboardPlayerData player = new LeaderboardPlayerData(userId, userName, score);
                    topPlayers.Add(player);
                }
            }

            // Return the list of top players
            callback?.Invoke(topPlayers);
        });
    }
}
