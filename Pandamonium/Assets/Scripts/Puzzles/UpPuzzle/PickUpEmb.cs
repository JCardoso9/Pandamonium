using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpEmb : MonoBehaviour
{

    public GameObject pickup;
    public GameObject invt;
    

    // Start is called before the first frame update
    void Start()
    {
        pickup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
            pickup.SetActive(true);


    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "EnemyRobot")
        {
            Physics2D.IgnoreCollision(other, gameObject.GetComponent<Collider2D>());
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            gameObject.SetActive(false);
            invt.gameObject.SetActive(true);
            PutEmblemOnStone.hasArtifact = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        pickup.SetActive(false);
    }

}
