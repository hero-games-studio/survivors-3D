using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gamemanager : MonoBehaviour
{

    #region Singelton
    private Gamemanager() { }

    public static Gamemanager Instance { get; private set; }

    private void Awake()
    {

        Application.targetFrameRate = 60;

        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Destroy(gameObject);
        }
        
    }
    #endregion

    public int level;
    public int rescuedNum;
    public bool onPlay;


    private GameObject player;

    public GameObject finishPoint;

    public int numOfSurviver = 0;

    private SceneManager SM;
    private UIManager UIM;
    private PlayerManager PM;

    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {

        GameData loadData = SaveSystem.LoadGameData();
        if (loadData != null)
        {
            level = loadData.level;
            rescuedNum = loadData.rescuedNum;
        }
        else
        {
            level = 1;
            rescuedNum = 0;
        }

        SM = SceneManager.Instance;
        UIM = UIManager.Instance;
        PM = PlayerManager.Instance;

        player = PM.player;


        UIM.OnWait();
        PlayerManager.Instance.player.GetComponent<PlayerController>().stop();
        SM.createPath(level);
        
    }


    public void play()
    {
        PlayerManager.Instance.player.GetComponent<PlayerController>().play();
        UIM.OnPlay();
        StartCoroutine("UpdateRoutine");
    }

    private void stop()
    {
        numOfSurviver = 0;
        PlayerManager.Instance.player.GetComponent<PlayerController>().stop();
        StartCoroutine(coroutine);
        StopCoroutine("UpdateRoutine");
    }



    // Update is called once per frame
    IEnumerator UpdateRoutine()
    {
        while (true)
        {
            UIM.UpdateProgress((player.transform.position.z / finishPoint.transform.position.z));
            yield return null;
        }
    }

    public void finish()
    {
        level++;
        rescuedNum += player.GetComponent<Rescue>().salvage.Count;
        player.GetComponent<Rescue>().releaseSurvivors();
        coroutine = resetScene(1f);
        SaveSystem.SaveGameData(Instance);
        stop();
    }

    public void Gameover()
    {
        player.GetComponent<Rescue>().Reset();
        coroutine = resetScene(0.5f);
        stop();
    }

    private IEnumerator resetScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SM.ReloadScene();
        SM.createPath(level);
        UIM.OnWait();
    }
}
