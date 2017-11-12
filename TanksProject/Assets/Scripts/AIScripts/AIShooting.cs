using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.AI;

public class AIShooting : MonoBehaviour
{
    public Rigidbody m_Shell;
    public Transform m_FireTransform;
    public Transform m_FireTransform2;
    public Transform m_FireTransform3;
    public AudioSource m_ShootingAudio;
    public AudioClip m_FireClip;

    public float fireRateTime = 0.5f;
    

    public float m_MinLaunchForce = 10f;
    public float m_MaxLaunchForce = 30f;
    public float m_MaxChargeTime = 0.75f;
    public int burstRate = 10;
    public bool attack = false;

    public float test = 0.1f;
    public float smooth = 2.0f;


    private float m_CurrentLaunchForce;
    private float m_ChargeSpeed;
    private bool m_Fired;
    private bool specFire = false;

    float launchForce = 18f;

    private GameObject player;
    private NavMeshAgent AIAgent;
    //private bool rounding = false;
    //private float timer = 1;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        AIAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
    }

    void Update()
    {
        //if (attack && !m_Fired) m_Fired = true;
        //else if (!attack) m_Fired = false;
        if(IsInvoking("FireAI")) print("Der tud do no wat X.X");

        launchForce = (player.transform.position - transform.position).magnitude;
        if (launchForce <= 30f)
        {
            if (attack && !m_Fired) StartCoroutine(FireAI(launchForce));
        }
        else
        {
            attack = false;
        }

        if (AIAgent != null)
        {
            AIAgent.isStopped = attack ? true : false;
        }
        else attack = true;

        
        if (attack && !specFire) 
        {
            //zum Spieler Drehen
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((player.transform.position - transform.position)), Time.deltaTime * smooth);
        }
    }
    
    // Schiessen auf den Spieler
    IEnumerator FireAI(float distance)
    {
        // Instantiate and launch the shell.
        m_Fired = true;

        // mit 80% Wahrscheinlichkeit wird normal geschossen
        // mit 20% kommt ein Spezialangriff
        int shootIt = Random.Range(1, 100);
        if (shootIt < 80) StartCoroutine(OneShoot());
        else
        {
            // Zufaelliger Spezialangriff: Doppel-,Dreier- und Burstfeuer
            switch (Random.Range(1, 5))
            {
                case 1: StartCoroutine(SpecWeap1());
                    break;
                case 2: StartCoroutine(SpecWeap2());
                    break;
                case 3: StartCoroutine(SpecWeap3());
                    break;
                case 4: StartCoroutine(SpecWeap4());
                    break;
                case 5: StartCoroutine(SpecWeap5());
                    break;
                default: StartCoroutine(OneShoot());
                    break;
            }
        }
        

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();
        m_CurrentLaunchForce = m_MinLaunchForce;

        //yield return new WaitForSeconds(0.5f);
        //m_Fired = false;
        yield return null;
    }

    IEnumerator OneShoot()
    {
        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        shellInstance.velocity = launchForce * m_FireTransform.forward;

        yield return new WaitForSeconds(fireRateTime);
        m_Fired = false;
        yield return null;

    }

    IEnumerator SpecWeap1()
    {
            //m_FireTransform.rotation *= Quaternion.Euler(0, 45, 0);
            //Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
            //shellInstance.velocity = launchForce * m_FireTransform.forward;
            //m_FireTransform.rotation *= Quaternion.Euler(0, -45, 0);

            //Rigidbody shellInstance2 = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
            //shellInstance2.velocity = launchForce * m_FireTransform.forward;

            //m_FireTransform.rotation *= Quaternion.Euler(0, -45, 0);
            //Rigidbody shellInstance3 = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
            //shellInstance3.velocity = launchForce * m_FireTransform.forward;
            //m_FireTransform.rotation *= Quaternion.Euler(0, 45, 0);

        Rigidbody shellInstance;
        m_FireTransform.rotation *= Quaternion.Euler(0, 45, 0);
        for (int i = 0; i < 3; i++)
        {
            shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
            shellInstance.velocity = launchForce * m_FireTransform.forward;
            m_FireTransform.rotation *= Quaternion.Euler(0, -45, 0);
        }
        m_FireTransform.rotation *= Quaternion.Euler(0, 90, 0);

        

        yield return new WaitForSeconds(fireRateTime);
        m_Fired = false;
        yield return null;
    }

    IEnumerator SpecWeap2()
    {
        //StartCoroutine(SpecWeap1());
        //yield return new WaitForSeconds(0.2f);
        //StartCoroutine(SpecWeap1());
        //yield return new WaitForSeconds(0.2f);
        //StartCoroutine(SpecWeap1());

        Rigidbody shellInstance;
        for (int i = 0; i < 3; i++)
        {
            m_FireTransform.rotation *= Quaternion.Euler(0, 45, 0);
            for (int n = 0; n < 3; n++)
            {
                shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
                shellInstance.velocity = launchForce * m_FireTransform.forward;
                m_FireTransform.rotation *= Quaternion.Euler(0, -45, 0);
            }
            m_FireTransform.rotation *= Quaternion.Euler(0, 90, 0);
            yield return new WaitForSeconds(0.2f);
        }
        
        yield return new WaitForSeconds(fireRateTime - 0.2f);
        m_Fired = false;
        yield return null;
    }

    IEnumerator SpecWeap3()
    {
        int fireRate = 18;
        m_FireTransform.rotation *= Quaternion.Euler(0, 45, 0);

        for (int i = 0; i < fireRate; i++)
        {
            if (i < fireRate / 2) m_FireTransform.rotation *= Quaternion.Euler(0, -10, 0);
            else m_FireTransform.rotation *= Quaternion.Euler(0, 10, 0);
            Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
            shellInstance.velocity = launchForce * m_FireTransform.forward;
            yield return new WaitForSeconds(0.05f);
        }
        m_FireTransform.rotation *= Quaternion.Euler(0, -45, 0);

        
        yield return new WaitForSeconds(fireRateTime);
        m_Fired = false;
        yield return null;
    }

    IEnumerator SpecWeap4()
    {
        //float launchForce = 18f;
        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        shellInstance.transform.localScale *= 3;
        shellInstance.GetComponent<ShellExplosion>().m_MaxDamage *= 2;
        shellInstance.GetComponent<ShellExplosion>().m_ExplosionRadius *= 2;
        shellInstance.velocity = launchForce * m_FireTransform.forward;

        
        yield return new WaitForSeconds(fireRateTime);
        m_Fired = false;
        yield return null;
    }

    IEnumerator SpecWeap5()
    {
        //FireRate  16 = 1 Round
        //          32 = 2 Rounds ....
        specFire = true;
        int fireRate = 32;
        //float launchForce = 18f;

        for (int i = 0; i < fireRate; i++)
        {
            //if (i < fireRate / 2) m_FireTransform.rotation *= Quaternion.Euler(0, -10, 0);
            //Shoot in 360 Degree with 16 Bullets
            Quaternion newTankPos = transform.rotation * Quaternion.Euler(0, 22.5f, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, newTankPos, 1f);
            Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
            shellInstance.velocity = launchForce * m_FireTransform.forward;
            // 16 Shoots per Second = 0.0625 
            yield return new WaitForSeconds(0.0625f);
        }
        //m_FireTransform.rotation *= Quaternion.Euler(0, -45, 0);
        specFire = false;

        
        yield return new WaitForSeconds(fireRateTime);
        m_Fired = false;
        yield return null;
    }


}

