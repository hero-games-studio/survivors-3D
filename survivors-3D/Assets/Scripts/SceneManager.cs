using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{

    #region Singelton
    private SceneManager() { }

    public static SceneManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Destroy(gameObject);
        }

    }
    #endregion

    [SerializeField] private GameObject player;
    public GameObject finishPoint;

    [SerializeField] private Vector3 playerStrartCor;
    [SerializeField] private Vector3 finishPointCor;


    [SerializeField] private GameObject water;


    [SerializeField] private List<GameObject> objectsOnScene = new List<GameObject>();

    private GameObject lastCreatedObject;

    [SerializeField] private Vector3 DefaultSpawn = new Vector3(0, .5f, 0);
    [SerializeField] private float currentZ;
    [SerializeField] private float maxObsDis = 10f;
    [SerializeField] private float minObsDis = 2f;
    public float wallDis = 2.5f;
    [SerializeField] private float freeSpace = 2f;
    [SerializeField] private Vector3 spawnPoint;

    [SerializeField] private float createSurviverRate = 30f;
    [SerializeField] private float removeDis = 10f;


    private float length = 2;
    private float width = 2;

    
    [SerializeField] private float baseLength = 5f;
    [SerializeField] private float additionalLevelLength = 2f;

    [SerializeField] private int numberOfObjectOnScene = 10;

    private ObjectPooler pool;
    private Gamemanager GM;

    // Start is called before the first frame update
    void Start()
    {
        playerStrartCor = new Vector3(0,.25f,0);
        finishPointCor = Vector3.zero;
        currentZ = freeSpace+minObsDis;
        spawnPoint = Vector3.zero;

        pool = ObjectPooler.Instance;
        GM = Gamemanager.Instance;
    }


    public void createPath(int lv)
    {
        length = lv * additionalLevelLength + baseLength;
        water.transform.localScale = new Vector3(2,1,length);
        water.transform.position = new Vector3(0, 0, (length * 5) - 5);

        finishPoint.SetActive(true);
        finishPoint.transform.position = new Vector3(0, 0, length * 8);

        GM.finishPoint = finishPoint;

        for(int i = 0; i < numberOfObjectOnScene; i++)
        {
            if ((finishPoint.transform.position.z - (maxObsDis + freeSpace + minObsDis)) > (currentZ))
            {
                OnObjectSpawn();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (objectsOnScene.Count > 0)
        {
            if ((objectsOnScene[0].transform.position.z + removeDis) <= (player.transform.position.z))
            {
                deleteObstacle();

                if ((finishPoint.transform.position.z - (removeDis + freeSpace + minObsDis)) > (currentZ))
                {
                    OnObjectSpawn();
                }
            }
        }
    }

    private void deleteObstacle()
    {
        objectsOnScene[0].SetActive(false);
        objectsOnScene[0].GetComponent<Rigidbody>().velocity = Vector3.zero;
        objectsOnScene.RemoveAt(0);
    }


    public void OnObjectSpawn()
    {
        
        currentZ += UnityEngine.Random.Range(minObsDis, maxObsDis);
        spawnPoint = DefaultSpawn + new Vector3(UnityEngine.Random.Range(-wallDis, wallDis), 0, currentZ);
        int rate = UnityEngine.Random.Range(0, 100);
        if(rate<createSurviverRate)

        {//create surviver
            lastCreatedObject = pool.SpawnFromPool("Surviver", spawnPoint);
            GM.numOfSurviver += 1;
        }
        else
        {//create obs
            lastCreatedObject = pool.SpawnFromPool("Wood", spawnPoint);
        }

        objectsOnScene.Add(lastCreatedObject);

    }

    public void deleteFromObjectList(GameObject dObject)
    {
        objectsOnScene.Remove(dObject);

        if ((finishPoint.transform.position.z - (removeDis + freeSpace + minObsDis)) > (currentZ))
        {
            OnObjectSpawn();
        }
    }

    public void ReloadScene()
    {
        finishPoint.SetActive(false);
        while (objectsOnScene.Count > 0)
        {
            deleteObstacle();
        }
        objectsOnScene.Clear();
        finishPointCor = Vector3.zero;
        currentZ = freeSpace + minObsDis;
        spawnPoint = Vector3.zero;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.transform.position = playerStrartCor;
        player.transform.rotation = Quaternion.Euler(0,0,0);
    }
}
