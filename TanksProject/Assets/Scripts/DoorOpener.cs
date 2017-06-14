using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{

    public Renderer [] colorLight;
    private Color signalColor;

    public Animator anim;

    public void Update()
    {
        if (anim.GetBool("open")) signalColor = Color.green;
        else signalColor = Color.red;
        for (int i = 0; i < colorLight.Length; i++)
        {
            colorLight[i].material.color = signalColor;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shell"))
        {
            anim.SetBool("open", true);
            anim.SetBool("doit", true);
        }
    }

}
