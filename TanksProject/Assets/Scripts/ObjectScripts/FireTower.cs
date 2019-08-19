using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTower : MonoBehaviour {


    public float stateChangeTime = 7f;
    public float delayTimer = 2f;
    private ParticleSystem fireParticle;
    int i = 0;


	// Use this for initialization
	void Start () {

        fireParticle = gameObject.GetComponent<ParticleSystem>();
        InvokeRepeating("ChangeAttackState", delayTimer, stateChangeTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void ChangeAttackState()
    {
        //Debug.Log("Wechsel State JETZT");
        var x = fireParticle.shape;
        i++;
        if (i % 2 == 0)
            x.arcMode = ParticleSystemShapeMultiModeValue.PingPong;
        else
            x.arcMode = ParticleSystemShapeMultiModeValue.Loop;

    }
}
