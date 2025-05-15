using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int totalScore;


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        GameEvents.OnPotionCollected += HandleScore;
        FirebasePlayerDataController.Instance.OnPlayerDataReady += OnPlayerDataLoaded;
    }

    private void OnDisable()
    {
        GameEvents.OnPotionCollected -= HandleScore;
        FirebasePlayerDataController.Instance.OnPlayerDataReady -= OnPlayerDataLoaded;

    }
    private void OnPlayerDataLoaded(PlayerData data)
    {
        totalScore = data.PlayerScore;
        GameEvents.ScoreUpdatedEvent(totalScore, 0);
    }
    public void HandleScore(string potionType, int potency, float timestamp, GameObject obj)
    {
        AddScore(potency);
    }


    public void AddScore(int amount)
    {
        totalScore += amount;
        GameEvents.ScoreUpdatedEvent(totalScore, amount);
    }

    public int GetScore() => totalScore;
}
