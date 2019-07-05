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
    private string lat = "";
    public float wallDis = 2.5f;

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
        if(tileName == lat)
        {
            spawnTile(RandomTile());
            return;
        }
        lat = tileName;
        lastCreatedObject = TilePooler.Instance.SpawnFromPool(tileName, Vector3.forward * spawnZ);
        objectsOnScene.Add(lastCreatedObject);
        spawnZ += tileLength;//go.transform.localScale.z * 10;
    }

    private void DeleteTile()
    {
        objectsOnScene[0].SetActive(false);
        objectsOnScene.RemoveAt(0);
    }

    IEnumerator pathRoutine()
    {
        while (true)
        {
            if ((spawnZ) <= ((gameLength - 1) * tileLength))
            {
                if (player.transform.position.z - safeZone > (spawnZ - tileOnScreen * tileLength))
                {
                    spawnTile(RandomTile());
                    DeleteTile();
                }
            }

            yield return null;
        }
    }

    public void createPath(int lv)
    {
        StartCoroutine("pathRoutine");
        gameLength = tileOnScreen + 1 + lv/10;
        spawnTile("start");
        spawnEnd();

        while (objectsOnScene.Count <= tileOnScreen)
        {
            spawnTile(RandomTile());
        }

        
    }

    private void spawnEnd()
    {
        finishPoint.transform.position = Vector3.forward * gameLength * tileLength;
        finishPoint.SetActive(true);
    }

    public void ReloadScene()
    {
        StopCoroutine("pathRoutine");

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
}
