using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankShield : MonoBehaviour {

    public Renderer rend;
    public int maxPower = 1000;
    public float power;
    public Color matColor;
    private Color effectColor;
    public float resetTimer = 0;

    private TankShooting playerShoot;

    public bool death = false;
    public Light lightColor;

    public Image shieldImage;



	// Use this for initialization
	void Start () {
        power = maxPower;
        playerShoot = GameObject.FindGameObjectWithTag("Player").GetComponent<TankShooting>();
        //matColor = mat.GetColor("MainColor");
	}

    // Farbe ändert sich je nach Stärke des Schildes von 
    // Blau voll Geladen -> Grün -> Gelb -> Orange -> Rot Fast leer
	void Update () {
        ShieldColor();

        playerShoot.shielded = rend.enabled ? true : false;

        shieldImage.fillAmount = 1 * power/ maxPower;  

        if(resetTimer > 0) resetTimer -= Time.deltaTime;

        if (power < maxPower && power > 0 && resetTimer <= 0 && !death)
        {

            print("Regeneriere Mich");
            power += (300 * Time.deltaTime);
            if (power > maxPower) power = maxPower;
        }
        else if (resetTimer <= 0 && death)
        {
            power = maxPower;
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            gameObject.GetComponent<Light>().enabled = true;
            death = false;
        }
    }

    void ShieldColor()
    {
        if (power > maxPower * 0.8)
        {
            matColor = new Color(0, 0, 1, 0.2156f);
            effectColor = new Color(0, 0, 1, 1);
        }
        else if (power <= maxPower * 0.8 && power > maxPower * 0.6)
        {
            matColor = new Color(0, 1, 1, 0.2156f);
            effectColor = new Color(0, 1, 1, 1);
        }
        else if (power <= maxPower * 0.6 && power > maxPower * 0.4)
        {
            //55 255 0
            matColor = new Color(0.2156f, 1, 0, 0.2156f);
            Debug.Log("Sollte Grün zeigen");
            effectColor = new Color(0.2156f, 1, 0, 1);
        }
        else if (power <= maxPower * 0.4 && power > maxPower * 0.2)
        {
            //255 180 0
            Debug.Log("Sollte Orange zeigen");
            matColor = new Color(1, 0.70588f, 0, 0.2156f);
            effectColor = new Color(1, 0.70588f, 0, 1);
        }
        else if (power <= maxPower * 0.2 && power > 0)
        {
            matColor = new Color(1, 0, 0, 0.2156f);
            effectColor = new Color(1, 0, 0, 1);

        }
        else if (power <= 0)
        {
            //gameObject.GetComponent<MeshRenderer>().enabled = false;
            //gameObject.GetComponent<Light>().enabled = false;
            rend.enabled = false;
            lightColor.enabled = false;
        }

        rend.materials[1].color = matColor;
        rend.materials[0].color = effectColor;
        lightColor.color = effectColor;
    }

    public void TakeDMG(float damage)
    {
        if (power > 0)
        {
            power -= damage;
            resetTimer = 3f;
            if (power <= 0 && !death)
            {
                resetTimer = 8f;
                death = true;
            }
        }
    }

}
