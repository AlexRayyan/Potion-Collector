using TMPro;
using UnityEngine;

public class ScoreUiManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;

    private void OnEnable()
    {
        GameEvents.OnScoreUpdated += UpdateScoreText;
    }

    private void OnDisable()
    {
        GameEvents.OnScoreUpdated -= UpdateScoreText;

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void UpdateScoreText(int totalScore, int scoreDelta)
    {
        scoreText.text = "Score : " +totalScore.ToString();
    }
}
