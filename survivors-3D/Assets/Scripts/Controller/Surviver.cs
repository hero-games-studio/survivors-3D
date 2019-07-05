using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surviver : MonoBehaviour
{
    public Rigidbody rb;
    [SerializeField] private float offset = .1f;
    [SerializeField] private Vector3 followAnchor = new Vector3(0,0.5f,0);
    public float survivedNumber;
    public bool isSurvived;

    //[SerializeField] private ConfigurableJoint joint;


    [SerializeField] private Rigidbody connectedRB;

    /*
    private float distance;
    private float spring = 0.1f;
    private float damper = 5f;
    */

    [SerializeField] private ParticleSystem smiles;
    [SerializeField] private ParticleSystem death;


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


    IEnumerator followRoutine()
    {
        while (true)
        {
            transform.LookAt(connectedRB.transform);
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out shot))
            {
                transform.LookAt(connectedRB.transform);

                targetDistance = shot.distance;

                float newSpeed = Mathf.Clamp(((targetDistance - minSpeed) / disDiv), minSpeed, maxSpeed);


                //Debug.Log(targetDistance + " || " + followSpeed + " || " + (targetDistance / 10));
                if (connectedRB.position.z > transform.position.z)
                {
                    rb.velocity = (connectedRB.position - transform.position) * (connectedRB.position - transform.position).z;
                }
            }

            yield return null;
        }
    }


    public void die()
    {
        StopCoroutine("followRoutine");
        death.Play();
        connectedRB = null;
        Invoke("disableObject", 2);
    }

    internal void endGame()
    {
        StopCoroutine("followRoutine");
        smiles.Play();
        connectedRB = null;
        Invoke("disableObject",2);
    }

    internal void Reset()
    {
        StopCoroutine("followRoutine");
        connectedRB = null;
        disableObject();
    }

    private void disableObject()
    {
        isSurvived = false;
        rb.velocity = new Vector3(0, 0, 0);
        gameObject.SetActive(false);
    }

    public void rescue(Rigidbody followTO)
    {
        isSurvived = true;
        
        connectedRB = followTO;
        smiles.Play();
        transform.SetParent(ObjectPooler.Instance.transform);
        StartCoroutine("followRoutine");
        //gameObject.GetComponentInParent<Spawner>().child = null;
        //SceneManager.Instance.deleteFromObjectList(gameObject);
        //distance = Vector3.Distance(rb.position, connectedRB.position) + offset;
        //allowedDistance = Vector3.Distance(rb.position, connectedRB.position) + offset;
        //joint.connectedBody = followTO;
    }
}
