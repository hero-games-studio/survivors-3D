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
    [SerializeField] private float lerpTimeMul = .3f;
    private Rigidbody rb;

    //private float wallDistance;
    //[SerializeField] public float minCamDistance = 6.2f;

    [SerializeField] private float maxRotate = 55f;


    private float rotY;
    private float lastRotY;

    private Vector2 lastMousePosition;
    private Quaternion lastRotation;

    private Gamemanager GM;
    private SceneManager SM;

    Vector2 deltaPosition = Vector2.zero;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastRotation = transform.rotation;

        GM = Gamemanager.Instance;
        SM = SceneManager.Instance;

        //wallDistance = SM.wallDis;



    }

    public void play()
    {
        StartCoroutine("FixedUpdateRoutine");
        StartCoroutine("UpdateRoutine");
    }
    public void stop()
    {
        StopCoroutine("FixedUpdateRoutine");
        StopCoroutine("UpdateRoutine");
    }


    IEnumerator FixedUpdateRoutine()
    { 
        while (true)
        {
            Vector3 forw = Vector3.forward * verticalSpeed * Time.deltaTime;

            if (deltaPosition != Vector2.zero)
            {
                lastRotY = rotY;
                Quaternion currentRotation = Quaternion.Euler(0, rotY, 0);
                lastRotation = currentRotation;
                GetComponent<Rigidbody>().rotation = Quaternion.Lerp(transform.rotation, lastRotation, lerpTimeMul);
            }

            Vector3 rotationForce = new Vector3(rb.rotation.y, 0, 0) * horizontalSpeed * sensitivity * accelerateSpeed;
            rb.velocity = new Vector3(rotationForce.x, rb.velocity.y, forw.z);

            yield return new WaitForFixedUpdate();
        }
    }

    

    IEnumerator UpdateRoutine()
    {

        while (true)
        {
            if ((transform.rotation.eulerAngles.x < 275 && transform.rotation.eulerAngles.x > 45) ||
                (transform.rotation.eulerAngles.x < -45 && transform.rotation.eulerAngles.x > -275) ||
                (transform.rotation.eulerAngles.z < 275 && transform.rotation.eulerAngles.z > 45) ||
                (transform.rotation.eulerAngles.z < -45 && transform.rotation.eulerAngles.z > -275) ||
                (transform.rotation.eulerAngles.y < 260 && transform.rotation.eulerAngles.y > 60) ||
                (transform.rotation.eulerAngles.y < -60 && transform.rotation.eulerAngles.y > -260))
            {
                Debug.Log("Gameover");
                GM.Gameover();
            }


            deltaPosition = Vector2.zero;

            if (Input.GetMouseButton(0))
            {
                Vector2 currentMousePosition = Input.mousePosition;

                if (lastMousePosition == Vector2.zero) { lastMousePosition = currentMousePosition; }

                deltaPosition = currentMousePosition - lastMousePosition;
                lastMousePosition = currentMousePosition;

                rotY += deltaPosition.x * Time.deltaTime * turnSpeed;
                rotY = Mathf.Clamp(rotY, -maxRotate, maxRotate);



            }
            else
            {
                lastMousePosition = Vector2.zero;
                lastRotation = new Quaternion(0f, 0f, 0f, 1f);
                rotY = 0;//transform.rotation.eulerAngles.y;
                         //rb.velocity = Vector3.zero;
            }

            

            yield return null;
        }
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
