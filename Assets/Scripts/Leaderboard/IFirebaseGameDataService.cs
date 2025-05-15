using System.Collections.Generic;
using System;

public interface IFirebaseGameDataService
{
    void SaveLeaderboardScore(int score, Action<bool> callback);
    void SavePlayerData(PlayerData playerData, Action<bool> callback);
}