using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour {
    Gradient g;
    GradientColorKey[] gck;
    GradientAlphaKey[] gak;

    public Renderer cube;
    public float speed = 0.5f;


    // Use this for initialization
    void Start () {
        
        g = new Gradient();
        gck = new GradientColorKey[6];
        gck[0].color = Color.red;
        gck[0].time = 0.0F;
        gck[1].color = new Color(1, 0.70588f, 0);
        gck[1].time = 0.2F;
        gck[2].color = Color.yellow;
        gck[2].time = 0.4F;
        gck[3].color = Color.green;
        gck[3].time = 0.6F;
        gck[4].color = Color.cyan;
        gck[4].time = 0.8F;
        gck[5].color = Color.blue;
        gck[5].time = 1.0F;
        gak = new GradientAlphaKey[6];
        gak[0].alpha = 1.0F;
        gak[0].time = 0.0F;
        gak[1].alpha = 0.0F;
        gak[1].time = 0.2F;
        gak[2].alpha = 0.0F;
        gak[2].time = 0.4F;
        gak[3].alpha = 0.0F;
        gak[3].time = 0.6F;
        gak[4].alpha = 0.0F;
        gak[4].time = 0.8F;
        gak[5].alpha = 0.0F;
        gak[5].time = 1.0F;
        g.SetKeys(gck, gak);
        
    }
	
	// Update is called once per frame
	void Update () {
        cube.material.color =  g.Evaluate((Time.time * speed % 1f));
        //Debug.Log(g.Evaluate(Time.deltaTime));
    }
}
