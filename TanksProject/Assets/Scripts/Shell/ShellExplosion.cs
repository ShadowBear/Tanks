using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;
    public LayerMask destroy;
    public LayerMask enemyMask;
    public ParticleSystem m_ExplosionParticles;       
    public AudioSource m_ExplosionAudio;              
    public float m_MaxDamage = 100f;                  
    public float m_ExplosionForce = 1000f;            
    public float m_MaxLifeTime = 2f;                  
    public float m_ExplosionRadius = 5f;

    public bool playerShell = false;
    public bool enemyShell = false;             


    private void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.isTrigger) return;
        // Find all the tanks in an area around the shell and damage them.

        Collider[] colliders;
        if (playerShell)
        {
            colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, enemyMask | destroy);
            print("SpielerSchuss o.O");

        }
        else colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask | destroy);

        //Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask | destroy );
        for(int i = 0; i < colliders.Length; i++)
        {
            if (playerShell)
            {
                AINavMesh targetEnemy = colliders[i].GetComponent<AINavMesh>();
                Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
                if (!targetEnemy && !targetRigidbody) continue;
                TankHealth targetHealth = null;
                DestroyCube cubeDes = null;
                if (targetEnemy != null) targetHealth = targetEnemy.GetComponent<TankHealth>();
                if(targetRigidbody != null) cubeDes = targetRigidbody.GetComponent<DestroyCube>();

                if (!targetHealth && !cubeDes) continue;

                //float damage = CalculateDamage(targetRigidbody.position);
                if (targetHealth != null)
                {
                    float damage = CalculateDamage(targetEnemy.GetComponent<Transform>().position);
                    targetHealth.TakeDamage(damage);
                    //print("Mach dem Gegner: " + damage +" Schaden");
                }
                if (cubeDes != null)
                {
                    float damage = CalculateDamage(targetRigidbody.position);
                    cubeDes.TakeDamage((int)damage);
                }
            }
            else
            {
                Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
                if (!targetRigidbody) continue;
                targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius, -1f);

                TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();
                DestroyCube cubeDes = targetRigidbody.GetComponent<DestroyCube>();
                if (!targetHealth && !cubeDes) continue;
                float damage = CalculateDamage(targetRigidbody.position);
                if (targetHealth != null) targetHealth.TakeDamage(damage);
                if (cubeDes != null) cubeDes.TakeDamage((int)damage);

                
            }
            //print(damage);
        }

        m_ExplosionParticles.transform.parent = null;
        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();

        Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.main.duration);
        Destroy(gameObject);

    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.
        Vector3 explosionToTarget = targetPosition - transform.position;
        float explosionDistance = explosionToTarget.magnitude;
        float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;

        float damage = relativeDistance * m_MaxDamage;

        damage = Mathf.Max(0f, damage);
        return damage;
    }
}