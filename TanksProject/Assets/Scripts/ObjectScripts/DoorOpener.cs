using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{

    public GameObject[] doorSwitches;

    public Animator anim;

    public void Update()
    {
        //if (anim.GetBool("open")) signalColor = Color.green;
        //else signalColor = Color.red;
        //for (int i = 0; i < colorLight.Length; i++)
        //{
        //    colorLight[i].material.color = signalColor;
        //}
    }


    public void CheckOpening()
    {

            for (int i = 0; i < doorSwitches.Length; i++) 
            {
                if (!doorSwitches[i].GetComponent<Door>().open)
                    return;
            }
            
            anim.SetBool("open", true);
            anim.SetBool("doit", true);
    }

}
