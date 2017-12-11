using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;       
    public Rigidbody m_Shell;            
    public Transform m_FireTransform;
    public Transform tankTransform;
    public Slider m_AimSlider;           
    public AudioSource m_ShootingAudio;  
    public AudioClip m_ChargingClip;     
    public AudioClip m_FireClip;         
    public float m_MinLaunchForce = 10f; 
    public float m_MaxLaunchForce = 30f; 
    public float m_MaxChargeTime = 0.75f;
    public bool shielded = false;
    public Text ammuText;

    public int ammu = 5;
    public int maxAmmu = 5;
    public bool canShoot = true;
    private bool isReloading = false;
    private float reloadTime = 2f;

    //private bool touchShoot = false;
    public TankVirtualJoystick joystick;

    
    private string m_FireButton;         
    public float m_CurrentLaunchForce;  
    public float m_ChargeSpeed;         
    public bool m_Fired = true;      
              


    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start()
    {
        m_FireButton = "Fire" + m_PlayerNumber;

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }
    

    private void Update()
    {

        //Mouse Input as Touch Tested
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    Vector3 mousePosition = Input.mousePosition;
        //    bool point = RectTransformUtility.RectangleContainsScreenPoint(joystick.GetComponent<Image>().rectTransform, mousePosition, Camera.current);
        //    if (point) Debug.Log("Punkt liegt im Kasten");
        //}

        if (ammu == 0) ammuText.text = "R";
        else ammuText.text = ammu.ToString();

        if (ammu <= 0 && !isReloading)
        {
            isReloading = true;
            StartCoroutine(ReloadAmmu());
        }

#if Unity_STANDALONE || UNITY_WEBPLAYER
        // Track the current state of the fire button and make decisions based on the current launch force.
        

        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
        {
            // at max Charge not fired
            m_CurrentLaunchForce = m_MaxLaunchForce;
            Fire();
        }
        else if (Input.GetButtonDown(m_FireButton) && !shielded && canShoot)
        {
            // have we pressed fire for first time
            m_Fired = false;
            m_CurrentLaunchForce = m_MinLaunchForce;
            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play();
        }
        else if (Input.GetButton(m_FireButton) && !m_Fired && !shielded && canShoot)
        {
            // holding fire button not yet firede
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
            m_AimSlider.value = m_CurrentLaunchForce;
        }
        else if (Input.GetButtonUp(m_FireButton) && !m_Fired && !shielded && canShoot)
        {
            // released the fire button 
            Fire();
        }
        else if (Input.GetKeyUp(KeyCode.Alpha1) && !shielded) SpecialFireArms(1);
        else if (Input.GetKeyUp(KeyCode.Alpha2) && !shielded) SpecialFireArms(2);
        else if (Input.GetKeyUp(KeyCode.Alpha3) && !shielded) SpecialFireArms(3);
        else if (Input.GetKeyUp(KeyCode.Alpha4) && !shielded) SpecialFireArms(4);
        else if (Input.GetKeyUp(KeyCode.Alpha5) && !shielded) SpecialFireArms(5);
#else
        //if (Input.touchCount > 0)
        //{
        //    Touch[] touches = Input.touches;
        //    //Touch fireTouch = touches[0];
        //    for (int i = 0; i < touches.Length; i++)
        //    {
        //        bool point = RectTransformUtility.RectangleContainsScreenPoint(joystick.GetComponent<Image>().rectTransform, touches[i].position, Camera.current);
        //        if (!point && touches[i].phase == TouchPhase.Stationary)
        //        {

        //            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
        //            m_AimSlider.value = m_CurrentLaunchForce;
        //            //touchShoot = true;

        //            //Debug.Log("Punkt liegt im Kasten");
        //        }
        //        if (!point && touches[i].phase == TouchPhase.Ended)
        //        {
        //            //touchShoot = false;
        //            Fire();
        //        }
        //    }
        //    //if(fireTouch.deltaTime > 0 && !RectTransformUtility.RectangleContainsScreenPoint(joystick.GetComponent<Image>().rectTransform, fireTouch.position, Camera.current))
        //    //{
        //    //    m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
        //    //    touchShoot = true;
        //    //    //m_AimSlider.value = m_CurrentLaunchForce;
        //    //}
        //    //else if(touchShoot)
        //    //{
        //    //    Fire();
        //    //    touchShoot = false;
        //    //}
        //}
            
#endif
    }


    public void LoadShoot()
    {
        m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
        {
            // at max Charge not fired
            m_CurrentLaunchForce = m_MaxLaunchForce;
            Fire();
        }
        m_AimSlider.value = m_CurrentLaunchForce;

    }

    public void Fire()
    {
        // Instantiate and launch the shell.
       

        if (canShoot && ammu > 0)
        {
            m_Fired = true;
            Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
            shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;
            shellInstance.GetComponent<ShellExplosion>().playerShell = true;
            ammu--;

            m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play();

            m_CurrentLaunchForce = m_MinLaunchForce;
            m_AimSlider.value = m_MinLaunchForce;
        }
        
    }


    public void FireAI(float distance)
    {
        // Instantiate and launch the shell.
        m_Fired = true;

        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        shellInstance.velocity = UnityEngine.Random.Range(m_MinLaunchForce, distance) * m_FireTransform.forward;
        shellInstance.GetComponent<ShellExplosion>().playerShell = true;

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        m_CurrentLaunchForce = m_MinLaunchForce;
    }

    public bool getFired()
    {
        return m_Fired;
    }

    IEnumerator ReloadAmmu()
    {
        canShoot = false;
        yield return new WaitForSeconds(reloadTime);
        ammu = maxAmmu;
        canShoot = true;
        isReloading = false;
        yield return null;
    }


    private void SpecialFireArms(int weapon)
    {
        m_Fired = true;
        float launchForce = 18f;

        if (weapon == 1)
        {
            m_FireTransform.rotation *= Quaternion.Euler(0, 45, 0);
            Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
            shellInstance.velocity = launchForce * m_FireTransform.forward;
            shellInstance.GetComponent<ShellExplosion>().playerShell = true;
            m_FireTransform.rotation *= Quaternion.Euler(0, -45, 0);

            Rigidbody shellInstance2 = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
            shellInstance2.velocity = launchForce * m_FireTransform.forward;
            shellInstance2.GetComponent<ShellExplosion>().playerShell = true;

            m_FireTransform.rotation *= Quaternion.Euler(0, -45, 0);
            Rigidbody shellInstance3 = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
            shellInstance3.velocity = launchForce * m_FireTransform.forward;
            shellInstance3.GetComponent<ShellExplosion>().playerShell = true;
            m_FireTransform.rotation *= Quaternion.Euler(0, 45, 0);
        }

        if (weapon == 2) StartCoroutine(SpecWeap2());
        if (weapon == 3) StartCoroutine(SpecWeap3());
        if (weapon == 4) StartCoroutine(SpecWeap4());
        if (weapon == 5) StartCoroutine(SpecWeap5());

    }



    IEnumerator SpecWeap2()
    {
        SpecialFireArms(1);
        yield return new WaitForSeconds(0.2f);
        SpecialFireArms(1);
        yield return new WaitForSeconds(0.2f);
        SpecialFireArms(1);
    }

    IEnumerator SpecWeap3()
    {
        int fireRate = 18;
        float launchForce = 18f;
        m_FireTransform.rotation *= Quaternion.Euler(0, 45, 0);

        for (int i = 0; i < fireRate; i++)
        {
            if (i < fireRate / 2) m_FireTransform.rotation *= Quaternion.Euler(0, -10, 0);
            else m_FireTransform.rotation *= Quaternion.Euler(0, 10, 0);
            Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
            shellInstance.velocity = launchForce * m_FireTransform.forward;
            shellInstance.GetComponent<ShellExplosion>().playerShell = true;
            yield return new WaitForSeconds(0.05f);
        }
        m_FireTransform.rotation *= Quaternion.Euler(0, -45, 0);
        yield return null;
    }

    IEnumerator SpecWeap4()
    {
        float launchForce = 18f;
        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        shellInstance.transform.localScale *= 3;
        shellInstance.GetComponent<ShellExplosion>().m_MaxDamage *= 2;
        shellInstance.GetComponent<ShellExplosion>().m_ExplosionRadius *= 2;
        shellInstance.velocity = launchForce * m_FireTransform.forward;
        shellInstance.GetComponent<ShellExplosion>().playerShell = true;

        yield return null;
    }

    IEnumerator SpecWeap5()
    {
        //FireRate  16 = 1 Round
        //          32 = 2 Rounds ....
        int fireRate = 32;
        float launchForce = 18f;

        for (int i = 0; i < fireRate; i++)
        {
            //if (i < fireRate / 2) m_FireTransform.rotation *= Quaternion.Euler(0, -10, 0);
            //Shoot in 360 Degree with 16 Bullets
            Quaternion newTankPos = tankTransform.rotation * Quaternion.Euler(0, 22.5f, 0);
            tankTransform.rotation = Quaternion.Slerp(tankTransform.rotation, newTankPos,1f);
            Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
            shellInstance.velocity = launchForce * m_FireTransform.forward;
            shellInstance.GetComponent<ShellExplosion>().playerShell = true;
            // 16 Shoots per Second = 0.0625 
            yield return new WaitForSeconds(0.0625f);
        }
        //m_FireTransform.rotation *= Quaternion.Euler(0, -45, 0);
        yield return null;
    }
}