//// Verschiedene Schuss kombinationen

//private void NormalShoot(float distance)
//{
//    Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
//    shellInstance.velocity = Random.Range(m_MinLaunchForce, distance) * m_FireTransform.forward;
//}

//private void ShootDree(float distance)
//{
//    Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
//    shellInstance.velocity = Random.Range(m_MinLaunchForce, distance) * m_FireTransform.forward;
//    shellInstance = Instantiate(m_Shell, m_FireTransform2.position, m_FireTransform.rotation) as Rigidbody;
//    shellInstance.velocity = Random.Range(m_MinLaunchForce, distance) * m_FireTransform2.forward;
//    shellInstance = Instantiate(m_Shell, m_FireTransform3.position, m_FireTransform.rotation) as Rigidbody;
//    shellInstance.velocity = Random.Range(m_MinLaunchForce, distance) * m_FireTransform3.forward;
//}

//private void DoubleShoot(float distance)
//{
//    //zweier Schuss
//    Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
//    shellInstance.velocity = Random.Range(m_MinLaunchForce, distance) * m_FireTransform.forward;
//    shellInstance = Instantiate(m_Shell, new Vector3(m_FireTransform.position.x + test, m_FireTransform.position.y,m_FireTransform.position.z + test), m_FireTransform.rotation) as Rigidbody;
//    shellInstance.velocity = Random.Range(m_MinLaunchForce, distance) * m_FireTransform.forward;
//}

//private void BurstFire(float distance)
//{
//    Rigidbody shellInstance;
//    int i = 0;
//    while (i < burstRate)
//    {
//        float rateTimer = 2 / burstRate;
//        rateTimer -= Time.deltaTime;

//        if (rateTimer < 0) 
//        {
//            rateTimer = 2 / burstRate;
//            i++;
//            shellInstance = Instantiate(m_Shell, new Vector3(m_FireTransform.position.x + test, m_FireTransform.position.y, m_FireTransform.position.z + test), m_FireTransform.rotation) as Rigidbody;
//            shellInstance.velocity = Random.Range(m_MinLaunchForce, distance) * m_FireTransform.forward;
//        }
//    }
//}
////Rundumschuss funktioniert nicht
//public void superShoot(float distance)
//{
//    Debug.Log("SUUUUPER");
//    // Instantiate and launch the shell.
//    m_Fired = true;

//    //AllAround(distance);

//    m_ShootingAudio.clip = m_FireClip;
//    m_ShootingAudio.Play();

//    m_CurrentLaunchForce = m_MinLaunchForce;
//}