﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public struct Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singelton

    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i=0; i<pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.transform.SetParent(transform);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }


    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {

        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log(tag + "doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.GetComponent<Rigidbody>().velocity = Vector3.zero;
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        /*
        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

        if (pooledObject != null)
        {
            pooledObject.OnSpawnObject();
        }
        */

        poolDictionary[tag].Enqueue(objectToSpawn);


        return objectToSpawn;
    }

    internal GameObject SpawnFromPool(string tag, Vector3 position)
    {

        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log(tag + "doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.GetComponent<Rigidbody>().velocity = Vector3.zero;
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = findPrefabByTag(tag).transform.rotation;
        

        /*
        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

        if (pooledObject != null)
        {
            pooledObject.OnSpawnObject();
        }
        */

        poolDictionary[tag].Enqueue(objectToSpawn);


        return objectToSpawn;
    }

    private GameObject findPrefabByTag(string tag)
    {
        foreach(Pool pool in pools)
        {
            if (pool.tag.Equals(tag))
            {
                return pool.prefab;
            }
        }

        return null;
    }
}
