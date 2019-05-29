using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour
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
            StartCoroutine(player.Knockback(0.02f,25f,player.transform.position));
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        lm = collision.collider.GetComponent<LifeManager>();
        if (lm != null) {
            lm.takeHit(damage);
        }
    }
}
