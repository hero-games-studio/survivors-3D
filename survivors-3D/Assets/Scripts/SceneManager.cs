using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    
    [SerializeField] private GameObject player;
    [SerializeField] public GameObject finishPoint;
    [SerializeField] public GameObject camera;

    private Gamemanager GM = Gamemanager.Instance;

    [SerializeField] private Vector3 playerStrartCor;
    [SerializeField] private Vector3 finishPointCor;


    [SerializeField] private GameObject water;
    [SerializeField] private GameObject underWater;

    [SerializeField] private List<GameObject> obstaclePool = new List<GameObject>();
    [SerializeField] private GameObject surviver;

    [SerializeField] private List<GameObject> objectsOnScene = new List<GameObject>();

    private GameObject lastCreatedObject;

    [SerializeField] private Vector3 DefaultSpawn = new Vector3(0, .5f, 0);
    [SerializeField] private float currentZ;
    [SerializeField] private float maxObsDis = 10f;
    [SerializeField] private float minObsDis = 2f;
    [SerializeField] private float wallDis = 2.5f;
    [SerializeField] private float freeSpace = 2f;
    [SerializeField] private Vector3 spawnPoint;

    [SerializeField] private float createSurviverRate = 30f;
    [SerializeField] private float removeDis = 10f;


    private float length = 2;
    private float width = 2;


    [SerializeField] private float baseLength = 5f;
    [SerializeField] private float additionalLevelLength = 2f;

    // Start is called before the first frame update
    void Start()
    {
        playerStrartCor = new Vector3(0,.25f,0);
        finishPointCor = Vector3.zero;
        currentZ = 0f;
        spawnPoint = Vector3.zero;



        //a1
        currentZ += UnityEngine.Random.Range(minObsDis, maxObsDis);
        spawnPoint = DefaultSpawn + new Vector3(UnityEngine.Random.Range(-wallDis, wallDis), 0, currentZ);
        lastCreatedObject = Instantiate(surviver, spawnPoint, surviver.transform.rotation) as GameObject;
        GM.numOfSurviver += 1;
        //start
        while ((finishPoint.transform.position.z - currentZ) > (maxObsDis + freeSpace))
        {
            spawnObject();
        }
    }


    public void createPath(int lv)
    {
        length = lv * additionalLevelLength + baseLength;
        width = length / 3;

        water.transform.localScale = new Vector3(width,1,length);
        underWater.transform.localScale = new Vector3(2, 1, length);
        Shader.SetGlobalFloat("RippleSimmness", width);


        water.transform.position = new Vector3(0, 0, (length * 5) - 5);
        underWater.transform.position = new Vector3(0, -.01f, (length * 5) - 5);

        finishPoint = Instantiate(finishPoint) as GameObject;
        finishPoint.transform.position = new Vector3(0, 0, length * 8);

        GM.finishPoint = finishPoint;
        currentZ += freeSpace;

    }

    // Update is called once per frame
    void Update()
    {
        if (objectsOnScene.Count >= 0)
        {
            if ((objectsOnScene[0].transform.position.z + removeDis) <= (player.transform.position.z))
            {
                deleteObstacle();
            }
        }
    }

    private void deleteObstacle()
    {
        Destroy(objectsOnScene[0]);
        objectsOnScene.RemoveAt(0);
    }


    void spawnObject()
    {
        
        currentZ += UnityEngine.Random.Range(minObsDis, maxObsDis);
        spawnPoint = DefaultSpawn + new Vector3(UnityEngine.Random.Range(-wallDis, wallDis), 0, currentZ);
        int rate = UnityEngine.Random.Range(0, 100);
        if(rate<createSurviverRate)

        {//create surviver
            lastCreatedObject = Instantiate(surviver, spawnPoint, surviver.transform.rotation) as GameObject;
            GM.numOfSurviver += 1;
        }
        else
        {//create obs
            GameObject newObject = obstaclePool[UnityEngine.Random.Range(0, (obstaclePool.Count - 1))];
            lastCreatedObject = Instantiate(newObject, spawnPoint, newObject.transform.rotation) as GameObject;
        }
        lastCreatedObject.transform.SetParent(transform);
        objectsOnScene.Add(lastCreatedObject);

    }

    public void deleteFromObjectList(GameObject dObject)
    {
        objectsOnScene.Remove(dObject);
    }
}
