using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour {


    public GameObject leaderAI;
    public GameObject[] follower;

    public Transform[] tacticPositions;
    public GameObject aiTactic;


    // Use this for initialization
	void Start () {
        InitialiseEnemys();
    }
	
	// Update is called once per frame
	void Update () {

        if (leaderAI == null) InitialiseEnemys();

        aiTactic.transform.position = leaderAI.transform.position;
        
        //leaderAI.GetComponent<NavMeshAgent>().SetDestination();

        
        if (follower.Length >= tacticPositions.Length)
        {
            //int positionCounter = follower.Length -1;
            for (int i = 1; i < tacticPositions.Length; i++)
            {

                if (leaderAI.GetComponent<NavMeshAgent>().isStopped == true)
                {
                    follower[i].GetComponent<HuntingAI>().isChasing = true;
                    continue;
                }

                follower[i].GetComponent<HuntingAI>().isChasing = false;
                follower[i].GetComponent<NavMeshAgent>().SetDestination(tacticPositions[i].position);
                if (!follower[i].GetComponent<NavMeshAgent>().pathPending && follower[i].GetComponent<NavMeshAgent>().remainingDistance < 0.5f)
                {
                    follower[i].GetComponent<NavMeshAgent>().speed = 3.5f;
                }
                else
                {
                    if(follower[i] != leaderAI) follower[i].GetComponent<NavMeshAgent>().speed = 5f;
                }
            }


        }     
  	}

    void InitialiseEnemys()
    {
        follower = GameObject.FindGameObjectsWithTag("Enemy");
        tacticPositions = new Transform[aiTactic.transform.childCount];
        for (int i = 0; i < aiTactic.transform.childCount; i++)
        {
            tacticPositions[i] = aiTactic.transform.GetChild(i).GetComponentInChildren<Transform>();
        }
        follower = GameObject.FindGameObjectsWithTag("Enemy");
        if(follower.Length > 0) leaderAI = follower[0];
    }

}


//follower[i].GetComponent<HuntingAI>().isChasing = false;
//if (!follower[i].GetComponent<NavMeshAgent>().pathPending && follower[i].GetComponent<NavMeshAgent>().remainingDistance < 0.5f)
//{
//    positionCounter--;
//    follower[i].GetComponent<NavMeshAgent>().isStopped = true;
//}
//else
//{
//    follower[i].GetComponent<NavMeshAgent>().SetDestination(tacticPositions[i].position);
//    follower[i].GetComponent<NavMeshAgent>().isStopped = false;
//}
//leaderAI.GetComponent<NavMeshAgent>().isStopped = (positionCounter != 0) ? true : false;
//if (positionCounter <= 0) { }
//{
//    positionCounter = follower.Length - 1;
//}