using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPatrol : MonoBehaviour {

    public Transform[] patrolPoints;
    public Transform startPatrol;
    public int nextPatrolPoint = 0;
    public float accuracy = 5f;
    private Rigidbody rigid;
    public float speed = 5f;
    public float rotationSpeed = 5f;
    private Vector3 direction;
    
    // Use this for initialization
	void Start () {
        if (patrolPoints != null) startPatrol = patrolPoints[nextPatrolPoint];
        rigid = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        Patrol();
	}

    private void Patrol()
    {
        if ((patrolPoints[nextPatrolPoint].position - gameObject.transform.position).magnitude <= accuracy)
        {
            nextPatrolPoint = (nextPatrolPoint+1) % patrolPoints.Length;
        }
        //rigid.AddForce((patrolPoints[nextPatrolPoint].position - gameObject.transform.position).normalized * speed * Time.deltaTime);

        direction = patrolPoints[nextPatrolPoint].position - gameObject.transform.position;
        this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        this.transform.Translate(0, 0, Time.deltaTime * speed);


    }
}
