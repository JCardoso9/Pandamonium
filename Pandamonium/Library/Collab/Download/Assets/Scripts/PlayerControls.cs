using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour {
	public CharacterController2D controller;
    public GameObject hand; //a mao que aparece para simular o ataque

    //rage bar
    public Slider inRageBar;
    public GameObject fill;

    //movement
    private float horizontalMove = 0f;
    private bool jump = false;
    private bool crouch = false;
    private float runSpeed = 40f;

    //rage
    private float timeInRage = 10.0f;
    private float rageCooldown = 20.0f;
    private bool inRage = false;

    //damage
    private float damage = 5.0f;
    private float damageInRage = 7.0f;

    //habilidades
    public bool breakWall = false;

    void start()
    {
        inRageBar.value = 1;
    }

	// Update is called once per frame
	void Update () {

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
		}
        else if (Input.GetButtonDown("Crouch"))
		{
			crouch = true;
		} else if (Input.GetButtonUp("Crouch"))
		{
			crouch = false;
		}
        else if (Input.GetButtonDown("Rage"))
        {
            if (inRage) {
                DisableRage();
            }
            else if (rageCooldown >= 20.0f)
            {
                EnableRage();
            }
        }
        else if (Input.GetButtonDown("Ataque"))
        {
            hand.SetActive(true);
        }
        //just to fix for now
        if (!Input.GetButtonDown("Ataque"))
        {
            hand.SetActive(false);
        }

        if (inRage) {
            timeInRage -= Time.deltaTime;
            if (timeInRage <= 0)
            {
                DisableRage();
            }

            fill.GetComponent<Image>().color = new Color32(255, 142, 0, 255); //orange
            float inRage0to1 = Mathf.Clamp01(timeInRage / 10.0f);
            inRageBar.value = inRage0to1;

            if (Input.GetButtonDown("BreakWall"))
            {
                breakWall = true;
            }
        }
        else if (rageCooldown >= 0.0f && rageCooldown < 20.0f)
        {
            rageCooldown += Time.deltaTime;
            if (rageCooldown >= 20.0f)
            {
                rageCooldown = 20.0f;
            }
        }

        if (!inRage)
        {
            fill.GetComponent<Image>().color = new Color32(130, 130, 130, 255);
            float rage0to1 = Mathf.Clamp01(rageCooldown / 20.0f);
            inRageBar.value = rage0to1;
        }
        if(inRageBar.value == 1)
        {
            fill.GetComponent<Image>().color = new Color32(255, 142, 0, 255); //orange
        }
        

    }

	void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
		jump = false;
	}

    private void DisableRage()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.white;
        inRage = !inRage;
        runSpeed = 40.0f;
        timeInRage = 10.0f;
        rageCooldown = 0.0f;
    }

    private void EnableRage()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        runSpeed = 25.0f;
        inRage = !inRage;
        rageCooldown = 20.0f;
    }
}
