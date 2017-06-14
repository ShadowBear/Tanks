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

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        nextPatrolPoint = 0;
        agent.autoBraking = false;

        player = GameObject.FindGameObjectWithTag("Player");
        Patrol();
    }
	
	// Update is called once per frame
	void Update () {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            Patrol();
        }
        //Disntace zum Spieler berechnen
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

    }

    private void Fire()
    {
        gameObject.GetComponent<TankShooting>().FireAI(distanceToPlayer);
    }

    void Patrol()
    {
        if (patrolPoints.Length > 0)
        {
            agent.destination = patrolPoints[nextPatrolPoint].position;
            nextPatrolPoint = (nextPatrolPoint + 1) % patrolPoints.Length;
        }
    }
}
