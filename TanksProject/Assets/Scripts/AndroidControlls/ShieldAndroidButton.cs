using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShieldAndroidButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    TankMovement playerMovementScript;

    public void OnPointerDown(PointerEventData eventData)
    {
        playerMovementScript.shieldedAndroid = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        playerMovementScript.shieldedAndroid = false;
    }

    // Use this for initialization
    void Start () {
        playerMovementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<TankMovement>();
    }
}
