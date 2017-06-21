using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AINavMesh : MonoBehaviour {

    public Transform [] patrolPoints;
    NavMeshAgent agent;
    private int nextPatrolPoint;
    private float distanceToPlayer;
    private GameObject player;
    public float maxDistanceToPlayer = 5f;      
    public float smooth = 2.0F;
    private float fireRate = 2;
    private float fireCounter = 0;

    public bool playerInSight = false;

    private bool isPatrolling;
    private bool isChasing;
    private bool isLookingFor;

    public bool isHearable = false;
    public bool shootable = false;

    private EnemySight enemySight;

    //private State[] states;

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        nextPatrolPoint = 0;
        agent.autoBraking = false;

        enemySight = GetComponent<EnemySight>();

        player = GameObject.FindGameObjectWithTag("Player");
        Patrol();
    }
	
	// Update is called once per frame
	void Update () {

        playerInSight = enemySight.playerInSight;

        if (isPatrolling && playerInSight)
        {
            Chase();
        }
        else if(isPatrolling && !playerInSight)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                Patrol();
            }
        }
        else if(isChasing)
        {
            Chase();
        }
        else if(isLookingFor && playerInSight)
        {
            Chase();
        }
        else if (isLookingFor && !playerInSight)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                Patrol();
            }
        }
        if(isHearable && !playerInSight)
        {
            Chase();
            Debug.Log("ICh Höre und verfolge");
            //LookForThePlayer(player.transform.position);
        }

    }

    private void Fire()
    {
        gameObject.GetComponent<AIShooting>().FireAI(distanceToPlayer);
    }

    void Patrol()
    {
        isPatrolling = true;
        isLookingFor = false;
        isChasing = false;
        if (patrolPoints.Length > 0)
        {
            agent.destination = patrolPoints[nextPatrolPoint].position;
            nextPatrolPoint = (nextPatrolPoint + 1) % patrolPoints.Length;
        }
    }

    void Chase()
    {

        isPatrolling = false;
        isLookingFor = false;
        isChasing = true;
        //Disntace zum Spieler berechnen
        agent.destination = player.transform.position;
        distanceToPlayer = Mathf.Abs((player.transform.position - transform.position).magnitude);

        if (distanceToPlayer <= maxDistanceToPlayer)
        {
            //Patroling Stoppen
            agent.isStopped = true;
            //Gegner in die Richtung zum Spieler rotieren
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((player.transform.position - transform.position)), Time.deltaTime * smooth);
            fireCounter += Time.deltaTime;
            if (fireCounter > fireRate)
            {
                Fire();
                fireCounter = 0;
            }


        }
        else agent.isStopped = false;

        if (!playerInSight)
        {
            LookForThePlayer(player.transform.position);
        }
    }

    void LookForThePlayer(Vector3 lastPlayerPosition)
    {
        isChasing = false;
        isPatrolling = false;
        isLookingFor = true;

        agent.destination = lastPlayerPosition;
        if (agent.isStopped == true) agent.isStopped = false; 
    }


    void OnDrawGizmosSelected()
    {
        if (isChasing) Gizmos.color = Color.red;
        if (isLookingFor) Gizmos.color = Color.blue;
        if (isPatrolling) Gizmos.color = Color.green;

        Gizmos.DrawCube(new Vector3(transform.position.x + 2, transform.position.y, transform.position.z), new Vector3(1,1,1));
        //Debug.Log("ich Male");

    }

}
