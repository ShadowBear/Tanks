﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

    
    private string m_FireButton;         
    private float m_CurrentLaunchForce;  
    private float m_ChargeSpeed;         
    private bool m_Fired = true;                


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
        // Track the current state of the fire button and make decisions based on the current launch force.
        m_AimSlider.value = m_MinLaunchForce;

        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
        {
            // at max Charge not fired
            m_CurrentLaunchForce = m_MaxLaunchForce;
            Fire();
        }
        else if (Input.GetButtonDown(m_FireButton))
        {
            // have we pressed fire for first time
            m_Fired = false;
            m_CurrentLaunchForce = m_MinLaunchForce;
            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play();
        }
        else if (Input.GetButton(m_FireButton) && !m_Fired)
        {
            // holding fire button not yet firede
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
            m_AimSlider.value = m_CurrentLaunchForce;
        }
        else if (Input.GetButtonUp(m_FireButton) && !m_Fired)
        {
            // released the fire button 
            Fire();
        }
        else if (Input.GetKeyUp(KeyCode.Alpha1)) SpecialFireArms(1);
        else if (Input.GetKeyUp(KeyCode.Alpha2)) SpecialFireArms(2);
        else if (Input.GetKeyUp(KeyCode.Alpha3)) SpecialFireArms(3);
        else if (Input.GetKeyUp(KeyCode.Alpha4)) SpecialFireArms(4);
        else if (Input.GetKeyUp(KeyCode.Alpha5)) SpecialFireArms(5);
    }


    private void Fire()
    {
        // Instantiate and launch the shell.
        m_Fired = true;

        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        m_CurrentLaunchForce = m_MinLaunchForce;
    }


    public void FireAI(float distance)
    {
        // Instantiate and launch the shell.
        m_Fired = true;

        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        shellInstance.velocity = Random.Range(m_MinLaunchForce, distance) * m_FireTransform.forward;

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        m_CurrentLaunchForce = m_MinLaunchForce;
    }

    public bool getFired()
    {
        return m_Fired;
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
            m_FireTransform.rotation *= Quaternion.Euler(0, -45, 0);

            Rigidbody shellInstance2 = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
            shellInstance2.velocity = launchForce * m_FireTransform.forward;

            m_FireTransform.rotation *= Quaternion.Euler(0, -45, 0);
            Rigidbody shellInstance3 = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
            shellInstance3.velocity = launchForce * m_FireTransform.forward;
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
            // 16 Shoots per Second = 0.0625 
            yield return new WaitForSeconds(0.0625f);
        }
        //m_FireTransform.rotation *= Quaternion.Euler(0, -45, 0);
        yield return null;
    }

}