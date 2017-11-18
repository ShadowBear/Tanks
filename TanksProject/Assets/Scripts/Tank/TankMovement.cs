using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public int m_PlayerNumber = 1;
    public float m_Speed = 12f;
    public float m_TurnSpeed = 180f;
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling;
    public AudioClip m_EngineDriving;
    public float m_PitchRange = 0.2f;

    public GameObject shield;
    public bool shieldedAndroid = false;
    public GameObject fireRing;

    public TankVirtualJoystick joystick;


    private string m_MovementAxisName;
    private string m_TurnAxisName;
    private Rigidbody m_Rigidbody;
    private float m_MovementInputValue;
    private float m_TurnInputValue;
    private float m_OriginalPitch;

    Vector3 velocity = Vector3.zero;


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    private void OnEnable()
    {
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable()
    {
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        m_OriginalPitch = m_MovementAudio.pitch;
    }


    private void Update()
    {

#if Unity_STANDALONE || UNITY_WEBPLAYER
        // Store the player's input and make sure the audio for the engine is playing.
        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);
#else
        m_MovementInputValue = joystick.Vertical();
        m_TurnInputValue = joystick.Horizontal();
#endif
        //EngineAudio();
    }


    private void EngineAudio()
    {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.

        if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f)
        {
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else
        {
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }

    }


    private void FixedUpdate()
    {
        // Move and turn the tank.
        Move();
        Turn();
        Shield();
        FireRing();
        ReloadAmmu();
    }


    private void Move()
    {
        //Adjust the position of the tank based on the player's input.
        //Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
        //m_Rigidbody.MovePosition(m_Rigidbody.position + movement);

        
#if Unity_STANDALONE || UNITY_WEBPLAYER
        velocity = new Vector3 (transform.forward.x * m_MovementInputValue * m_Speed,m_Rigidbody.velocity.y,transform.forward.z * m_MovementInputValue * m_Speed);
        m_Rigidbody.velocity = velocity;
#else
        velocity.Set(m_MovementInputValue, 0, m_TurnInputValue);
        velocity = velocity.normalized * m_Speed * Time.deltaTime;
        m_Rigidbody.MovePosition(transform.position + velocity);
#endif
    }


    private void Turn()
    {
        // Adjust the rotation of the tank based on the player's input.
#if Unity_STANDALONE || UNITY_WEBPLAYER
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
#else
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_Rigidbody.transform.position), m_TurnSpeed);
        if(velocity.magnitude > 0.1f) m_Rigidbody.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(velocity), m_TurnSpeed));
#endif


    }

    private void Shield()
    {
#if Unity_STANDALONE || UNITY_WEBPLAYER
        if (Input.GetKey(KeyCode.Q))
        {
            //shield.SetActive(true);
            shield.GetComponent<MeshRenderer>().enabled = true;
            shield.GetComponent<Light>().enabled = true;
            //gameObject.GetComponent<TankHealth>().indestructable = true;
        }
        else //if (Input.GetKeyUp(KeyCode.Q))
        {
            //shield.SetActive(false);
            //gameObject.GetComponent<TankHealth>().indestructable = false;
            shield.GetComponent<MeshRenderer>().enabled = false;
            shield.GetComponent<Light>().enabled = false;
        }
#else
        if (shieldedAndroid)
        {
            shield.GetComponent<MeshRenderer>().enabled = true;
            shield.GetComponent<Light>().enabled = true;
        }
        else
        {
            shield.GetComponent<MeshRenderer>().enabled = false;
            shield.GetComponent<Light>().enabled = false;
        }
#endif
    }

    private void FireRing()
    {
        if (Input.GetKey(KeyCode.E))
        {
            fireRing.SetActive(true);
            fireRing.GetComponentInChildren<ParticleSystem>().Play();
            
        }
        else //if (Input.GetKeyUp(KeyCode.E))
        {
            fireRing.SetActive(false);
        }
    }

    private void ReloadAmmu()
    {
        if (Input.GetKeyDown(KeyCode.R)){
            TankShooting tankShoot = gameObject.GetComponent<TankShooting>();
            if (tankShoot.ammu != 5) tankShoot.ammu = 0;
        }
    }

    //void OnCollisionEnter(Collision col)
    //{
    //    m_Rigidbody.angularVelocity = new Vector3(0,0,0);
    //}

    //void OnCollisionStay(Collision col)
    //{
    //    m_Rigidbody.angularVelocity = new Vector3(0, 0, 0);
    //}
}
    