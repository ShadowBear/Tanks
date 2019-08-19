using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Arena01 : MonoBehaviour {


    public Text stageText;
    public int stageCounter = 1;
    public GameObject tankAI;

    public GameObject victoryScreen;

    private bool gamePaused = false;
    public Menu menu;

    public GameObject standingTowerStage2;
    public GameObject standingTowerStage3;

    public Transform [] spawnPoints;

	// Use this for initialization
	void Start () {
        GameController.control.levelNumber = 1;
        if(GameController.control.starsEarned[1] < 0 || GameController.control.starsEarned[1] > 3) GameController.control.starsEarned[1] = 0;
        GameController.control.temporaryStars[1] = 0;
        print("Tower aus");
        standingTowerStage2.SetActive(false);
        standingTowerStage3.SetActive(false);
}
	
	// Update is called once per frame
	void Update () {
        CheckEnemyState();
	}

    IEnumerator InstantiateEnemies()
    {
        gamePaused = true;
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < spawnPoints.Length; i+= 1)
        {
            spawnPoints[i].position = new Vector3(spawnPoints[i].position.x -3, spawnPoints[i].position.y-1, spawnPoints[i].position.z);
            GameObject ai = Instantiate(tankAI, spawnPoints[i],true);
            //ai.transform.position = spawnPoints[i].position;
            ai.GetComponent<NavMeshAgent>().Warp(spawnPoints[i].position);
        }
        gamePaused = false;
        yield return null;
    }

    IEnumerator ShowStage()
    {
        stageText.text = "Stage " + stageCounter.ToString();
        stageText.enabled = true;
        yield return new WaitForSeconds(5);
        stageText.enabled = false;
        yield return null;
    }

    void CheckEnemyState()
    {
        if (GameObject.FindGameObjectWithTag("Enemy") == null && stageCounter < 5 && stageCounter > 0 && !gamePaused)
        {
            if(stageCounter < 4) StartCoroutine(ShowStage());
            nextStage(stageCounter);
        }
    }

    void nextStage(int stage)
    {
        switch (stage)
        {
            case 1:
                StartCoroutine(InstantiateEnemies());
                stageCounter++;
                //print("Round 1");
                break;
            case 2:
                StartCoroutine(InstantiateEnemies());
                if (GameController.control.starsEarned[1] < 1)
                GameController.control.starsEarned[1] = 1;
                GameController.control.temporaryStars[1] = 1;
                standingTowerStage2.SetActive(true);
                stageCounter++;
                break;
            case 3:
                StartCoroutine(InstantiateEnemies());
                if (GameController.control.starsEarned[1] < 2)
                    GameController.control.starsEarned[1] = 2;
                GameController.control.temporaryStars[1] = 2;
                standingTowerStage3.SetActive(true);
                stageCounter++;
                break;
            case 4:
                if (GameController.control.starsEarned[1] < 3)
                    GameController.control.starsEarned[1] = 3;
                GameController.control.temporaryStars[1] = 3;
                GameController.control.Save();
                StartCoroutine(Victory());
                //SceneManager.LoadScene(1);
                break;
            default:
                StartCoroutine(InstantiateEnemies());
                break;
        }
    }

    IEnumerator Victory()
    {
        menu.playerDeath = true;
        menu.victory = true;
        yield return null;
        //yield return new WaitForSeconds(3);
        //SceneManager.LoadScene(1);
    }

}
