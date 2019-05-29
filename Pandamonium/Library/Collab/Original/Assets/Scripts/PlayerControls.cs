using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControls : CharacterController2D {
    public GameObject hand; //a mao que aparece para simular o ataque
    

    //rage bar
    public Slider inRageBar;
    public GameObject fill;

    //movement
    public float maxSpeed = 12f;
    public float jumpTakeoffSpeed = 19f;
    private SpriteRenderer spriteRenderer;
    private bool facingRight = true;


    //rage
    private float timeInRage = 10.0f;
    private float rageCooldown = 20.0f;
    private bool inRage = false;

    //damage
    private float damage = 5.0f;
    private float damageInRage = 7.0f;

    //habilidades
    private int jumps = 0;
    public bool breakWall = false;
    private bool isWallSliding = false;
    private int direction;

    //Dash
    public float dashSpeed = 20f;
    private float dashTime = 0.5f;
    private int dashLeft = 0;
    private int dashRight = 0;
    private bool startTimer;
    private float timePassed;

    //Cameras
    public Camera mainCamera;
    public Camera xRayShaderCamera;
    public Camera hiddenObjectsCamera;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    void Start()
    {
        inRageBar.value = 1;

    }

	// Update is called once per frame
	protected override void ComputeVelocity() {

        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");


		if (Input.GetButtonDown("Jump") && (grounded || !doubleJump))
		{
            velocity.y = jumpTakeoffSpeed;

            if (!grounded && !doubleJump)
            {
                doubleJump = true;
            }
		}
        //Reduce velocity by half if player stops pressing jump for a "mini-jump"
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }
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
        else if (Input.GetButtonDown("X-Ray"))
        {
            if (Camera.main == mainCamera)
            {
                mainCamera.enabled = false;
                xRayShaderCamera.enabled = true;
                hiddenObjectsCamera.enabled = true;
            }
            else
            {
                mainCamera.enabled = true;
                xRayShaderCamera.enabled = false;
                hiddenObjectsCamera.enabled = false;
            }
        }
        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0 && (direction == 0 || direction == 1) && !grounded)
        {
            dashRight += 1;
            startTimer = true;
        }
        else if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0 && (direction == 0 || direction == -1) && !grounded)
        {
            dashLeft += 1;
            startTimer = true;
        }

        CanDash(ref move);

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
        
        bool flipSprite = spriteRenderer.flipX ? (move.x < 0f && facingRight) : (move.x > 0f && !facingRight);
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            facingRight = !facingRight;
        }

        targetVelocity = move * maxSpeed;

    }

    private void DisableRage()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.white;
        inRage = !inRage;
        maxSpeed = 12f;
        timeInRage = 10.0f;
        rageCooldown = 0.0f;
    }

    private void EnableRage()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        maxSpeed = 7f;
        inRage = !inRage;
        rageCooldown = 20.0f;
    }

    private void CanDash(ref Vector2 move)
    {
        if (startTimer)
        {
            timePassed += Time.deltaTime;
            if (timePassed > dashTime)
            {
                startTimer = false;
                dashLeft = 0;
                dashRight = 0;
                timePassed = 0;
            }
            else if (!grounded)
            {
                if (dashLeft >= 2)
                {
                    move.x = -dashSpeed;
                    dashLeft = 0;
                    dashRight = 0;
                }
                else if (dashRight >= 2)
                {
                    move.x = dashSpeed;
                    dashRight = 0;
                    dashLeft = 0;
                }
            }
        }
    }
}
