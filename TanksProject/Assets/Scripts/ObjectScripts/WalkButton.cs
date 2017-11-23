using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkButton : MonoBehaviour {


    //Grafik des Buttons von Rot auf Grün
    public bool active = false;
    public Renderer[] colorLight;
    private Color signalColor;

    //Object fuer den der Button bestimmt ist
    public GameObject [] actionObjects;
    public float maxActiveTime;
    private float activeTime = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            activeTime += Time.deltaTime;
            if(activeTime >= maxActiveTime)
            {
                active = false;
                activeTime = 0;
                SetButton(Color.red);
                Deactivate();
            }
        }

	}

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            signalColor = Color.green;
            for (int i = 0; i < colorLight.Length; i++)
            {
                colorLight[i].material.color = signalColor;
            }
            active = true;
            Activate();
        }
    }

    void SetButton(Color color)
    {
        for (int i = 0; i < colorLight.Length; i++)
        {
            colorLight[i].material.color = color;
        }
    }

    void Activate()
    {
        for (int i = 0; i < actionObjects.Length; i++)
        {
            actionObjects[i].SetActive(false);
        }
    }

    void Deactivate()
    {
        for (int i = 0; i < actionObjects.Length; i++)
        {
            actionObjects[i].SetActive(true);
        }
    }

}
