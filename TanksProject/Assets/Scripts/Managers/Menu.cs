using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour {

    // Use this for initialization
    //bool setMenu = false;
    Canvas menu;

    void Start () {
        menu = gameObject.GetComponentInChildren<Canvas>();
        menu.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.enabled = menu.enabled ? false : true;
        }
        if (menu.enabled) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    public void ContinueBtn()
    {
        menu.enabled = false;        
    }

    public void ExitBtn()
    {
        Application.Quit();
    }

    public void Respawn()
    {
        SceneManager.LoadSceneAsync(0);
    }

}
