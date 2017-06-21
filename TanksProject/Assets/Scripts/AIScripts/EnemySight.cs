using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySight : MonoBehaviour {

    public float fieldOfViewAngle = 110f;
    public bool playerInSight;
    public Vector3 personalLastSighting;

    private NavMeshAgent nav;
    private AINavMesh aiController;
    private SphereCollider col;
    private GameObject player;
    private TankHealth playerHealth;
    private Vector3 previousSighting;


    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        aiController = GetComponent<AINavMesh>();
        col = GetComponent<SphereCollider>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<TankHealth>();
        
        previousSighting = player.transform.position;
    }

    void Update()
    {
        if (personalLastSighting != previousSighting)
        {
            personalLastSighting = player.transform.position;
        }
        previousSighting = player.transform.position;

        if (playerHealth.m_CurrentHealth <= 0)
        {
            playerInSight = false;
            aiController.isHearable = false;
            //Debug.Log(playerHealth.m_CurrentHealth + "Leben");
        }

    }


    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInSight = false;

            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            if(angle < fieldOfViewAngle * 0.5f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position + (transform.up/2),direction.normalized, out hit, col.radius))
                {
                    if (hit.collider.gameObject == player) {
                        playerInSight = true;
                        personalLastSighting = player.transform.position;
                        Debug.Log("Player in Sicht");
                        aiController.shootable = true;
                    }
                    aiController.shootable = false;

                }
            }
            //Teste ColRadius * 2
            if (CalculatePathLength(player.transform.position) <= col.radius * 0.5d)
            {
                personalLastSighting = player.transform.position;
                //Teste
                Debug.Log("Player in Hörweite");
                if(!playerInSight) aiController.isHearable = true;
            }
            else aiController.isHearable = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerInSight = false;
    }

    float CalculatePathLength(Vector3 targetPosition)
    {

        NavMeshPath path = new NavMeshPath();

        if (nav.enabled)
        {
            nav.CalculatePath(targetPosition, path);
        }

        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];
        allWayPoints[0] = transform.position;
        allWayPoints[allWayPoints.Length - 1] = targetPosition;

        for (int i = 0; i < path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }

        float pathLength = 0f;

        for (int i = 0; i < allWayPoints.Length-1; i++)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }

        return pathLength;
    }

}
