using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class HitPlayer : MonoBehaviour
{
    public float damage = 5;
    public GameObject damageTo;

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
            StartCoroutine(player.Knockback(0.02f,25f,player.transform.position, false));
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.gameObject.name == "Player") { 
            lm = collision.collider.GetComponent<LifeManager>();
            if (lm != null) {
                lm.takeHit(damage);
            }
            GameObject parentObject = gameObject.transform.parent.gameObject;
            if (parentObject != null)
            {
                if (parentObject.tag == "EnemyRobot")
                {
                    Debug.Log("yeey");
                    Player player = damageTo.GetComponent<Player>();
                    Enemy robot = gameObject.GetComponent<Enemy>();
                    StartCoroutine(player.Knockback(0.02f, 7f, robot.getVelocity(), true));
                }
            }
        }
    }
}
