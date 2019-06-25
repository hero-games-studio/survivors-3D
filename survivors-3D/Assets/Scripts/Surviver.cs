using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surviver : MonoBehaviour
{

    public bool isSurvived;
    public Rigidbody rb;
    [SerializeField] private float offset = .1f;
    [SerializeField] private Vector3 followAnchor = new Vector3(0,0.5f,0);
    public float survivedNumber;

    //[SerializeField] private ConfigurableJoint joint;


    [SerializeField] private Rigidbody connectedRB;

    /*
    private float distance;
    private float spring = 0.1f;
    private float damper = 5f;
    */

    [SerializeField] private ParticleSystem smiles;


    private float allowedDistance = 2.5f;
    private float minSpeed = .5f;
    private float maxSpeed = 3f;
    private float disDiv = 10f;
    private float targetDistance;
    private float followSpeed;
    private RaycastHit shot;

    private float oldSpeed;

    [SerializeField] private GameObject sailport;


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


        if (isSurvived)
        {
            transform.LookAt(connectedRB.transform);
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out shot))
            {
                transform.LookAt(connectedRB.transform);

                targetDistance = shot.distance;
                /*
                if (targetDistance >= allowedDistance)
                {
                    followSpeed = 0.18f;
                    transform.position = Vector3.MoveTowards(transform.position, connectedRB.position, followSpeed);
                }
                else
                {
                    followSpeed = 0;

                }*/


                float newSpeed = Mathf.Clamp(((targetDistance-minSpeed)/disDiv), minSpeed, maxSpeed);


                //Debug.Log(targetDistance + " || " + followSpeed + " || " + (targetDistance / 10));
                if (connectedRB.position.z > transform.position.z)
                {
                    rb.velocity = (connectedRB.position - transform.position)* (connectedRB.position - transform.position).z;
                }
                //transform.position = Vector3.Lerp(transform.position, connectedRB.position, Mathf.Lerp(oldSpeed, newSpeed, Time.deltaTime * 20));
                //transform.position = Vector3.MoveTowards(transform.position, connectedRB.position, Mathf.Lerp(oldSpeed, newSpeed, Time.deltaTime*20));
            }

        }
    }

    internal void endGame()
    {
        smiles.Play();
        isSurvived = false;
        rb.velocity = new Vector3(0,0,0);
        rb.velocity = (sailport.transform.position - transform.position) * 50 * 0.03f;
        Destroy(gameObject,1f);
    }

    private void FixedUpdate()
    {
        /*
        if (isSurvived)
        {

            var connection = rb.position - connectedRB.position;
            var distanceDiscrepancy = distance - connection.magnitude;

            rb.position += distanceDiscrepancy * connection.normalized;

            var velocityTarget = connection + (rb.velocity + Physics.gravity * spring);
            var projectOnConnection = Vector3.Project(velocityTarget, connection);

            rb.velocity = (velocityTarget - projectOnConnection) / (1 + damper * Time.fixedDeltaTime);
        }*/

    }


    public void rescue(Rigidbody followTO)
    { 
        isSurvived = true;
        connectedRB = followTO;
        smiles.Play();
        gameObject.GetComponentInParent<SceneManager>().deleteFromObjectList(gameObject);
        //distance = Vector3.Distance(rb.position, connectedRB.position) + offset;
        //allowedDistance = Vector3.Distance(rb.position, connectedRB.position) + offset;
        //joint.connectedBody = followTO;
    }
}
