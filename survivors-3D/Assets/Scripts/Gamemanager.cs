using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour
{

    public int level = 1;
    public bool onPlay;
    public bool gameOver;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject finishPoint;
    [SerializeField] private Slider progressBar;



    // Start is called before the first frame update
    void Start()
    {
        onPlay = true;
        gameOver = false;
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
    }
}
