using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankFireRing : MonoBehaviour {
    public LayerMask destroy;
    public float m_ExplosionRadius = 2.75f;
    public LayerMask m_TankMask;

    public void OnTriggerEnter(Collider col)
    {
        //Invoke("FireDMG", 1f);
        FireDMG();
    }

    public void FireDMG()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask | destroy);
        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
            if (!targetRigidbody || colliders[i].CompareTag("Player") || colliders[i].CompareTag("Shell")) continue;

            //targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();
            DestroyCube cubeDes = targetRigidbody.GetComponent<DestroyCube>();
            if (!targetHealth && !cubeDes) continue;

            float damage = 10;

            if (targetHealth != null)
                targetHealth.TakeDamage(damage);
            if (cubeDes != null)
                cubeDes.TakeDamage((int)damage);
            print("Damage Taken");

            //print(damage);
        }
    }

}
