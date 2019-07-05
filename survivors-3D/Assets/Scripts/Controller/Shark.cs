using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shark : MonoBehaviour
{


    [SerializeField] private float lookRadius = 10f;
    [SerializeField] private float attackRadius = 1f;

    [SerializeField] private float wanderRadius = 10f;
    


    [SerializeField] Transform target;
    [SerializeField] private NavMeshAgent agent;


    public List<Transform> moveSpots = new List<Transform>();
    [SerializeField] Transform wanderPoint;
    private int spot;
    private int randomSpot;



    // Start is called before the first frame update
    void Start()
    {
        target = null;
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.Warp(transform.position);
        spot = 0;
    }

    private void OnDisable()
    {
        target = null;
        spot = 0;
    }

    // Update is called once per frame
    void Update()
    {  
        if (target == null)
        {
            float playerDis = Vector3.Distance(transform.position, PlayerManager.Instance.player.transform.position);

            if (playerDis <= lookRadius && PlayerManager.Instance.player.GetComponent<Rescue>().salvage.Count > 0)
            {
                target = PlayerManager.Instance.player.GetComponent<Rescue>().salvage[0].transform;
            }
            else
            {
                if (moveSpots.Count > 0)
                {
                    wander();
                }
            }

        }
        else
        {

            if (!target.gameObject.activeSelf)
            {
                target = null;
            }
            else
            {
                float distance = Vector3.Distance(transform.position, target.position);

                if (distance <= lookRadius)
                {
                    agent.SetDestination(target.position);
                    FaceTarget(target);

                    if (distance <= attackRadius)
                    {
                        PlayerManager.Instance.player.GetComponent<Rescue>().FirstSurviverDie();
                        gameObject.SetActive(false);
                    }
                }
            }
            
        }
        
    }

    internal void setMovePoints(GameObject path)
    {
        while(moveSpots.Count > 0)
        {
            moveSpots.RemoveAt(0);
        }
        moveSpots.Clear();

        //Transform[] points = path.GetComponentsInChildren<Transform>();
        foreach(Transform ts in path.GetComponent<PathPoints>().points)
        {
            moveSpots.Add(ts);
        }

        spot = 0;
        //randomSpot = UnityEngine.Random.Range(0, moveSpots.Count - 1);
        wanderPoint = moveSpots[randomSpot];
    }

    private void wander()
    {

        if(Vector3.Distance(wanderPoint.position, transform.position) < 1f)
        {
            //randomSpot = UnityEngine.Random.Range(0, moveSpots.Count-1);
            spot++;
            if (spot == moveSpots.Count)
            {
                spot = 0;
            }
            wanderPoint = moveSpots[spot];
                //WanderPosition();
        }

        //transform.position = Vector3.MoveTowards(transform.position, wanderPoint.position, Time.deltaTime*agent.speed);
        FaceTarget(wanderPoint);
        agent.SetDestination(wanderPoint.position);
        //agent.SetDestination(wanderPoint);
    }


    private void FaceTarget(Transform fTarget)
    {
        Vector3 direction = (fTarget.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime*(agent.acceleration/8));
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

}
