using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(CharacterController2D))]
public class Player : MonoBehaviour
{
    private BoxCollider2D coll;
    public Animator animator;

    //rage bar
    public Slider inRageBar;
    public GameObject fill;

    //movement
    private SpriteRenderer spriteRenderer;
    private bool facingRight = true;
    public float maxJumpHeight = 4f;
    public float minJumpHeight = 1f;
    public float timeToJumpApex = .4f;
    private float accelerationTimeAirborne = .2f;
    private float accelerationTimeGrounded = .1f;
    private float moveSpeed = 14f;
    private float gravity;
    private float maxJumpVelocity;
    private float minJumpVelocity;
    private Vector3 velocity;
    private float velocityXSmoothing;
    private bool canJump = true;

    //WRITE TO FILE
    private GameObject writer;


    //Rage
    //estas duas variaveis tem de ter o mm valor!!!
    private float timeInRage = 15.0f;
    private const float timeInRageDefault = 15.0f;
    //estas duas variaveis tem de ter o mm valor!!!
    private float rageCooldown = 5.0f;
    private const float rageCooldownDefault = 5.0f;
    private bool inRage = false;

    //Damage
    private float damage = 2.5f;
    private float damageInRage = 2.5f;

    //Break Wall
    public bool breakWall = false;

    //Wall Jump & slide
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;
    public float wallSlideSpeedMax = 3f;
    public float wallStickTime = .25f;
    private float timeToWallUnstick;
    private bool wallSliding;
    private int wallDirX;

    //Dash
    public float dashSpeed = 20f;
    private float dashTime = 0.2f;
    private int dashLeft = 0;
    private int dashRight = 0;
    private bool startTimer;
    private float timePassed;

    //Cameras
    public Camera mainCamera;
    public Camera xRayShaderCamera;
    public Camera hiddenObjectsCamera;

    //Double Jump
    public bool canDoubleJump;
    private bool isDoubleJumping = false;

    //Ground slam
    private float slamCoolDown = 3f;
    private float groundSlamTimeElapsed = 0f;
    public Transform groundSlamAttack;
    public float groundSlamRange;
    public float stunDuration;
    private bool canGroundSlam = true;
    public float groundSlamDamage = 2.5f;
    private bool canGroundSlamStun = false;
    public float groundSlamStunDuration = 2f;

    //Normal attack
    public Transform attackPos;
    public float attackRange;
    public bool canAttack = true;
    public float rageDamage = 5f;
    public float normalDamage = 2.5f;
    public float attackStunDuration;
    private float timeBtwAttack = 0.7f;
    private float startTimeBtwAttack = 0.7f;

    private CharacterController2D controller;

    private Vector2 directionalInput;

    private List<SkillNode> enabledSkills = new List<SkillNode>();


