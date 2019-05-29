using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestInteraction : MonoBehaviour
{

    protected bool opened = false;

    public float lifeBoost;
    public int experience;

    public Player player;
    protected LifeManager lm;

    protected Text canvasChestText;
    public GameObject canvasChest;
    public float timeToDestroyChest = 3;

    public bool playerNear = false;
    public Animator anim;

    // Start is called before the first frame update
    protected void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        lm = GameObject.FindGameObjectWithTag("Player").GetComponent<LifeManager>();
        //canvasChest = GameObject.Find("ChestText");
        canvasChestText = canvasChest.GetComponent<Text>();
        canvasChest.SetActive(false);

        
    }

    // Update is called once per frame
    void Update()
    {
        if (!opened) {
            if (playerNear) {
                if (Input.GetButtonDown("Interact")) {
                    anim.SetBool("isOpened", true);
                    lm.restoreLife(lifeBoost);
                    //Debug.Log(player.experience);
                    player.GainExperience(experience);
                    //Debug.Log(player.experience);
                    opened = true;
                    canvasChestText.text = " You gained " + experience + " experience and " + lifeBoost + " life.";
                    canvasChest.SetActive(true);

                   
                }
            }
        }
        // chest has been opened and will destroy itself
        else {
            if (timeToDestroyChest <= 0) {
                //Destroy(gameObject);
                canvasChest.SetActive(false);
            }

            else timeToDestroyChest -= Time.deltaTime;
        }
    }

    protected void OnCollisionEnter2D(Collision2D other) {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "Player") {
            Debug.Log("Enter");
            playerNear = true;
        }
    }

    protected void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("Left");
            playerNear = false;
        }
    }
}
