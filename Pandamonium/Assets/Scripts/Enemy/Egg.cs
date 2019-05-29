using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour {

    public float speed;
    public float timeToSpawn;
    public GameObject baby;

    public Transform player;
    private Vector2 moveDirection;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        moveDirection = (player.position - transform.position).normalized * speed;
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y);

    }

    // Update is called once per frame
    void Update()
    {
        if (timeToSpawn <= 0) {
            DestroyEgg();
        }
        else {
            timeToSpawn -= Time.deltaTime;
        }
    }

    void DestroyEgg() {
        Vector3 preventDigging = new Vector3(0, 0.25f,0);
        Instantiate(baby, transform.position + preventDigging, Quaternion.identity, transform.parent);
        Destroy(gameObject);
    }
}
