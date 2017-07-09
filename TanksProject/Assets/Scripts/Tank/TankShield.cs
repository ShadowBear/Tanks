using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShield : MonoBehaviour {

    public Renderer rend;
    public int maxPower = 1000;
    public int power;
    public Color matColor;


	// Use this for initialization
	void Start () {
        power = maxPower;
        //matColor = mat.GetColor("MainColor");
	}
	
	// Update is called once per frame
	void Update () {
        if (power > maxPower * 0.8)
        {
            matColor = new Color(0, 0, 1, 0.2156f);
        }
        if (power <= maxPower * 0.8 && power > maxPower * 0.6)
        {
            matColor = new Color(0, 1, 1, 0.2156f);
        }
        else if (power <= maxPower * 0.6 && power > maxPower * 0.4)
        {
            //55 255 0
            matColor = new Color(0.2156f, 1, 0, 0.2156f);
            Debug.Log("Sollte Grün zeigen");
        }
        else if (power <= maxPower * 0.4 && power > maxPower * 0.2)
        {
            //255 180 0
            Debug.Log("Sollte Orange zeigen");
            matColor = new Color(1, 0.70588f, 0, 0.2156f);
        }
        else if (power <= maxPower * 0.2 && power > 0)
        {
            matColor = new Color(1, 0, 0, 0.2156f);
        }
        else if (power <= 0)
        {

        }

        rend.materials[1].color = matColor;

    }
}
