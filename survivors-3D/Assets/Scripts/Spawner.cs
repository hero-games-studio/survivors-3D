using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{


    [SerializeField] List<string> ObjectList = new List<string>();
    public string selectedObject;
    public GameObject child = null;
    [SerializeField] bool randomY = false;
    [SerializeField] Vector3 position;
    [SerializeField] Vector3 spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        position = new Vector3(transform.position.x,.5f,0);

        foreach (ObjectPooler.Pool pool in ObjectPooler.Instance.pools)
        {
            ObjectList.Add(pool.tag);
        }

        if (selectedObject == null || selectedObject == "")
        {
            selectedObject = ObjectList[UnityEngine.Random.Range(0, ObjectList.Count)];
        }

        if (randomY)
        {
            position.x = UnityEngine.Random.Range(-SceneManager.Instance.wallDis, SceneManager.Instance.wallDis);
        }


        spawnPoint = new Vector3(position.x, position.y, transform.position.z);

        if (child == null)
        {
            child = ObjectPooler.Instance.SpawnFromPool(selectedObject, spawnPoint);
        }
    }


    private void OnEnable()
    {

        if (randomY)
        {
            position.x = UnityEngine.Random.Range(-SceneManager.Instance.wallDis, SceneManager.Instance.wallDis);
        }

        spawnPoint = new Vector3(position.x, position.y, transform.position.z);

        if (child == null)
        {
            child = ObjectPooler.Instance.SpawnFromPool(selectedObject, spawnPoint);
        }

        child.transform.SetParent(transform);
    }


    internal void release()
    {
        child.transform.SetParent(ObjectPooler.Instance.transform);
    }

    private void OnDisable()
    {
        if (selectedObject.Equals("Surviver"))
        {
            if (!child.GetComponent<Surviver>().isSurvived)
            {
                child.SetActive(false);
            }
        }
        else
        {
            child.SetActive(false);
        }
        child = null;
    }
}
