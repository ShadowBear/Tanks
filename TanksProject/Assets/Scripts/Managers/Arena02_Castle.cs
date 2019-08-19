using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Arena02_Castle : MonoBehaviour {

    public Text stageText;
    public int stageCounter = 1;
    public GameObject tankAI;

    private bool gamePaused = false;
    public Menu menu;

    public GameObject standingTowerStage2;
    public GameObject standingTowerStage3;

    public Transform[] spawnPoints;

    // Use this for initialization
    void Start()
    {
        GameController.control.levelNumber = 2;
        GameController.control.temporaryStars[2] = 0;
        if (GameController.control.starsEarned[2] < 0 || GameController.control.starsEarned[2] > 3) GameController.control.starsEarned[2] = 0;
        standingTowerStage2.SetActive(false);
        standingTowerStage3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemyState();
    }

    IEnumerator InstantiateEnemies(int stage)
    {
        gamePaused = true;
        yield return new WaitForSeconds(2f);
        int enemyCounter = 5 * stage;
        for (int i = 0; i < enemyCounter; i++)
        {
            int spawnPoint = Random.Range(0, spawnPoints.Length);
            GameObject ai = Instantiate(tankAI, spawnPoints[spawnPoint], true);
            ai.GetComponent<NavMeshAgent>().Warp(spawnPoints[spawnPoint].position);
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
            if (stageCounter < 4) StartCoroutine(ShowStage());
            nextStage(stageCounter);
        }
    }

    void nextStage(int stage)
    {
        switch (stage)
        {
            case 1:
                StartCoroutine(InstantiateEnemies(stage));
                stageCounter++;
                //print("Round 1");
                break;
            case 2:
                StartCoroutine(InstantiateEnemies(stage));
                standingTowerStage2.SetActive(true);
                if (GameController.control.starsEarned[2] < 1)
                    GameController.control.starsEarned[2] = 1;
                GameController.control.temporaryStars[2] = 1;
                stageCounter++;
                break;
            case 3:
                StartCoroutine(InstantiateEnemies(stage));
                standingTowerStage3.SetActive(true);
                //GameController.control.starsEarned[2] = 2;
                if (GameController.control.starsEarned[2] < 2)
                    GameController.control.starsEarned[2] = 2;
                GameController.control.temporaryStars[2] = 2;
                stageCounter++;
                //stageCounter = 1;
                break;
            case 4:
                if (GameController.control.starsEarned[2] < 3)
                    GameController.control.starsEarned[2] = 3;
                GameController.control.temporaryStars[2] = 3;
                GameController.control.Save();
                StartCoroutine(Victory());
                break;
            default:
                StartCoroutine(InstantiateEnemies(stage));
                break;
        }
    }
    IEnumerator Victory()
    {
        menu.playerDeath = true;
        menu.victory = true;
        yield return null;
    }
}
