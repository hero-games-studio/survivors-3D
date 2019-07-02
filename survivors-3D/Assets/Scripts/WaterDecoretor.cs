using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaterDecoretor : MonoBehaviour
{

    public NavMeshSurface surface;

    // Start is called before the first frame update
    void Start()
    {
        if(surface != null)
        {
            surface.BuildNavMesh();
        }
    }

    public void resetNavMesh()
    {
        surface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
