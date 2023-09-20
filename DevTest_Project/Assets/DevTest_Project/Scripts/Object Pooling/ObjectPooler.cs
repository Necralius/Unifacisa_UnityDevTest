using System.Collections.Generic;
using UnityEngine;
using static NekraByte.Utility.DataTypes;

// --------------------------------------------------------------------------
// Name: ObjectPooler (Class)
// Desc: This class represents an ObjectPooler desing pattern, an pattern that
//       instance objects previously and maintains them, allowing them to be
//       reused, greatly improving performance and efficiency.
// --------------------------------------------------------------------------
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

    // ----------------------------------------------------------------------
    // Name: Start (Method)
    // Desc: This method is called on the game start, mainly the method
    //       prepare the object pooler, instantiating all objects, and
    //       managing them in a Queue of GameObjects.
    // ----------------------------------------------------------------------
    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (PoolData pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, pool.isAnCanvasObject ? GameManager.Instance.mainCanvas.transform.GetChild(0) : transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    // ----------------------------------------------------------------------
    // Name : SpawnFromPool (Method)
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
            Debug.LogWarning($"There is no pool with this tag {tag}, review the code syntax!");
            return null;
        }

        GameObject objectToSpawn = null;
        while (true)
        {
            objectToSpawn = poolDictionary[tag].Dequeue();
            if (!objectToSpawn.activeInHierarchy)
            {
                poolDictionary[tag].Enqueue(objectToSpawn);
                break;
            }
        }

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}