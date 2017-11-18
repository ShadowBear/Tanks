using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    private GameObject player;
    public float m_DampTime = 0.2f;
    private Vector3 m_MoveVelocity;

    public float speedH = 0.002f;
    public float speedV = 0.002f;

    private float yaw = 0f;
    private float pitch = 0f;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = Vector3.SmoothDamp(transform.position, player.transform.position, ref m_MoveVelocity , m_DampTime);
#if Unity_STANDALONE || UNITY_WEBPLAYER
        if (Input.GetKey(KeyCode.Mouse1))
        {
            print("Mouse is in da House");
            yaw += speedH * Input.GetAxis("Mouse X");
            //pitch += speedV * Input.GetAxis("Mouse Y");

            //transform.rotation.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, yaw, transform.rotation.eulerAngles.z);

        }
#endif
    }
}
