using UnityEngine;
using TMPro;
using System;
using Firebase.Firestore;

[Serializable]
public class LeaderboardPlayerData
{
    public string UserId;
    public string UserName;
    public int Score;

    public LeaderboardPlayerData(string userId, string userName, int score)
    {
        UserId = userId;
        UserName = userName;
        Score = score;
    }
}

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private LeaderboardUiItem leaderboardUiItemPrefab;
    [SerializeField] private Transform itemContainer;
    [SerializeField] private int topCount = 5;

    private ILeaderboardService leaderboardService;

    private void Awake()
    {
        leaderboardService = new LeaderboardService();
    }

    private void OnEnable()
    {
        GameEvents.OnLeaderboardLoaded += UpdateLeaderboardUI;
    }

    private void OnDisable()
    {
        GameEvents.OnLeaderboardLoaded -= UpdateLeaderboardUI;
    }

    public void LoadLeaderboard()
    {
        leaderboardService.GetTopPlayers(topCount, null);
    }

    private void UpdateLeaderboardUI(LeaderboardPlayerData[] topPlayers)
    {
        foreach (Transform child in itemContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < topPlayers.Length; i++)
        {
            var playerLeaderboardItem = Instantiate(leaderboardUiItemPrefab, itemContainer);
            playerLeaderboardItem.SetPlayerDataToLeaderboardUi(i + 1, topPlayers[i].UserName, topPlayers[i].Score);
        }
    }
}
