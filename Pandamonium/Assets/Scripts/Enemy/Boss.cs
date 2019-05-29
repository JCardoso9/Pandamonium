using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : Enemy
{

    public Slider bossHealthBar;
    public GameObject wall;

    private float timeBtwShots;
    public float startTimeBtwShots;
    public int startShotsBtwLittleGuys;
    private int shotsBtwLittleGuys;

    public GameObject projectile;
    public GameObject egg;
    //private Vector2 moveDirection;
    //Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        timeBtwShots = startTimeBtwShots;
        shotsBtwLittleGuys = startShotsBtwLittleGuys;

        wall = GameObject.Find("BossWall");

        //rb = GetComponent<Rigidbody2D>();

        //moveDirection = (player.position - transform.position).normalized * speed;
        //rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
    }

    // Update is called once per frame
    void Update()
    {

        //moveDirection = (player.position - transform.position).normalized * speed;
        //rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
        base.Update();
        bossHealthBar.value = currentHealth;

        if (timeBtwShots < 0) {

            if (shotsBtwLittleGuys <= 0 && currentHealth < health / 2 ) {
                Vector3 eggPosition = new Vector3(transform.position.x, transform.position.y, 0);
                Instantiate(egg, eggPosition, Quaternion.identity, transform.parent);
                shotsBtwLittleGuys = startShotsBtwLittleGuys;
            }
            else {
                Instantiate(projectile, transform.position, Quaternion.identity, transform.parent);
                shotsBtwLittleGuys -= 1;
            }
            timeBtwShots = startTimeBtwShots;
        }
        else {
            timeBtwShots -= Time.deltaTime;
        }

        
    }

    public override void TakeDamage(float damage) {
        Instantiate(bloodEffect, transform.position, Quaternion.identity);
        this.currentHealth -= damage;
        Debug.Log(this.currentHealth);

        if (currentHealth <= 0) {
            currentHealth = 0;
            player.GainExperience(experience);
            Destroy(transform.parent.gameObject);
            Destroy(wall);
            bossHealthBar.gameObject.SetActive(false);
        }   
    }
}
