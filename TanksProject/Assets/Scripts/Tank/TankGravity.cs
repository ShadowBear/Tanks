using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankGravity : MonoBehaviour {

    public LayerMask m_GravityMask;
    public float m_GravityForce = 1000f;
    public float m_MaxLifeTime = 2f;
    public float m_GravityRadius = 5f;
    public Vector3 targetDirection;
    //private float distance = 0;
    public ParticleSystem gravityField;


    // Use this for initialization
    void Start () {
	}

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButton("Druckwelle"))
        {
            //print("Fire2 gedrückt");
            gravityField.Play();
            Collider[] colliders = Physics.OverlapSphere(transform.position, m_GravityRadius, m_GravityMask);

            // ********************* Druckwelle **************************//
            /*
            for (int i = 0; i < colliders.Length; i++)
            {
                Rigidbody rb = colliders[i].GetComponent<Rigidbody>();
                if (!rb) continue;
                rb.AddExplosionForce(m_GravityForce, transform.position, m_GravityRadius);
            }

            */

            // ******************** Anziehung ***************************//
            for (int i = 0; i < colliders.Length; i++)
            {
                Rigidbody rb = colliders[i].GetComponent<Rigidbody>();
                if (!rb) continue;
                targetDirection = colliders[i].transform.position - transform.position;
                //distance = targetDirection.magnitude;
                targetDirection = targetDirection.normalized;
                rb.AddForce(-targetDirection * m_GravityForce * Time.deltaTime);

            }


        }
        if (Input.GetButtonUp("Druckwelle"))
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, m_GravityRadius, m_GravityMask);

            // ********************* Druckwelle **************************//

            for (int i = 0; i < colliders.Length; i++)
            {
                Rigidbody rb = colliders[i].GetComponent<Rigidbody>();
                if (!rb) continue;
                rb.AddExplosionForce(m_GravityForce, transform.position, m_GravityRadius);
            }
            gravityField.Pause();
            gravityField.Clear();

        }
    }
}
