using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public bool open = false;
    public Renderer[] colorLight;
    private Color signalColor;

    public DoorOpener doorScript;



    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Shell"))
        {
            signalColor = Color.green;            
            for (int i = 0; i < colorLight.Length; i++)
            {
                colorLight[i].material.color = signalColor;
            }
            open = true;

            doorScript.CheckOpening();

        }
    }
}
