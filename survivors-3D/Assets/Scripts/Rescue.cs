using System;
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
                surviver.rescue(rb);
                salvage.Add(other.gameObject);

            }
        }
    }



    internal void releaseSurvivors()
    {
        rescueNumber = 0;
        while (salvage.Count > 0)
        {
            salvage[0].GetComponent<Surviver>().endGame();
            salvage.RemoveAt(0);
        }
        salvage.Clear();
    }

    internal void Reset()
    {
        rescueNumber = 0;
        while (salvage.Count > 0)
        {
            salvage[0].GetComponent<Surviver>().Reset();
            salvage.RemoveAt(0);
        }
        salvage.Clear();
    }
}