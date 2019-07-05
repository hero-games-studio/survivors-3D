using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rescue : MonoBehaviour
{

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
            surviver.rescue(rb);
            salvage.Add(other.gameObject);
        }
    }

    internal void FirstSurviverDie()
    {
        salvage[0].GetComponent<Surviver>().die();
        salvage.RemoveAt(0);

    }

    internal void releaseSurvivors()
    {
        while (salvage.Count > 0)
        {
            salvage[0].GetComponent<Surviver>().endGame();
            salvage.RemoveAt(0);
        }
        salvage.Clear();
    }

    internal void Reset()
    {
        while (salvage.Count > 0)
        {
            salvage[0].GetComponent<Surviver>().Reset();
            salvage.RemoveAt(0);
        }
        salvage.Clear();
    }
}