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
    private Vector3 wanderPoint;

    private bool canHunt;
    private bool hunting;
    private Transform target;
    [SerializeField] private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        canHunt = false;
        hunting = false;
        //target = PlayerManager.Instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        agent.Warp(transform.position);

        wanderPoint = WanderPosition();
    }

    // Update is called once per frame
    void Update()
    {

        float playerDis = Vector3.Distance(transform.position, PlayerManager.Instance.player.transform.position);
        if(playerDis <= lookRadius)
        {
            canHunt = true;
        }
        else
        {
            canHunt = false;
            hunting = false;
        }


        if (!hunting && canHunt)
        {
            if(PlayerManager.Instance.player.GetComponent<Rescue>().salvage.Count > 0)
            {
                target = PlayerManager.Instance.player.GetComponent<Rescue>().salvage.Peek().transform;
                hunting = true;
            }
        }

        if (hunting && canHunt)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance <= lookRadius)
            {
                agent.SetDestination(target.position);
                FaceTarget();

                if(distance <= attackRadius)
                {
                    PlayerManager.Instance.player.GetComponent<Rescue>().FirstSurviverDie();
                    //SceneManager.Instance.killObstacle(gameObject);
                    gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    gameObject.SetActive(false);
                    hunting = false;
                    canHunt = false;
                }
            }
        }
        else
        {
            wander();
        }

    }

    private void wander()
    {
        if(Vector3.Distance(wanderPoint, transform.position) < 0.5f)
        {
            wanderPoint = WanderPosition();
        }

        agent.SetDestination(wanderPoint);
    }

    private Vector3 WanderPosition()
    {
        Vector3 randomPosition = (UnityEngine.Random.insideUnitSphere * wanderRadius) + transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomPosition, out navHit, wanderRadius, -5);
        return navHit.position;
    }


    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime*5f);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

}
