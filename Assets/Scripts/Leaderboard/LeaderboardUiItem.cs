using TMPro;
using UnityEngine;

public class LeaderboardUiItem : MonoBehaviour
{
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text scoreText;

    public void SetPlayerDataToLeaderboardUi(int rank, string name, int score)
    {
        rankText.text = $"#{rank}";
        nameText.text = name;
        scoreText.text = score.ToString();
    }
}
