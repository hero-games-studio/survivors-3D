using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surviver : MonoBehaviour
{

    public bool isSurvived;
    public Rigidbody rb;
    [SerializeField] private Transform player;
    [SerializeField] private float offset = 2f;
    [SerializeField] private Vector3 followAnchor = new Vector3(0,0.5f,0);
    public float survivedNumber;

    //[SerializeField] private ConfigurableJoint joint;

    
    private Rigidbody connectedRB;

    private float distance;
    private float spring = 0.1f;
    private float damper = 5f;


    /*
    private float allowedDistance = 2.5f;
    private float targetDistance;
    private float followSpeed;
    private RaycastHit shot;
    */


    // Start is called before the first frame update
    void Start() 
    {
        isSurvived = false;
        rb = GetComponent<Rigidbody>();
        //joint = GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void Update()
    {

        /*
        if (isSurvived)
        {
            transform.LookAt(connectedRB.transform);
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out shot))
            {
                targetDistance = shot.distance;
                if(targetDistance >= allowedDistance)
                {
                    followSpeed = 0.1f;
                    transform.position = Vector3.MoveTowards(transform.position,connectedRB.position,followSpeed);
                }
                else
                {
                    followSpeed = 0;

                }
            }

        }
        */      

    }

    private void FixedUpdate()
    {

        if (isSurvived)
        {

            var connection = rb.position - connectedRB.position;
            var distanceDiscrepancy = distance - connection.magnitude;

            rb.position += distanceDiscrepancy * connection.normalized;

            var velocityTarget = connection + (rb.velocity + Physics.gravity * spring);
            var projectOnConnection = Vector3.Project(velocityTarget, connection);

            rb.velocity = (velocityTarget - projectOnConnection) / (1 + damper * Time.fixedDeltaTime);
        }

    }


    public void rescue(Rigidbody followTO)
    { 
        isSurvived = true;

        connectedRB = followTO;
        distance = Vector3.Distance(rb.position, connectedRB.position) + offset;
        //allowedDistance = Vector3.Distance(rb.position, connectedRB.position) + offset;
        //joint.connectedBody = followTO;
    }
}
