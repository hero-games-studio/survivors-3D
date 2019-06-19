using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rescue : MonoBehaviour
{

    public int rescueNumber;
    public List<Rigidbody> salvage;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        salvage = new List<Rigidbody>();
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

            salvage.Add(other.gameObject.GetComponent<Rigidbody>());
        }
    }


}