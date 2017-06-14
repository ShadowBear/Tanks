using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldCharackter : MonoBehaviour {

    public void OnTriggerEnter(Collider other)
    {
        other.transform.parent = gameObject.transform;
    }

    public void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;
    }
}
