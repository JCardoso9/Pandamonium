using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossChestInteraction : ChestInteraction
{

    // predetermined skill
    public System.String skill;
    public Enemy boss;
    public DisplaySkill skillToUnlock;


    public GameObject popUp;
    public string text;


    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    void UnlockPreDeterminedSkill() {
        skillToUnlock.GetSkill(false);
    }

    // Update is called once per frame
    // Update is called once per frame
    protected void Update() {
        if (!opened) {
            if (playerNear) {
                if (Input.GetButtonDown("Interact")) {
                    if (boss.currentHealth <= 0) {
                        anim.SetBool("isOpened", true);
                        lm.restoreLife(lifeBoost);
                        //Debug.Log(player.experience);
                        player.GainExperience(experience);
                        UnlockPreDeterminedSkill();
                        //Debug.Log(player.experience);
                        opened = true;
                        Debug.Log(skill);
                        canvasChestText.text = " You gained " + experience + " experience and " + lifeBoost + " life. You also unlocked a new skill: " + skill;
                        canvasChest.SetActive(true);

                        popUp.GetComponent<Advice>().setText(text);
                    }
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
}


