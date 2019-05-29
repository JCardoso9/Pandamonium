using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLogAux : MonoBehaviour
{
    private GameObject logs;

    private void Start()
    {
        logs = GameObject.Find("/Aux");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            logs.GetComponent<TriggerLog>().Area = this.gameObject.name;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            logs.GetComponent<TriggerLog>().Area = this.gameObject.name;
        }
    }
}
