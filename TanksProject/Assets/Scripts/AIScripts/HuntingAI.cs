using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HuntingAI : MonoBehaviour {

    NavMeshAgent agent;
    private float distanceToPlayer;
    private GameObject player;
    public float maxDistanceToPlayer = 5f;      
    public float smooth = 2.0f;
    private float fireRate = 2;
    private float fireCounter = 0;

    public LayerMask playerMask;    
    private bool isChasing;    
    public bool shootable = false;    
    public LayerMask enemyMask;
    public float walkRadius = 5;
    public bool shallDodge = false;


    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        player = GameObject.FindGameObjectWithTag("Player");
        Chase();
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.timeScale == 0) return;

         if(isChasing)
        {
            if (!shallDodge) Chase();
        }        
        
    }

    // Verfolge den Spieler mithilfe der NavMeshMap
    // Wenn Spieler nahe genug Attack
    void Chase()
    {

        isChasing = true;
        //Disntace zum Spieler berechnen
        agent.destination = player.transform.position;
        distanceToPlayer = Mathf.Abs((player.transform.position - transform.position).magnitude);

        if (distanceToPlayer <= maxDistanceToPlayer)
        {
            gameObject.GetComponent<AIShooting>().attack = true;
            ////Patroling Stoppen
            agent.isStopped = true;
        }
        else
        {
            gameObject.GetComponent<AIShooting>().attack = false;
        }
    }
}
