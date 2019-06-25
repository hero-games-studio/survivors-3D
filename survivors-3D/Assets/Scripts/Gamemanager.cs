using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour
{


    private Gamemanager() { }

    private static Gamemanager _instance;
    public static Gamemanager Instance {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("Gamemanager");
                go.AddComponent<Gamemanager>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }

    public int level = 1;
    public bool onPlay;
    public bool gameOver;

    [SerializeField] private GameObject player;

    public GameObject finishPoint;

    [SerializeField] private Slider progressBar;

    [SerializeField] private SceneManager SM;

    public int numOfSurviver = 0;



    // Start is called before the first frame update
    void Start()
    {
        onPlay = true;
        gameOver = false;
        SM.createPath(level);
    }

    // Update is called once per frame
    void Update()
    {
        progressBar.value = player.transform.position.z / finishPoint.transform.position.z;
    }

    public void finish()
    {
        onPlay = false;
        level++;
        player.GetComponent<Rescue>().releaseSurvivors();
        player.GetComponent<PlayerController>().enabled = false;
        numOfSurviver = 0;
    }
}
