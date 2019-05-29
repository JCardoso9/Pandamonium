using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdviceTrigger: MonoBehaviour
{
    public GameObject Container;
    public GameObject advice;
    public string str_advice;

    private bool triggered = false;
    private bool destroyed = false;
    private float time = 0.0f;
    private float timeToDestroy = 3.0f;

    void Update()
    {
        if(triggered == true && destroyed == false)
        {
            time += Time.deltaTime;
            if(time >= timeToDestroy)
            {
                Container.SetActive(false);
                destroyed = true;
            }
        }

         
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" && triggered == false)
        {
            advice.GetComponent<UnityEngine.UI.Text>().text = str_advice;
            Container.SetActive(true);
            triggered = true;
        }
    }
}

