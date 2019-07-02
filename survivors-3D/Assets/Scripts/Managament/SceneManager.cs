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

    [SerializeField] GameObject player;
    public GameObject finishPoint;

    [SerializeField] private Vector3 playerStrartCor;
    [SerializeField] private Vector3 finishPointCor;



    [SerializeField] private List<GameObject> objectsOnScene;

    private GameObject lastCreatedObject;

    /*
    [SerializeField] private Vector3 DefaultSpawn = new Vector3(0, .5f, 0);
    [SerializeField] private float currentZ;
    [SerializeField] private float maxObsDis = 10f;
    [SerializeField] private float minObsDis = 2f;
    [SerializeField] private float freeSpace = 2f;
    [SerializeField] private Vector3 spawnPoint;
    
    [SerializeField] private float createSurviverRate = 30f;
    [SerializeField] private float removeDis = 10f;
    */
    public float wallDis = 2.5f;

    /*
private float length = 2;
private float width = 2;


[SerializeField] private float baseLength = 5f;
[SerializeField] private float additionalLevelLength = 2f;

[SerializeField] private int numberOfObjectOnScene = 10;
*/
    /*
    public ObjectPooler pool;
    private Gamemanager GM;
    
    */
    private PlayerManager PM;

    private float tileLength = 20.0f;
    private float safeZone = 12.5f;
    public int tileOnScreen = 4;
    private int lastTilesIndex = 0;
    private float spawnZ = 0f;


    [SerializeField] int gameLength = 0;
    [SerializeField] int currentLength = 0;


    // Start is called before the first frame update
    void Start()
    {
        playerStrartCor = new Vector3(0,.25f,0);
        /*
        finishPointCor = Vector3.zero;
        currentZ = freeSpace+minObsDis;
        spawnPoint = Vector3.zero;
        
        pool = ObjectPooler.Instance;
        GM = Gamemanager.Instance;
        */
        PM = PlayerManager.Instance;
        player = PM.player;
        objectsOnScene = new List<GameObject>();
    }

    private string RandomTile()
    {
        return TilePooler.Instance.pools[UnityEngine.Random.Range(1, TilePooler.Instance.pools.Count - 1)].tag;
    }

    private void spawnTile(string tileName)
    {
        lastCreatedObject = TilePooler.Instance.SpawnFromPool(tileName, Vector3.forward * spawnZ);
        objectsOnScene.Add(lastCreatedObject);
        spawnZ += tileLength;//go.transform.localScale.z * 10;
    }

    private void DeleteTile()
    {
        objectsOnScene[0].SetActive(false);
        objectsOnScene.RemoveAt(0);
    }

    void Update()
    {
        if ((spawnZ)  <= ((gameLength-1)*tileLength))
        {
            if (player.transform.position.z - safeZone > (spawnZ - tileOnScreen * tileLength))
            {
                spawnTile(RandomTile());
                DeleteTile();
            }
        }
    }

    public void createPath(int lv)
    {

        gameLength = tileOnScreen + lv;


        spawnTile("start");

        while (objectsOnScene.Count <= tileOnScreen)
        {
            spawnTile(RandomTile());
        }

        spawnEnd();
    }

    private void spawnEnd()
    {
        finishPoint.transform.position = Vector3.forward * gameLength * tileLength;
        finishPoint.SetActive(true);
    }

    public void ReloadScene()
    {
        finishPoint.SetActive(false);
        while (objectsOnScene.Count > 0)
        {
            deleteTiles();
        }
        objectsOnScene.Clear();

        finishPoint.SetActive(false);


        finishPointCor = Vector3.zero;
        lastTilesIndex = 0;
        spawnZ = 0f;

        //spawnPoint = Vector3.zero;

        gameLength = 0;
        currentLength = 0;

    player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.transform.position = playerStrartCor;
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void deleteTiles()
    {
        objectsOnScene[0].GetComponent<Activation>().cleanSpawners();
        objectsOnScene[0].SetActive(false);
        objectsOnScene.RemoveAt(0);
    }

    /*
    public void createPath(int lv)
    {
        length = lv * additionalLevelLength + baseLength;
        water.transform.localScale = new Vector3(2,1,length);
        water.transform.position = new Vector3(0, 0, (length * 5) - 5);

        finishPoint.SetActive(true);
        finishPoint.transform.position = new Vector3(0, 0, length * 8);

        Gamemanager.Instance.finishPoint = finishPoint;

        for(int i = 0; i < numberOfObjectOnScene; i++)
        {
            if ((finishPoint.transform.position.z - (maxObsDis + freeSpace + minObsDis)) > (currentZ))
            {
                OnObjectSpawn();
            }
        }

        water.GetComponent<WaterDecoretor>().resetNavMesh();
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
            lastCreatedObject = ObjectPooler.Instance.SpawnFromPool("Surviver", spawnPoint);
            Gamemanager.Instance.numOfSurviver += 1;
        }
        else if (rate>= createSurviverRate && rate < createSurviverRate+15)
        {
            lastCreatedObject = ObjectPooler.Instance.SpawnFromPool("Shark", spawnPoint);
        }
        else
        {//create obs
            lastCreatedObject = ObjectPooler.Instance.SpawnFromPool("Wood", spawnPoint);
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

    internal void killObstacle(GameObject obs)
    {
        obs.GetComponent<Rigidbody>().velocity = Vector3.zero;
        deleteFromObjectList(obs);
        obs.SetActive(false);
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
    */
}
