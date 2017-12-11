using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingTank : MonoBehaviour {

    public Rigidbody m_Shell;
    public Transform m_FireTransform;
    public bool m_Fired = false;
    public float fireRateTime = 1f;
    public float smooth = 2.0f;

    public float reduceForHeight;
    //private Transform startPosition;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //startPosition = transform;
    }


    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player") && !m_Fired)
        {
            

            m_Fired = true;
            float launchForce = (player.transform.position - transform.position).magnitude - reduceForHeight;
            StartCoroutine(Fire(launchForce));
        }
        if (col.CompareTag("Player"))
        {
            Vector3 positionTank = (player.transform.position - transform.position);
            positionTank.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(positionTank), Time.deltaTime * smooth);
        }
    }

    //void  OnTriggerExit(Collider col)
    //{
    //    if (col.CompareTag("Player"))
    //    {
            
    //        print("verlasse Trigger");
    //    }
    //}

    IEnumerator Fire(float launchForce)
    {
        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        shellInstance.velocity = launchForce * m_FireTransform.forward;

        yield return new WaitForSeconds(fireRateTime);
        m_Fired = false;
        yield return null;
    }


}
