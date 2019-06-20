using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{

    public int level = 1;
    public bool onPlay;
    public bool gameOver;

    public GameObject player;
    


    // Start is called before the first frame update
    void Start()
    {
        onPlay = true;
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void finish()
    {
        onPlay = false;
        level++;
        player.GetComponent<Rescue>().releaseSurvivors();
        player.GetComponent<PlayerController>().enabled = false;
    }
}
