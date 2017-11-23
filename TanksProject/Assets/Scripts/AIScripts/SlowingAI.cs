using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlowingAI : MonoBehaviour {


    NavMeshAgent agent;
    GameObject player;
    Vector3 playerPosition;
    public float stopDistanceToPlayer;
    float distance;

    // Use this for initialization
	void Start () {
        agent = gameObject.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
        playerPosition = player.transform.position;
        agent.SetDestination(playerPosition);
        distance = Mathf.Abs((player.transform.position - transform.position).magnitude);
        agent.isStopped =  distance <= stopDistanceToPlayer ?  true : false;
        
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<TankMovement>().m_Speed *= 0.2f;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<TankMovement>().m_Speed /= 0.2f;
        }
    }
}
