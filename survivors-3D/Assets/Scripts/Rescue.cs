﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rescue : MonoBehaviour
{

    public int rescueNumber;
    public List<GameObject> salvage;
    private Rigidbody rb;




    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        salvage = new List<GameObject>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Surviver"))
        {

            Surviver surviver = other.gameObject.GetComponent<Surviver>();


            if (!surviver.isSurvived)
            {
                rescueNumber += 1;
                /*
                if(salvage.Count == 0)
                {
                    surviver.rescue(rb);
                }
                else
                {
                    surviver.rescue(salvage[salvage.Count-1]);
                }*/
                surviver.rescue(rb);

            }

            salvage.Add(other.gameObject);
        }
    }



    internal void releaseSurvivors()
    {
        foreach(GameObject survived in salvage)
        {
            survived.GetComponent<Surviver>().endGame();
        }
    }
}