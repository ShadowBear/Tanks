using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TankHealth : MonoBehaviour
{
    public float m_StartingHealth = 100f;          
    public Slider m_Slider;                        
    public Image m_FillImage;                      
    public Color m_FullHealthColor = Color.green;  
    public Color m_ZeroHealthColor = Color.red;    
    public GameObject m_ExplosionPrefab;
    public GameObject shieldObject = null;
    public TankShield tankShield;

    public bool indestructable = false;
    public bool respawned = false;
    public Transform lastRespawenPoint;


    private AudioSource m_ExplosionAudio;          
    private ParticleSystem m_ExplosionParticles;   
    public float m_CurrentHealth;  
    private bool m_Dead;

    private bool corStarted = false;     


    private void Awake()
    {
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();

        m_ExplosionParticles.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;

        SetHealthUI();
    }
    

    void Update()
    {
        if (this.gameObject != GameObject.FindGameObjectWithTag("Player"))
        {
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<TankHealth>().respawned && !corStarted)
            {
                m_CurrentHealth = m_StartingHealth;
                SetHealthUI();
                corStarted = true;
                gameObject.GetComponent<AIShooting>().attack = false;
                StartCoroutine(ResetRespawn());
            }
        }
    }


    public void TakeDamage(float amount)
    {
        //print("TakeDmg aufgerufen");
        // Adjust the tank's current health, update the UI based on the new health and check whether or not the tank is dead.
        if (shieldObject != null)
        {
            if (shieldObject.GetComponent<MeshRenderer>().enabled && tankShield.power > 0) tankShield.TakeDMG(amount);
            else
            {
                m_CurrentHealth -= amount;
                SetHealthUI();
                if (m_CurrentHealth <= 0f && !m_Dead)
                {
                    OnDeath();
                }
            }            
        }
        else
        {
            m_CurrentHealth -= amount;
            SetHealthUI();
            if (m_CurrentHealth <= 0f && !m_Dead)
            {
                OnDeath();
            }
            //print("Habe schaden bekomen");
        }
    }


    private void SetHealthUI()
    {
        // Adjust the value and colour of the slider.
        m_Slider.value = m_CurrentHealth;

        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
    }


    private void OnDeath()
    {
        // Play the effects for the death of the tank and deactivate it.
        m_Dead = true;

        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);

        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();

        if(this.gameObject == GameObject.FindGameObjectWithTag("Player"))
        {
            RespawnAtCheckPoint();
        }else gameObject.SetActive(false);

    }

    private void RespawnAtCheckPoint()
    {
        transform.position = lastRespawenPoint.position;
        respawned = true;
        OnEnable();
    }

    IEnumerator ResetRespawn()
    {
        yield return new WaitForSeconds(2f);
        GameObject.FindGameObjectWithTag("Player").GetComponent<TankHealth>().respawned = false;
        corStarted = false;
        yield return null;

    }

}