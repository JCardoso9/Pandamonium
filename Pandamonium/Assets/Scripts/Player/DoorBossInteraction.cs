using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBossInteraction : MonoBehaviour
{
    public GameObject player;

    public bool isNear;

    //advice
    public GameObject Container;
    public GameObject advice;
    public string str_advice;
    private bool triggered = false;
    private bool destroyed = false;
    private float time = 0.0f;
    private float timeToDestroy = 3.0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (Vector3.Distance(GameObject.Find("BossDoor").transform.position, player.transform.position) < 10)
        {
            if (Chest.keyFound && Input.GetButtonDown("Interact"))
            {
                GameObject.Find("BossDoor").SetActive(false);
                triggered = true; // a cheet to dont be called aguen if player open door
                Container.SetActive(false);
            }
            else
            {
                advice.GetComponent<UnityEngine.UI.Text>().text = str_advice;
                Container.SetActive(true);
                triggered = true;
            }
        }
        else
        {
            Container.SetActive(false);
        }

        if (triggered == true && destroyed == false)
        {
            time += Time.deltaTime;
            if (time >= timeToDestroy)
            {
                Container.SetActive(false);
                destroyed = true;
            }
        }
    }
}
