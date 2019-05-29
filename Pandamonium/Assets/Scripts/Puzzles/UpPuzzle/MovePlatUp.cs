using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatUp : MonoBehaviour
{
    public Vector3 _inicio;
    public Vector3 _fim;
    float speed = -4f;
    bool i = false, f = false ;


    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition+= new Vector3(0, speed,0) * Time.deltaTime;

        if (transform.localPosition.y > _inicio.y || transform.localPosition.y < _fim.y)
            speed = -speed;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (collision.gameObject.transform.position.y - collision.gameObject.GetComponent<BoxCollider2D>().size.y / 2 <= transform.position.y)
            {
                if (collision.gameObject.GetComponent<CharacterController2D>().collisions.below)
                {
                    speed = -speed;
                }
            }
            else {
                collision.transform.SetParent(transform);
            }
            /*if (speed < 0)
                speed = -speed;*/
        }
        else if (collision.gameObject.tag == "EnemyRobot")
        {
            if (collision.gameObject.transform.position.y - collision.gameObject.GetComponent<BoxCollider2D>().size.y / 2 <= transform.position.y)
            {
                if (collision.gameObject.GetComponent<CharacterController2D>().collisions.below)
                {
                    speed = -speed;
                }
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.transform.SetParent(null);
        }
    }
}