    //experience 
    private int level = 10;
    private float[] experiencePerLevel = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 100, 250, 350, 500 };
    private float experience = 80;
    private float levelExperience;
    private Color32 yellow = new Color32(231, 213, 5, 255); //yellow
    public Slider sliderExperience;
    public GameObject fillExperience;
    private int abilityPoint = 0; //points to buy skills
    public delegate void OnAbilityPointChangeDelegate(int newVal);
    public event OnAbilityPointChangeDelegate OnAbilityPointChange;

    /*public int experience;
    public int level;
    public int experienceForFirstLevel;
    private int experienceUntillNextLevel;*/


    public float fallMultiplier = 2.5f;

    // X-ray ability
    public bool canUseXRays = false;

    public GameObject sun;

    public static Player instance;

    private void Awake() {
        animator = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>(); ;
        controller = GetComponent<CharacterController2D>();
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        inRageBar.value = 1;
        levelExperience = experiencePerLevel[level];
        fillExperienceBar();

        //write to file
        writer = GameObject.Find("/WriteOnFile");
    }

    void Start()
    {
        
    }

    private void Update()
    {

        CalculateVelocity();
        HandleWallSliding();
        if(velocity.y < 0)
        {
            velocity.y += Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        Vector2 move = velocity * Time.deltaTime;

        CanDash(ref move);

        controller.Move(move, directionalInput);

        if (controller.collisions.above || controller.collisions.below)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isDoubleJumping", false);
            velocity.y = 0f;
            if (controller.collisions.below) {
                canJump = true;
            }
        }

        if (inRage)
        {
            timeInRage -= Time.deltaTime;
            if (timeInRage <= 0)
            {
                DisableRage();
            }

            fill.GetComponent<Image>().color = new Color32(255, 142, 0, 255); //orange
            float inRage0to1 = Mathf.Clamp01(timeInRage / timeInRageDefault);
            inRageBar.value = inRage0to1;

            /*if (Input.GetButtonDown("BreakWall"))
            {
                breakWall = true;
            }*/
        }
        else if (rageCooldown >= 0.0f && rageCooldown < rageCooldownDefault)
        {
            rageCooldown += Time.deltaTime;
            if (rageCooldown >= rageCooldownDefault)
            {
                rageCooldown = rageCooldownDefault;
            }
        }

        if (!inRage)
        {
            fill.GetComponent<Image>().color = new Color32(130, 130, 130, 255);
            float rage0to1 = Mathf.Clamp01(rageCooldown / rageCooldownDefault);
            inRageBar.value = rage0to1;
        }
        if (inRageBar.value == 1)
        {
            fill.GetComponent<Image>().color = new Color32(255, 142, 0, 255);
        }

        if (!canGroundSlam)
        {
            if (groundSlamTimeElapsed < slamCoolDown)
            {
                groundSlamTimeElapsed += Time.deltaTime;
            }
            else
            {
                canGroundSlam = true;
                groundSlamTimeElapsed = 0f;
            }
        }

        if (!canAttack)
        {
            timeBtwAttack -= Time.deltaTime;
            if (timeBtwAttack <= 0)
            {
                animator.SetBool("isPunching", false);
                canAttack = true;
                timeBtwAttack = startTimeBtwAttack;

            }
        }
        if (move.x < 0f && facingRight || move.x > 0f && !facingRight)
        {
            FlipPlayer();

        }

    }


    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    public void OnJumpInputDown()
    {
        animator.SetBool("isJumping", true);
        if (!inRage && wallSliding)
        {
            if (wallDirX == directionalInput.x)
            {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0)
            {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else
            {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
                writer.GetComponent<WriteOnFile>().WriteToFile("wall jump");
            }

        }
        if (controller.collisions.below || canJump)
        {
            velocity.y = maxJumpVelocity;
            animator.SetBool("isDoubleJumping", false);
            isDoubleJumping = false;
            canJump = false;
        }
        else if (!inRage && canDoubleJump && !controller.collisions.below && !isDoubleJumping && !wallSliding)
        {
            velocity.y = maxJumpVelocity;
            animator.SetBool("isJumping", false);
            animator.SetBool("isDoubleJumping", true);
            isDoubleJumping = true;
            writer.GetComponent<WriteOnFile>().WriteToFile("double jump");
        }
    }

    public void OnJumpInputUp()
    {

        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }

    }

    private void HandleWallSliding()
    {
        if (!inRage)
        {
            wallDirX = (controller.collisions.left) ? -1 : 1;
            wallSliding = false;
            if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
            {
                wallSliding = true;

                if (velocity.y < -wallSlideSpeedMax)
                {
                    velocity.y = -wallSlideSpeedMax;
                }

                if (timeToWallUnstick > 0f)
                {
                    velocityXSmoothing = 0f;
                    velocity.x = 0f;
                    if (directionalInput.x != wallDirX && directionalInput.x != 0f)
                    {
                        timeToWallUnstick -= Time.deltaTime;
                    }
                    else
                    {
                        timeToWallUnstick = wallStickTime;
                    }
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
        }
    }

    private void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne));
        animator.SetFloat("SpeedX", Mathf.Abs(velocity.x));
        velocity.y += gravity * Time.deltaTime;
        animator.SetFloat("SpeedY", velocity.y);
    }

    public void Rage()
    {
        if (inRage)
        {
            DisableRage();
        }
        else if (rageCooldown >= rageCooldownDefault)
        {
            EnableRage();
        }
        writer.GetComponent<WriteOnFile>().WriteToFile(inRage ? "rage mode" :  "normal mode");

    }

    public void XRayGoggles()
    {
        if (canUseXRays || CheckIfUnlockedSkill("X-Ray Goggles"))
        {
            canUseXRays = true;
            writer.GetComponent<WriteOnFile>().WriteToFile("XRayGoggles");
            if (Camera.main == mainCamera)
            {
                foreach (GameObject child in GameObject.FindGameObjectsWithTag("Chest"))
                {
                    child.GetComponent<SpriteGlow.SpriteGlowEffect>().enabled = true;
                }
                foreach (Transform child in GameObject.Find("X-Ray").transform)
                {
                    child.GetComponent<BoxCollider2D>().enabled = true;
                }
                mainCamera.enabled = false;
                xRayShaderCamera.enabled = true;
                hiddenObjectsCamera.enabled = true;
                sun.active = false;
            }
            else
            {
                foreach (Transform child in GameObject.Find("X-Ray").transform)
                {
                    child.GetComponent<BoxCollider2D>().enabled = false;
                }

                foreach (GameObject child in GameObject.FindGameObjectsWithTag("Chest"))
                {
                    child.GetComponent<SpriteGlow.SpriteGlowEffect>().enabled = false;
                }
                mainCamera.enabled = true;
                xRayShaderCamera.enabled = false;
                hiddenObjectsCamera.enabled = false;
                sun.active = true;
            }
        }
    }

    public void Dash(bool right)
    {
        foreach (SkillNode skill in enabledSkills)
        {
            if (skill.name == "Mid-air dash" && !inRage) {
                if (!controller.collisions.below)
                {
                    if (right)
                    {

                        dashRight += 1;
                    }
                    else
                    {
                        dashLeft += 1;
                    }
                    startTimer = true;
                }
            }
        }
    }

    private void DisableRage()
    {
        animator.SetBool("Rage", false);
        inRage = !inRage;
        moveSpeed = 14f;
        timeInRage = timeInRageDefault;
        rageCooldown = 0.0f;
    }

    private void EnableRage()
    {
        animator.SetBool("Rage", true);
        moveSpeed = 10f;
        inRage = !inRage;
        rageCooldown = rageCooldownDefault;
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
            else if (!controller.collisions.below)
            {
                if (dashLeft >= 2)
                {
                    move.x = -dashSpeed;
                    dashLeft = 0;
                    dashRight = 0;

                    writer.GetComponent<WriteOnFile>().WriteToFile("dash left");

                }
                else if (dashRight >= 2)
                {
                    move.x = dashSpeed;
                    dashRight = 0;
                    dashLeft = 0;

                    writer.GetComponent<WriteOnFile>().WriteToFile("dash right");
                }
            }
        }
    }

    public IEnumerator Knockback(float knockDur, float knockPwr, Vector2 knockDir, bool movingTarget, bool isFacingRight)
    {
        float timeElapsed = 0;
        float movingThreshold = 0.5f;
        while (knockDur > timeElapsed)
        {
            timeElapsed += Time.deltaTime;
            if (!movingTarget)
            {
                if (velocity.x > 0)
                {
                    velocity.x = (-velocity.x * knockPwr) / (velocity.x);
                }
                else
                {
                    velocity.x = (-velocity.x * knockPwr) / (-velocity.x);
                }
            }
            else
            {
                int newDirection = 1;
                if ((velocity.x > movingThreshold && isFacingRight) || (velocity.x < -movingThreshold && !isFacingRight))
                {
                    if (isFacingRight)
                    {
                        newDirection = -1;
                    }
                    velocity.x = knockDir.x * knockPwr * newDirection;
                }
                else
                {
                    if (!isFacingRight)
                    {
                        newDirection = -1;
                    }
                    velocity.x = knockDir.x * knockPwr * newDirection;
                }
            }
            if (velocity.y != 0)
            {
                velocity.y = 15;
            }
            else
            {
                velocity.y = 0;
            }
        }
        yield return 0;
    }

    public void GainExperience(int experienceGained)
    {
        experience += experienceGained;


        if (experience > levelExperience)
        {

            level += 1;
            levelExperience = experiencePerLevel[level]; //experience of the new level
            // Gain special ability point
            abilityPoint += 1;
            if (OnAbilityPointChange != null)
                OnAbilityPointChange(this.abilityPoint);

            writer.GetComponent<WriteOnFile>().WriteToFile("level upgrade to: " + level);
        }

        fillExperienceBar();
    }

    public void handleQInput()
    {
        if (inRage)
        {
            this.GroundSlam();
        }
        else
        {
            this.XRayGoggles();
        }
    }

    public void GroundSlam()
    {
        if (canGroundSlam && controller.collisions.below)
        {
            if (canGroundSlamStun || CheckIfUnlockedSkill("Roar")){
                canGroundSlamStun = true;
                Collider2D[] enemiesToStun = Physics2D.OverlapCircleAll(groundSlamAttack.position, groundSlamRange, LayerMask.GetMask("Enemy"));
                for (int i = 0; i < enemiesToStun.Length; i++)
                {
                    enemiesToStun[i].GetComponent<Enemy>().TakeDamage(groundSlamDamage);
                    enemiesToStun[i].GetComponent<Enemy>().stunEnemy(groundSlamStunDuration);
                }
            }
            else
            {
                Collider2D[] enemiesToStun = Physics2D.OverlapCircleAll(groundSlamAttack.position, groundSlamRange, LayerMask.GetMask("Enemy"));
                for (int i = 0; i < enemiesToStun.Length; i++)
                {
                    enemiesToStun[i].GetComponent<Enemy>().TakeDamage(groundSlamDamage);
                }
            }
            Collider2D[] toBreak = Physics2D.OverlapCircleAll(groundSlamAttack.position, groundSlamRange, LayerMask.GetMask("Breakable"));
            foreach (var c in toBreak)
            {
                c.gameObject.SetActive(false);
            };

            canGroundSlam = false;
            writer.GetComponent<WriteOnFile>().WriteToFile("Ground Slam");
        }
    }

    public void Attack()
    {
        StartCoroutine("AttackRoutine");
    }

    IEnumerator AttackRoutine()
    {
        if (canAttack)
        {
            animator.SetBool("isPunching", true);
            canAttack = false;
            Collider2D[] enemiesToStun = Physics2D.OverlapCircleAll(attackPos.position, attackRange, LayerMask.GetMask("Enemy"));
            for (int i = 0; i < enemiesToStun.Length; i++)
            {
                if (enemiesToStun[i])
                {
                    if (inRage)
                    {
                        if (enemiesToStun[i])
                        {
                            enemiesToStun[i].GetComponent<Enemy>().TakeDamage(rageDamage);
                        }
                        if (!enemiesToStun[i].gameObject.tag.Equals("Boss") && !enemiesToStun[i].GetComponent<Enemy>().IsStunned())
                        {
                            enemiesToStun[i].GetComponent<Enemy>().stunEnemy((attackStunDuration*2)+4/12f);
                        }
                        yield return new WaitForSeconds(4/12f);
                        if (enemiesToStun[i]) {
                            enemiesToStun[i].GetComponent<Enemy>().TakeDamage(rageDamage);
                        }
                    }
                    else
                    {
                        if (enemiesToStun[i])
                        {
                            enemiesToStun[i].GetComponent<Enemy>().TakeDamage(normalDamage);
                        }
                        if (!enemiesToStun[i].gameObject.tag.Equals("Boss") && !enemiesToStun[i].GetComponent<Enemy>().IsStunned())
                        {
                            enemiesToStun[i].GetComponent<Enemy>().stunEnemy(attackStunDuration);
                        }
                    }
                    
                }
            }
            if (inRage)
            {
                Collider2D[] wallsToBreak = Physics2D.OverlapCircleAll(attackPos.position, attackRange, LayerMask.GetMask("Breakable"));
                for (int i = 0; i < wallsToBreak.Length; i++)
                {
                    if (wallsToBreak[i].enabled)
                    {
                        wallsToBreak[i].gameObject.SetActive(false);
                    }
                }
            }

            writer.GetComponent<WriteOnFile>().WriteToFile(inRage ? "rage atack" : "normal atack");
        }
    }

        //Helper method to visualize the range of abilities, do not remove pls
        //void OnDrawGizmosSelected()
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawWireSphere(attackPos.position, attackRange);
        //}

        public bool getMode()
    {
        return inRage;
    }

    public float getDamage()
    {
        if (inRage)
        {
            return damageInRage;
        }
        else
        {
            return damage;
        }
    }

    public bool CheckIfUnlockedSkill(string skillName)
    {
        foreach (SkillNode skill in enabledSkills)
        {
            if (skill.name == skillName)
            {
                return true;
            }
        }
        return false;
    }

    private void FlipPlayer()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void fillExperienceBar()
    {
        fillExperience.GetComponent<Image>().color = yellow;
        float experience0to1 = Mathf.Clamp01(experience / levelExperience);
        //Debug.Log(experience0to1 + "   " + experience + "   " +  levelExperience);
        sliderExperience.value = experience0to1;
    }

    public List<SkillNode> getEnabledSkills()
    {
        return enabledSkills;
    }

    public int GetAttributePoints()
    {
        return abilityPoint;
    }

    public void DecrementAttributePoints()
    {
        Debug.Log("Decrementing ... " + this.abilityPoint);
        this.abilityPoint -= 1;
        Debug.Log("Done Decrementing ... " + this.abilityPoint);
        if (OnAbilityPointChange != null)
            OnAbilityPointChange(this.abilityPoint);
        Debug.Log("Called listener ... " + this.abilityPoint);
    }
}
