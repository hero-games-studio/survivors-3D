using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField] private Vector3 offset = new Vector3(0,10,-7);
    Transform pt;

    // Start is called before the first frame update
    void Start()
    {
        pt = PlayerManager.Instance.player.transform;
        StartCoroutine("FollowRoutine");

    }

    IEnumerator FollowRoutine()
    {
        while (true)
        {
            transform.position = pt.position + offset;
            yield return null;
        }
    }

}
