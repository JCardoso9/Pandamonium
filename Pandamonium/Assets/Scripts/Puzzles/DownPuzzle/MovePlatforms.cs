using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatforms : MonoBehaviour
{

    public Vector3 dir;
    private Rigidbody2D rb;
    private bool headingRight = true;
    public float lowerLimitX;
    public float upperLimitX;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dir = transform.right;
        Debug.Log(transform.right);
    }


    private void Update()
    {
        transform.position += dir * Time.deltaTime * 7f;
        if (transform.localPosition.x >= upperLimitX || transform.localPosition.x <= lowerLimitX)
        {
            dir.x = -dir.x;
            headingRight = !headingRight;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            dir = (headingRight == true) ? transform.right : -transform.right;
            collision.gameObject.GetComponent<PlayerInput>().FreezeDirection(0f);
            collision.transform.SetParent(null);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.transform.position.y - (collision.gameObject.GetComponent<BoxCollider2D>().size.y / 2));
        Debug.Log((gameObject.transform.position.y - gameObject.GetComponent<BoxCollider2D>().size.y / 2));
        if (collision.gameObject.name == "Player" && collision.gameObject.transform.position.y - (collision.gameObject.GetComponent<BoxCollider2D>().size.y / 2) >= (gameObject.transform.position.y - gameObject.GetComponent<BoxCollider2D>().size.y/2))
        {
            collision.transform.SetParent(transform);
        }
        else if (collision.gameObject.name == "Player")
        {
            collision.gameObject.GetComponent<PlayerInput>().FreezeDirection(-dir.x);
            dir.x = 0f;
        }
    }
}
