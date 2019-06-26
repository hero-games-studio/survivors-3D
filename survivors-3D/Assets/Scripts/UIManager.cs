﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    #region Singelton
    private UIManager() { }

    public static UIManager Instance { get; private set; }

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


    [SerializeField] private Slider progressBar;
    [SerializeField] private Button runButton;
    [SerializeField] private Text levelText;
    [SerializeField] private Text rescueText;


    private SceneManager SM;
    private Gamemanager GM;

    // Start is called before the first frame update
    void Start()
    {
        SM = SceneManager.Instance;
        GM = Gamemanager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateProgress(float val)
    {
        progressBar.value = val;
    }


    public void OnPlay() {
        levelText.gameObject.SetActive(false);
        progressBar.gameObject.SetActive(true);
        runButton.gameObject.SetActive(false);
        rescueText.gameObject.SetActive(false);
    }

    public void OnWait()
    {
        levelText.text = "Level " + GM.level.ToString();
        rescueText.text = "You Saved " + GM.rescuedNum + " Person";
        rescueText.gameObject.SetActive(true);
        levelText.gameObject.SetActive(true);
        runButton.gameObject.SetActive(true);
        progressBar.gameObject.SetActive(false);
    }

    public void runButtonOnClick()
    {

        GM.runnGame();
    }

}