using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldParticel : MonoBehaviour {

    public float scrollSpeed = 0.5f;


    public bool U = true;
    public bool V = false;

    public bool swap = false;

    public float offset;

    public Material mat;

    
    // Use this for initialization
	void Start () {
        mat = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
        //offset = ((Time.time * scrollSpeed) % 0.7f) - 0.1f;
        //if (offset < 0 || offset > 0.6f) swap = !swap;
        if (offset > 0.6f) swap = false;
        else if (offset < -0.1f) swap = true;

        if (swap)
        {
            offset += Time.deltaTime * scrollSpeed;
        }
        else
        {
            offset -= Time.deltaTime * scrollSpeed;
        }
        //offset = Time.time * scrollSpeed -0.1f;


        if (U&V)
        {
            mat.mainTextureOffset = new Vector2(offset, offset);
        }
        else if (U)
        {
            mat.mainTextureOffset = new Vector2(0, offset);
        }
        else if (V)
        {
            mat.mainTextureOffset = new Vector2(offset, 0);
        }
		
	}
}
