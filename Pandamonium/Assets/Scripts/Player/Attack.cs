using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject player;
    private float normalDamage = 5.0f;
    private float rageDamage = 10.0f;
    private float damage;

    void OnCollisionEnter2D(Collision2D collision)
    {
        damage = player.gameObject.GetComponent<Player>().getDamage();
        
        if (collision.collider.gameObject.layer == 11)
        {
            collision.collider.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
