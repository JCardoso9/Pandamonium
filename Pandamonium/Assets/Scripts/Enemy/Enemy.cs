using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController2D))]
public class Enemy : MonoBehaviour {
    public LayerMask enemyMask;
    public float speed;
    public float health = 10;
    [HideInInspector]
    public float currentHealth;
    Rigidbody2D myBody;
    Transform myTrans;
    float myWidth, myHeight;
    public LayerMask layerMaskToIgnore;
    int enemyLayer;
    int playerLayer;
    private Vector2 myVel;
    private CharacterController2D controller;
    private float stunDuration = 0f;
    float timeElapsed = 0f;
    public GameObject bloodEffect;
    private bool isStunned = false;

    //experience
    public Player player;
    public int experience;

    protected void Start() {
        controller = GetComponent<CharacterController2D>();
        myTrans = this.transform;
        myBody = this.GetComponent<Rigidbody2D>();
        SpriteRenderer mySprite = this.GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        myWidth = mySprite.bounds.extents.x;
        myHeight = mySprite.bounds.extents.y;

        myVel = new Vector2(speed / 10, 0);

        // Ignore collision between other enemies
        enemyLayer = LayerMask.NameToLayer("Enemy");
        playerLayer = LayerMask.NameToLayer("Player");
        Physics2D.IgnoreLayerCollision(enemyLayer, enemyLayer);
        currentHealth = health;
    }

    public virtual void Update()
    {
        StunEnemy();
        
    }

    void FixedUpdate() {

        //Use this position to cast the isGrounded/isBlocked lines from
        Vector2 lineCastPos = myTrans.position.toVector2() + myTrans.right.toVector2() * myWidth - Vector2.up * myHeight/2;

        //Check to see if there's ground in front of us before moving forward
        Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down);
        bool isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, enemyMask);

        //Check to see if there's a wall in front of us before moving forward
        Debug.DrawLine(lineCastPos, lineCastPos - myTrans.right.toVector2() * .05f);

        layerMaskToIgnore = playerLayer | enemyLayer;
        bool isBlocked = Physics2D.Linecast(lineCastPos, lineCastPos - myTrans.right.toVector2() * .05f, layerMaskToIgnore);

        //If theres no ground, turn around. Or if I hit a wall, turn around
        if (!isGrounded || isBlocked) {
            Vector3 currRot = myTrans.eulerAngles;
            currRot.y += 180;
            myTrans.eulerAngles = currRot;
        }

        //Always move forward
        //myVel = myBody.velocity;
        //myVel.x = -myTrans.right.x * speed;
        //myBody.velocity = myVel;
        controller.Move(myVel, true);
    }

    public Vector2 getVelocity()
    {
        return myVel;
    }

    public bool isFacingRight()
    {
        bool facingRight = (gameObject.transform.rotation.y != -1) ? true : false;
        return facingRight;
    }

    public virtual void stunEnemy(float duration)
    {
        stunDuration = duration;
        isStunned = true;
    }

    public virtual void TakeDamage(float damage)
    {
        Instantiate(bloodEffect, transform.position, Quaternion.identity);
        this.currentHealth -= damage;
        Debug.Log(this.currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            player.GainExperience(experience);
            Destroy(gameObject);
        }
    }

    public virtual void StunEnemy()
    {
        if (timeElapsed<stunDuration)
        {
            timeElapsed += Time.deltaTime;
            myVel = new Vector2(0, 0);
}
        else
        {
            isStunned = false;
            stunDuration = 0;
            timeElapsed = 0;
            myVel = new Vector2(speed / 10, 0);
        }
    }

    public bool IsStunned()
    {
        return isStunned;
    }
}
