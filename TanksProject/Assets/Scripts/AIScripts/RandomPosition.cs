using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomPosition : MonoBehaviour {

    NavMeshAgent nav;
    float timec = 0;

    public int radius = 5;

    // Use this for initialization
    void Start () {
        nav = gameObject.GetComponent<NavMeshAgent>();
        

    }
	
	// Update is called once per frame
	void Update () {
        if (timec > 4)
        {
            RandomDodge();
            timec = 0;
        }
        timec += Time.deltaTime;
    }

    void RandomDodge()
    {
        Debug.Log("Ich Dodge Jetzt");
        //Vector2 randpos = Random.insideUnitCircle;
        Vector3 fin = Random.insideUnitSphere * radius + new Vector3(5,0,5);
        transform.position += new Vector3(fin.x, transform.position.y,fin.z);
    }
}
