using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float distance = 3f;
    private Rigidbody rb;
    public Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0,10,-7);
    [SerializeField] private float offsetZ = -7f;
    [SerializeField] private float rotateRate = 2f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Quaternion playeRotate = player.rotation;

        //float rotateY = playeRotate.y;

        //if(rotateY < 0) { rotateY = -rotateY; }
        //offset.z = offsetZ + (rotateY * distance);
        transform.position = player.position + offset;

        //Quaternion camRotation = new Quaternion(transform.rotation.x, playeRotate.y/rotateRate, transform.rotation.z, transform.rotation.w);

        //transform.rotation = Quaternion.Lerp(transform.rotation, camRotation, Time.deltaTime);





    }

    private void FixedUpdate()
    {
        //Vector3 forw = Vector3.forward * speed * Time.deltaTime;
        //rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, forw.z);
    }
}
