using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public List<PotionDataSO> potionTypes;
    public float spawnDelay = 2f;

    [Header("Spawn Area")]
    public Vector3 areaCenter = Vector3.zero;
    public Vector3 areaSize = new Vector3(5f, 0f, 5f);

    private Dictionary<string, GameObject> activePotions = new Dictionary<string, GameObject>();

    void OnEnable()
    {
        GameEvents.OnPotionCollected += HandlePotionCollected;
    }

    void OnDisable()
    {
        GameEvents.OnPotionCollected -= HandlePotionCollected;
    }

    void Start()
    {
        foreach (var potionData in potionTypes)
        {
            SpawnPotion(potionData);
        }
    }

    void SpawnPotion(PotionDataSO potionData)
    {
        if (activePotions.ContainsKey(potionData.potionName)) return;

        Vector3 spawnPos = areaCenter + new Vector3(
            Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
            Random.Range(-areaSize.y / 2f, areaSize.y / 2f),
            Random.Range(-areaSize.z / 2f, areaSize.z / 2f)
        );

        GameObject potionObj = PotionPoolManager.Instance.GetFromPool(potionData.potionName, potionData.prefab);
        if (potionObj == null)
        {
            Debug.LogWarning($"[PotionSpawner] Failed to get potion for {potionData.potionName}");
            return;
        }

        potionObj.transform.position = spawnPos;
        potionObj.SetActive(true);

        var potion = potionObj.GetComponent<Potion>();
        potion.Setup(potionData);

        activePotions[potionData.potionName] = potionObj;

        GameEvents.PotionSpawnedEvent(potionData.potionName, spawnPos);
    }

    void HandlePotionCollected(string potionType, int value, float timestamp, GameObject collectedPotion)
    {
        if (!activePotions.ContainsKey(potionType)) return;

        PotionPoolManager.Instance.ReturnToPool(potionType, collectedPotion);

        activePotions.Remove(potionType);

        StartCoroutine(RespawnPotionAfterDelay(potionType));
    }


    IEnumerator RespawnPotionAfterDelay(string potionType)
    {
        yield return new WaitForSeconds(spawnDelay);

        PotionDataSO data = potionTypes.Find(p => p.potionName == potionType);
        if (data != null)
        {
            SpawnPotion(data);
        }
    }
}
