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
        if (SceneManager.GetActiveScene().buildIndex < 2) menu.enabled = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex > 1)
        {
            menu.enabled = menu.enabled ? false : true;
            
        }
        Time.timeScale = menu.enabled ? 0 : 1;
    }

    public void ChooseLevel()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void ContinueBtn()
    {
        menu.enabled = false;
        //Time.timeScale = 1;
    }

    public void ExitBtn()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) Application.Quit();
        else SceneManager.LoadScene(0);
    }

    public void Respawn()
    {
        int activeScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(activeScene);
    }

    public void LoadLevel01()
    {
        SceneManager.LoadSceneAsync(2);
    }
    public void LoadLevel02()
    {
        SceneManager.LoadSceneAsync(3);
    }

}
