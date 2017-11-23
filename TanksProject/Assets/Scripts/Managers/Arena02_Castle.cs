using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Arena02_Castle : MonoBehaviour {

    public Text stageText;
    int stageCounter = 1;
    public GameObject tankAI;

    public GameObject standingTowerStage2;
    public GameObject standingTowerStage3;

    public Transform[] spawnPoints;

    // Use this for initialization
    void Start()
    {

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
        int enemyCounter = 5 * stage;

        for (int i = 0; i < enemyCounter; i++)
        {
            int spawnPoint = Random.Range(0, spawnPoints.Length);
            //spawnPoints[].position = new Vector3(spawnPoints[i].position.x - 3, spawnPoints[i].position.y - 1, spawnPoints[i].position.z);
            GameObject ai = Instantiate(tankAI, spawnPoints[spawnPoint], true);
            //ai.transform.position = spawnPoints[spawnPoint].position;
            ai.GetComponent<NavMeshAgent>().Warp(spawnPoints[spawnPoint].position);
            //yield return new WaitForSeconds(1);
        }
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
        if (GameObject.FindGameObjectWithTag("Enemy") == null && stageCounter < 4 && stageCounter > 0)
        {
            StartCoroutine(ShowStage());
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
                print("Round 1");
                break;
            case 2:
                StartCoroutine(InstantiateEnemies(stage));
                standingTowerStage2.SetActive(true);
                stageCounter++;
                break;
            case 3:
                StartCoroutine(InstantiateEnemies(stage));
                standingTowerStage3.SetActive(true);
                stageCounter = 1;
                break;
            default:
                StartCoroutine(InstantiateEnemies(stage));
                break;
        }
    }
}
