using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TankAndroidButtons : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{

    GameObject player;
    TankShooting shootingScript;
    private float m_CurrentLaunchForce;
    private float m_ChargeSpeed;

    public bool shallShoot = false;


    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        shootingScript = player.GetComponent<TankShooting>();
        m_ChargeSpeed = shootingScript.m_ChargeSpeed;
        m_CurrentLaunchForce = shootingScript.m_MinLaunchForce;
}
	
	// Update is called once per frame
	void Update () {
        if (shallShoot && shootingScript.m_Fired == false && shootingScript.canShoot)
        {
            shootingScript.LoadShoot();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(shootingScript.m_Fired == false) shootingScript.Fire();
        shallShoot = false;        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        shallShoot = true;
        shootingScript.m_Fired = false;
    }
}
