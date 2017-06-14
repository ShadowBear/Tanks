using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCube : MonoBehaviour {

    public float health;
	
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(health <= 0)
        {
            Destroy(gameObject);
        }
	}

    public void TakeDamage(float amoung)
    {
        health -= Mathf.Abs(amoung);
    }
}
