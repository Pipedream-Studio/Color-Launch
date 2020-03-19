using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    [SerializeField] private List<PrefabObject> prefabObjects;
    private List<PrefabQueue> prefabQueues;

    #region Singleton
    private static ObjectPoolingManager mInstance;

    public static ObjectPoolingManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<ObjectPoolingManager>();
                if (mInstance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(ObjectPoolingManager).Name;
                    mInstance = obj.AddComponent<ObjectPoolingManager>();
                }
            }
            return mInstance;
        }
    }

    public virtual void Awake()
    {
        if (mInstance == null)
        {
            mInstance = this as ObjectPoolingManager;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //Create a new list of [PrefabQueue]
        prefabQueues = new List<PrefabQueue>();

        //Instantiate all objects 
        InstantiateAllPrefabs();
    }
    #endregion

    public PrefabObject GetPrefabObject(string prefabName)
    {
        for (int i = 0; i < prefabQueues.Count; i++)
        {
            if(prefabQueues[i].QueueName != prefabName)
                continue;

            //If there is no available prefab object inside pool, instatiate a new one and add it inside pool
            if (prefabQueues[i].MyQueue.Count == 0)
            {
                CreateAndEnqueuePrefab(i);
            }
            
            return prefabQueues[i].MyQueue.Dequeue();
        }

        Debug.Log("Error! Prefab not assigned in ObjectPoolingManager! Please assign prefab in the inspector");
        return null;
    }

    public void SpawnPrefabObject(string prefabName, Vector2 spawnPos, float objectLifeSpan = Mathf.Infinity)
    {
        StartCoroutine(SpawnPrefab(prefabName, spawnPos, objectLifeSpan));
    }

    IEnumerator SpawnPrefab(string prefabName, Vector2 spawnPos, float objectLifeSpan)
    {
        var tempObject = GetPrefabObject(prefabName);
        if (tempObject == null)
        {
            Debug.LogError(prefabName + " : Error! Prefab not assigned in ObjectPoolingManager! Please assign prefab in the inspector");
            yield break;
        }

        tempObject.transform.position = spawnPos;
        tempObject.gameObject.SetActive(true);

        if (objectLifeSpan == Mathf.Infinity)
            yield break;

        else
            yield return new WaitForSeconds(objectLifeSpan);

        DespawnPrefabObject(tempObject, prefabName);
    }

    public void DespawnPrefabObject(PrefabObject prefabObject, string prefabName)
    {
        if (!prefabObject.gameObject.activeInHierarchy || prefabObject.gameObject == null)
            return;

        for (int i = 0; i < prefabQueues.Count; i++)
        {
            if (prefabName.Contains(prefabQueues[i].QueueName))
            {
                prefabObject.gameObject.SetActive(false);
                prefabQueues[i].MyQueue.Enqueue(prefabObject);
            }
        }
    }

    void InstantiateAllPrefabs()
    {
        //For every prefab object, 
        for(int i = 0; i < prefabObjects.Count; i++)
        {
            //Create a new prefab queue
            PrefabQueue newPrefabQueues = new PrefabQueue();

            //Add created queue into prefabQueues list
            prefabQueues.Add(newPrefabQueues);

            //Set created prefab queue name to prefab object name (Used for object pooling later on)
            if (prefabQueues[i] != null)
                prefabQueues[i].QueueName = prefabObjects[i].name;

            else
                Debug.LogError("PrefabQueue [" + i + "] is null! Current Queue size : " + prefabQueues.Count);

            //Create the prefab object based on the amount to spawn and enqueue it into the queue
            for (int j = 0; j < prefabObjects[i].AmountToSpawn; j++)
            {
                CreateAndEnqueuePrefab(i);
            }
        }
    }

    void CreateAndEnqueuePrefab(int prefabInt)
    {
        //Instantiate a new object
        PrefabObject prefab = Instantiate(prefabObjects[prefabInt]);

        //Disable newly instantiated object 
        prefab.gameObject.SetActive(false);

        //Enqueue it to current queue
        prefabQueues[prefabInt].MyQueue.Enqueue(prefab);
    }
}

[System.Serializable]
public class PrefabQueue //This class is used for all prefabs' queue. 
{
    public string QueueName;
    public Queue<PrefabObject> MyQueue = new Queue<PrefabObject>();
}
