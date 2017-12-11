using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsManager : MonoBehaviour {

    // Use this for initialization
    Image stars;
    public int levelNumber;

    void Start () {
        stars = gameObject.GetComponent<Image>();
        stars.fillAmount = 0.33f * GameController.control.starsEarned[levelNumber];
	}
	
	// Update is called once per frame
	void Update () {
        //stars = gameObject.GetComponent<Image>();
        //stars.fillAmount = 0.33f * GameController.control.starsEarned[levelNumber];
    }

    public void UpdateStars(int level)
    {
        stars = gameObject.GetComponent<Image>();
        stars.fillAmount = 0.33f * GameController.control.starsEarned[level];
    }

}
