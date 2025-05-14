using UnityEngine;

public class Potion : MonoBehaviour
{
    public PotionDataSO potionData;

    public void Setup(PotionDataSO data)
    {
        potionData = data;
    }

    public void Collect()
    {
        if (potionData == null)
        {
            Debug.LogError("[Potion] potionData is null!");
            return;
        }

        float timestamp = Time.time;
        GameEvents.PotionCollectedEvent(potionData.potionName, potionData.potency, timestamp, this.gameObject);
        ScoreManager.Instance.AddScore(potionData.potency);
    }
}
