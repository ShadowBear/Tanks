using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Menu : MonoBehaviour {

    // Use this for initialization
    //bool setMenu = false;
    Canvas menu;
    public bool playerDeath;
    public bool victory = false;
    public Text quitBtnText;

    public Text menuText;

    public GameObject continueBtn;
    public GameObject deathScreen;

    public Image stars;
    public int levelNumber;

    void Start () {
        menu = gameObject.GetComponentInChildren<Canvas>();
        menu.enabled = false;
        if(menuText != null) menuText.text = "Pause Menu";
        if (SceneManager.GetActiveScene().buildIndex < 2) menu.enabled = true;
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex > 1)
        {
            menu.enabled = menu.enabled ? false : true;
            
        }else if(Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex == 1)
        {
            SceneManager.LoadSceneAsync(0);
        }
        Time.timeScale = menu.enabled ? 0 : 1;

        if (playerDeath)
        {
            menu.enabled = true;
            if (victory)
            {
                menuText.text = "Victory";
                quitBtnText.text = "Menu";
            }
            else menuText.text = "Defeat";
            continueBtn.SetActive(false);
            deathScreen.SetActive(true);
            stars.fillAmount = 0.33f * GameController.control.temporaryStars[levelNumber];
        }
        else
        {
            //menu.enabled = true;
            if(menuText != null) menuText.text = "Pause Menu";
            if(quitBtnText != null) quitBtnText.text = "Quit";
            if (continueBtn != null) continueBtn.SetActive(true);
            if (deathScreen != null) deathScreen.SetActive(false);
        }
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
        GameController.control.Save();
        if (SceneManager.GetActiveScene().buildIndex == 0) Application.Quit();
        else SceneManager.LoadScene(0);
    }

    public void Respawn()
    {
        GameController.control.Save();
        int activeScene = SceneManager.GetActiveScene().buildIndex;
        menuText.text = "Pause Menu";
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
