using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static NekraByte.Utility.DataTypes;

public class ObjectPooler : MonoBehaviour
{
    #region - Singleton Pattern -
    public static ObjectPooler Instance;
    private void Awake()
    {
        if (Instance != null) Destroy(Instance);
        Instance = this;
    }
    #endregion

    //Public Data
    [Header("Pools List")]
    [Tooltip("This list contains all pooled objects")]
    public List<PoolData> pools;

    //Private Data
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    // ------------------------------------------ Methods ------------------------------------------ //

    //
    // Name: Start
    // Desc: 
    //
    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (PoolData pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    // ----------------------------------------------------------------------
    // Name : SpawnFromPool
    // Desc : This method represents an spawn  action but using the object
    //        pooler desing pattern, the  method activates an object  that
    //        already has been instatiated, but assinging his position and
    //        rotation values, also finally the method returns the  object
    //        itself.
    // ----------------------------------------------------------------------
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("There is no pool with this tag, review the code syntax!");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}