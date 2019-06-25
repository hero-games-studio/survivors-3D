using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float verticalSpeed = 300.0f;
    [SerializeField] private float horizontalSpeed = 150.0f;
    [SerializeField] private float accelerateSpeed = 3f;
    [SerializeField] private float turnSpeed = 15f;
    [SerializeField] private float sensitivity = 0.03f;
    [SerializeField] private float lerpTimeMul = 6f;
    private Rigidbody rb;

    [SerializeField] public float wallDistance = 2.5f;
    //[SerializeField] public float minCamDistance = 6.2f;

    [SerializeField] private float maxRotate= 55f;


    private float rotY;
    private float lastRotY;

    private Gamemanager GM = Gamemanager.Instance;

    private Vector2 lastMousePosition;
    private Quaternion lastRotation;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        Vector3 forw = Vector3.forward * verticalSpeed * Time.deltaTime;
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, forw.z);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.rotation.eulerAngles);
        //Debug.Log(Input.GetAxis("Horizontal") + " " + Input.GetAxis("Vertical"));
        Vector2 deltaPosition = Vector2.zero;


        if (Input.GetMouseButton(0))
        {
            Vector2 currentMousePosition = Input.mousePosition;

            if (lastMousePosition == Vector2.zero) { lastMousePosition = currentMousePosition; }

            deltaPosition = currentMousePosition - lastMousePosition;
            lastMousePosition = currentMousePosition;





            rotY += deltaPosition.x * Time.deltaTime * turnSpeed;
            rotY = Mathf.Clamp(rotY, -maxRotate, maxRotate);
            //if (rotY > maxRotate) { rotY = maxRotate; }
            //if (rotY < -maxRotate) { rotY = -maxRotate; }


            //transform.rotation = Quaternion.Euler(0,rotY,0);

            if(deltaPosition != Vector2.zero)
            {
                lastRotY = rotY;
                Quaternion currentRotation = Quaternion.Euler(0, rotY, 0);
                lastRotation = currentRotation;
                //Debug.Log(rotY + " -> " + lastRotation + " ||| " + currentRotation);
                transform.rotation = Quaternion.Lerp(transform.rotation, lastRotation, Time.deltaTime*lerpTimeMul);
                //transform.rotation = lastRotation;
            }


        }
        else
        {
            lastMousePosition = Vector2.zero;
            lastRotation = new Quaternion(0f, 0f, 0f, 1f);
            rotY = 0;//transform.rotation.eulerAngles.y;
            //rb.velocity = Vector3.zero;
        }




        Vector3 force = new Vector3(deltaPosition.x, 0, 0) * horizontalSpeed * sensitivity;
        //rb.AddForce(force);
        rb.velocity = new Vector3(force.x, rb.velocity.y, rb.velocity.z);


        Vector3 rotationForce = new Vector3(rb.rotation.y, 0, 0) * horizontalSpeed * sensitivity * accelerateSpeed;
        rb.velocity = new Vector3(rotationForce.x, rb.velocity.y, rb.velocity.z);
    }


    private void LateUpdate()
    {


        Vector3 pos = transform.position;

        if (pos.x > wallDistance) { pos.x = wallDistance; }
        if (pos.x < -wallDistance) { pos.x = -wallDistance; }

        /*
        if(pos.z < Camera.main.transform.position.z + minCamDistance)
        {
            pos.z = Camera.main.transform.position.z + minCamDistance;
        }
        */
        transform.position = pos;

    }

    public void OnTriggerEnter(Collider other)
    {
        if (GM.onPlay)
        {
            if (other.gameObject.CompareTag("Finish"))
            {
                GM.finish();
            }
        }
    }
}
