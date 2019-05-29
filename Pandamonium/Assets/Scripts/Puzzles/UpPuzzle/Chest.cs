using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Player player;
    GameObject chest,text;
    public GameObject[] obj;
    public static bool keyFound = false;
    public static bool chestO = false;
    int aux = 0;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        chest = GameObject.Find("ChestPuzzle");
        text = chest.transform.GetChild(0).gameObject;
        text.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(obj[0].transform.Find("red").gameObject.activeSelf && obj[1].transform.Find("green").gameObject.activeSelf && obj[2].transform.Find("blue").gameObject.activeSelf)
        {
            keyFound = true;   
        }
        if (chestO)
        {
            text.transform.GetComponent<TextMesh>().text = "Chest Opened\nKey Found";
            text.transform.GetComponent<TextMesh>().alignment = TextAlignment.Center;
            text.transform.localPosition = new Vector3(-3.5f, 6.5f, 0);

            if(aux == 0)
            {
                GameObject.Find("Red").SetActive(false);
                GameObject.Find("Green").SetActive(false);
                GameObject.Find("Blue").SetActive(false);
                aux++;
            }
            
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!chestO)
        {
            text.SetActive(true);
            text.transform.GetComponent<TextMesh>().text = "F to open chest";
            text.transform.GetComponent<TextMesh>().alignment = TextAlignment.Center;
            text.transform.localPosition = new Vector3(-3.5f, 5, 0);
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (Input.GetKeyUp(KeyCode.F))
        {
            if (!keyFound)
            {
                text.transform.GetComponent<TextMesh>().text = "Solve puzzle \nto open chest";
                text.transform.GetComponent<TextMesh>().alignment = TextAlignment.Center;
                text.transform.localPosition = new Vector3(-3.5f, 6.5f, 0);
            }
            else
            {
                anim.SetBool("isOpened", true);
                text.transform.GetComponent<TextMesh>().text = "Key Found";
                text.transform.GetComponent<TextMesh>().alignment = TextAlignment.Center;
                chestO = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!chestO)
            text.SetActive(false);
    }
}
