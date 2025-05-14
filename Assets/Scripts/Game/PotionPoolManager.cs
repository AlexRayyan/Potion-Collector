using System.Collections.Generic;
using UnityEngine;

public class PotionPoolManager : MonoBehaviour
{
    public static PotionPoolManager Instance;
    public Transform potionParent;
    private Dictionary<string, Queue<GameObject>> pool = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public GameObject GetFromPool(string potionKey, GameObject prefab)
    {
        if (!pool.ContainsKey(potionKey))
            pool[potionKey] = new Queue<GameObject>();

        if (pool[potionKey].Count > 0)
        {
            return pool[potionKey].Dequeue();
        }

        GameObject newObj = Instantiate(prefab, potionParent);
        return newObj;
    }

    public void ReturnToPool(string potionKey, GameObject obj)
    {
        if (!pool.ContainsKey(potionKey))
            pool[potionKey] = new Queue<GameObject>();

        obj.SetActive(false);
        pool[potionKey].Enqueue(obj);
    }
}
