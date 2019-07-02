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
        onPlay = false;
        SM.createPath(level);
        
    }

    // Update is called once per frame
    void Update()
    {

        UIM.UpdateProgress((player.transform.position.z / finishPoint.transform.position.z));

    }


    IEnumerator Example()
    {
        yield return new WaitForSeconds(3);
    }

    public void runnGame()
    {
        onPlay = true;
        UIM.OnPlay();
    }

    public void finish()
    {

        
        level++;
        rescuedNum += player.GetComponent<Rescue>().salvage.Count;
        player.GetComponent<Rescue>().releaseSurvivors();
        numOfSurviver = 0;
        onPlay = false;

        coroutine = resetScene(1f);
        StartCoroutine(coroutine);

        SaveSystem.SaveGameData(Instance);

    }

    public void Gameover()
    {
        
        player.GetComponent<Rescue>().Reset();
        numOfSurviver = 0;
        onPlay = false;

        coroutine = resetScene(0.5f);
        StartCoroutine(coroutine);
    }



    private IEnumerator resetScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SM.ReloadScene();
        SM.createPath(level);
        UIM.OnWait();
    }
}
