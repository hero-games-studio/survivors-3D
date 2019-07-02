using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activation : MonoBehaviour
{
    private void OnEnable()
    {
        foreach (Transform ts in transform)
        {
            ts.gameObject.SetActive(true);
        }
    }

    public void cleanSpawners()
    {
        foreach (Spawner sp in GetComponentsInChildren<Spawner>())
        {
            sp.release();
        }
    }

    private void OnDisable()
    {
        foreach (Transform ts in transform)
        {
            ts.gameObject.SetActive(false);
        }
    }
}
