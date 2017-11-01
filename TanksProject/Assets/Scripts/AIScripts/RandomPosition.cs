using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomPosition : MonoBehaviour {

    NavMeshAgent nav;
    float timec = 0;
    public int range = 5;

    public int radius = 5;
    public float minRange = 2;
    public float distanceToPlayer = 10;
    public float accuracy = 0.5f;
    public float smooth = 2.0f;

    private float fieldOfViewAngle = 110f;


    private GameObject player;
    private bool isDodging = false;
    Vector3 dodgeit;
    private float resetTime = 5;

    // Use this for initialization
    void Start () {
        nav = gameObject.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }




    //// Update is called once per frame
    //void Update () {


    //    //Abfragen wie weit der Spieler entfernt ist und ob er auf die KI schiesst
    //    if (distanceToPlayer > Mathf.Abs((player.transform.position - transform.position).magnitude))
    //    {
    //        if (!player.GetComponent<TankShooting>().getFired())
    //        {
    //            Vector3 direction = player.transform.position - transform.position;
    //            float angle = Vector3.Angle(direction, transform.forward);

    //            //schiesst er in die Richtung der KI -> Ausweichen (Funktionierte nicht immer)
    //            if (angle < fieldOfViewAngle * 0.5f)
    //            {

    //                if (!isDodging) RandomDodge();

    //                //Raycast weggelassen/hat nicht richtig funktioniert =(

    //                //RaycastHit hit;
    //                //if (Physics.Raycast(transform.position + (transform.up / 2), direction.normalized, out hit, 25f))
    //                //{
    //                //    if (hit.collider.gameObject == player)
    //                //    {



    //                //    }
    //                //}
    //            }
    //            //if (!isDodging) RandomDodge();
    //        }
    //    }
    //    //Sobald die KI fertig mit ausweichen -> Attack
    //    if((dodgeit-transform.position).magnitude < accuracy && isDodging)
    //    {
    //        gameObject.GetComponent<AIShooting>().FireAI((dodgeit - transform.position).magnitude);
    //        isDodging = false;
    //        gameObject.GetComponent<AINavMesh>().shallDodge = false;
    //    }

    //    //Reset des Ausweichens falls irgendwas schief geht
    //    if (isDodging)
    //    {
    //        resetTime -= Time.deltaTime;
    //        if (resetTime <= 0)
    //        {
    //            resetTime = 5;
    //            isDodging = false;
    //            gameObject.GetComponent<AINavMesh>().shallDodge = false;
    //        }
    //    }

    //}

    // Update is called once per frame
    void Update()
    {


        //Abfragen wie weit der Spieler entfernt ist und ob er auf die KI schiesst
        if (Mathf.Abs((player.transform.position - transform.position).magnitude) <= distanceToPlayer)
        {
            if (!player.GetComponent<TankShooting>().getFired())
            {
                Vector3 direction = transform.position - player.transform.position;
                float angle = Vector3.Angle(direction, player.transform.forward);

                //schiesst er in die Richtung der KI -> Ausweichen (Funktionierte nicht immer)
                if (angle < fieldOfViewAngle * 0.5f)
                {
                    print("Spieler schießt in Richtung Gegner! Sollte besser Ausweichen x.x");
                    if (!isDodging) RandomDodge();

                    //Raycast weggelassen/hat nicht richtig funktioniert =(

                    //RaycastHit hit;
                    //if (Physics.Raycast(transform.position + (transform.up / 2), direction.normalized, out hit, 25f))
                    //{
                    //    if (hit.collider.gameObject == player)
                    //    {



                    //    }
                    //}
                }
                //if (!isDodging) RandomDodge();
            }
        }
        //Sobald die KI fertig mit ausweichen -> Attack
        if ((dodgeit - transform.position).magnitude < accuracy && isDodging)
        {
            StartCoroutine(turnToPlayer());
        }

        //Reset des Ausweichens falls irgendwas schief geht
        if (isDodging)
        {
            resetTime -= Time.deltaTime;
            if (resetTime <= 0)
            {
                resetTime = 5;
                isDodging = false;
                gameObject.GetComponent<AINavMesh>().shallDodge = false;
                gameObject.GetComponent<AIShooting>().attack = true;

                print("Fehler beim Dodgen");
                ResetDodgeSpeed();
            }
        }

    }

    IEnumerator turnToPlayer()
    {
        
        Vector3 direction = player.transform.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);

        while (Mathf.Abs(angle) >= 10)
        {
            //transform.rotation *= Quaternion.Euler(0, 1, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((player.transform.position - transform.position)), Time.deltaTime * smooth);
            yield return new WaitForSeconds(0.15f);
            direction = player.transform.position - transform.position;
            angle = Vector3.Angle(direction, transform.forward);
            nav.isStopped = true;
        }        

        //gameObject.GetComponent<AIShooting>().FireAI((transform.position - player.transform.position).magnitude);

        isDodging = false;
        gameObject.GetComponent<AINavMesh>().shallDodge = false;
        gameObject.GetComponent<AIShooting>().attack = true;
        nav.isStopped = false;
        ResetDodgeSpeed();
        yield return null;
    }

    // Zufaellige position in der naehe bestimmen -> zu dieser wird ausgewichen
    void RandomDodge()
    {
        Debug.Log("Ich Dodge Jetzt");
        isDodging = true;
        gameObject.GetComponent<AINavMesh>().shallDodge = true;
        float getawayX = Random.Range(-range, range);
        if (getawayX > 0) getawayX += minRange;
        else getawayX -= minRange;
        float getawayY = Random.Range(-range, range);
        if (getawayY > 0) getawayY += minRange;
        else getawayY -= minRange;
        dodgeit = transform.position + new Vector3(UnityEngine.Random.Range(-range, range), 0, UnityEngine.Random.Range(-range, range));
        nav.isStopped = false;
        gameObject.GetComponent<AIShooting>().attack = false;
        nav.SetDestination(dodgeit);
        SetDodgeSpeed();
    }

    void SetDodgeSpeed()
    {
        nav.speed = 5;
        nav.angularSpeed = 240;
        nav.acceleration = 16;
    }

    void ResetDodgeSpeed()
    {
        nav.speed = 3.5f;
        nav.angularSpeed = 120;
        nav.acceleration = 8;
    }


}
