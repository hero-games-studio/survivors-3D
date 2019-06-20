﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnbreakableObstacle : MonoBehaviour
{

    [SerializeField] private Camera camera;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(camera.gameObject.transform.position.z > transform.position.z + 2)
        {
            Destroy(gameObject);
        }
    }
}
