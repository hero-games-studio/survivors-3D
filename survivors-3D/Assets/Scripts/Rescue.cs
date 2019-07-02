using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rescue : MonoBehaviour
{

    public Queue<GameObject> salvage;
    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        salvage = new Queue<GameObject>();

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
                surviver.rescue(rb);
                salvage.Enqueue(other.gameObject);

            }
        }
    }

    internal void FirstSurviverDie()
    {
        salvage.Peek().GetComponent<Surviver>().die();
        salvage.Dequeue();

    }

    internal void releaseSurvivors()
    {
        while (salvage.Count > 0)
        {
            salvage.Peek().GetComponent<Surviver>().endGame();
            salvage.Dequeue();
        }
        salvage.Clear();
    }

    internal void Reset()
    {
        while (salvage.Count > 0)
        {
            salvage.Peek().GetComponent<Surviver>().Reset();
            salvage.Dequeue();
        }
        salvage.Clear();
    }
}