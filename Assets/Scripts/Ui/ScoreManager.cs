using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private int totalScore;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AddScore(int amount)
    {
        totalScore += amount;
        GameEvents.ScoreUpdatedEvent(totalScore, amount);
    }

    public int GetScore() => totalScore;
}
