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

    public LayerMask playerMask;
    public float scanRadius;

    private bool alerted = false;

    public bool playerInSight = false;

    private bool isPatrolling;
    private bool isChasing;
    private bool isLookingFor;

    private bool scanning = false;
    private float scanCounter = 0f;

    public bool isHearable = false;
    public bool shootable = false;

    private EnemySight enemySight;
    public float alertRadius = 10;
    public LayerMask enemyMask;

    public float walkRadius = 5;


    //private State[] states;

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        nextPatrolPoint = 0;
        agent.autoBraking = false;

        enemySight = GetComponent<EnemySight>();

        player = GameObject.FindGameObjectWithTag("Player");
        Patrol();
        //Invoke("dodgeAttack", 5f); //Teste dodging 
        //dodgeAttack();
    }
	
	// Update is called once per frame
	void Update () {

        /*if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            dodgeAttack();
        }
        //Invoke("dodgeAttack", 5f); //Teste dodging 
        */

        playerInSight = enemySight.playerInSight;

        if (scanning)
        {
            ScanAround();
            return;
        }

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
                scanning = true;
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
        alerted = false;
    }

    void Chase()
    {

        isPatrolling = false;
        isLookingFor = false;
        isChasing = true;
        //Disntace zum Spieler berechnen
        agent.destination = player.transform.position;

        if (!alerted) AlertOther(player.transform.position);

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

    public void AlertFromOther(Vector3 position)
    {
        LookForThePlayer(position);
        alerted = true;
    }

    private void AlertOther(Vector3 position)
    {
        Collider[] others = Physics.OverlapSphere(transform.position, alertRadius, enemyMask);
        for (int i = 0; i < others.Length; i++)
        {
            if(others[i] != gameObject.GetComponent<Collider>())
            {
                if(others[i].GetComponent<AINavMesh>() != null) others[i].GetComponent<AINavMesh>().AlertFromOther(position);
            }
            
        }
        
    }

    private void ScanAround()
    {
        gameObject.transform.Rotate(0, Time.deltaTime * 90, 0);
        scanCounter += Time.deltaTime;
        agent.isStopped = true;
        Collider[] cols = Physics.OverlapSphere(transform.position, scanRadius, playerMask);
        if (cols.Length > 0)
        {
            scanning = false;
            Chase();
        }
        else if (scanCounter > 3)
        {
            scanCounter = 0f;
            scanning = false;
            agent.isStopped = false;
        }
    }


    private void dodgeAttack()
    {       

        Vector3 randomDodge = UnityEngine.Random.insideUnitSphere * walkRadius;

        randomDodge += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDodge, out hit, walkRadius, 1);
        Vector3 dodgePoint = hit.position;
        agent.SetDestination(dodgePoint);
    }


    void OnDrawGizmosSelected()
    {
        if (isChasing) Gizmos.color = Color.red;
        if (isLookingFor) Gizmos.color = Color.blue;
        if (isPatrolling) Gizmos.color = Color.green;

        Gizmos.DrawCube(new Vector3(transform.position.x + 2, transform.position.y, transform.position.z), new Vector3(1,1,1));
        //Debug.Log("ich Male");
        if (alerted) Gizmos.color = Color.red;
        else Gizmos.color = Color.green;

        Gizmos.DrawSphere(transform.position, 1);

    }

    void OnDrawGizmo()
    {
        
    }

}
