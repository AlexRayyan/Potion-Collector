using UnityEngine;

[CreateAssetMenu(fileName = "NewPotion", menuName = "Potion Collector/Potion Data")]
public class PotionDataSO : ScriptableObject
{
    public string potionName;
    public int potency;
    public GameObject prefab;
}
