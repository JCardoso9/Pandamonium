using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    private GameObject writer;

    private void Start()
    {
        //write to file
        writer = GameObject.Find("/WriteOnFile");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            writer.GetComponent<WriteOnFile>().WriteToFile("End");
        }
    }


}

