using System.Collections.Generic;
using System;

public interface ILeaderboardService
{
    void GetTopPlayers(int count, Action<List<LeaderboardPlayerData>> callback);
}
