using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PutEmblemOnStone : MonoBehaviour
{
    public GameObject[] embGrabed;
    public GameObject[] obj;
    public GameObject text;
    int count = 0;
    GameObject lastobj;
    public static bool hasArtifact = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!Chest.chestO)
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                //Vector3 pos = new Vector3(28.618f, -38.793f, 0);
                if (!hasArtifact)
                    text.transform.GetComponent<TextMesh>().text = "Find the artifacts";

                if (lastobj != null)
                {
                    lastobj.SetActive(false);
                }

                if (count == 0 && embGrabed[0].activeSelf)
                {
                    obj[0].SetActive(true);
                    lastobj = obj[0];
                }
                if (count == 0 && embGrabed[0].activeSelf == false)
                {
                    count++;
                }

                if (count == 1 && embGrabed[1].activeSelf)
                {
                    obj[1].SetActive(true);
                    lastobj = obj[1];
                }
                if (count == 1 && embGrabed[1].activeSelf == false)
                {
                    count++;
                }

                if (count == 2 && embGrabed[2].activeSelf)
                {
                    obj[2].SetActive(true);
                    lastobj = obj[2];
                }
                if (count == 2 && embGrabed[2].activeSelf == false)
                {
                    count++;
                }
                count++;
                if (count >= 3)
                    count = 0;
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Chest.chestO)
        {
            text.SetActive(true);
            if (hasArtifact)
            {
                text.transform.GetComponent<TextMesh>().text = "F to place artifact";
            }
        }
       



    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        text.SetActive(false);
    }
}
