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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateScoreText(int totalScore, int scoreDelta)
    {
        scoreText.text = "Score : " +totalScore.ToString();
    }
}
