using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class HitPlayer : MonoBehaviour
{
    public float damage = 5;
    public GameObject damageTo;
    public float knockDuration;
    public float knockPower;

    private LifeManager lm;
    // Start is called before the first frame update
    void Start()
    {
        damageTo = GameObject.FindGameObjectWithTag("Player");
        lm = damageTo.GetComponent<LifeManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            lm.takeHit(damage);
            Player player = damageTo.GetComponent<Player>();
            StartCoroutine(player.Knockback(0.02f,25f,player.transform.position, false, false));
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.gameObject.name == "Player") {
            lm = collision.collider.GetComponent<LifeManager>();
            if (lm != null) {
                lm.takeHit(damage);
            }
            if (gameObject.tag == "EnemyRobot")
            {
                Player player = damageTo.GetComponent<Player>();
                Enemy robot = gameObject.GetComponent<Enemy>();
                StartCoroutine(player.Knockback(0.02f, 250f, robot.getVelocity(), true, robot.isFacingRight()));
            }

            else if (gameObject.name == "Boss")
            {
                Player player = damageTo.GetComponent<Player>();
                Enemy robot = gameObject.GetComponent<Enemy>();
                StartCoroutine(player.Knockback(0.02f, 400f, robot.getVelocity(), true, robot.isFacingRight()));
            }
        }
    }

    void OnCollisionExit2D(Collision2D collisionInfo)
    {
        print("Collision Out: " + gameObject.name);
    }
}
