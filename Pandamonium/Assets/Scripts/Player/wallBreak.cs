using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallBreak : MonoBehaviour
{
    public Player playerControls;
    public GameObject[] toBreak;
    private bool breaked = false;

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if(playerControls.breakWall == true) 
            {
                foreach (GameObject obj in toBreak)
                {
                    obj.SetActive(false);
                 
                }
                this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                breaked = true;
            }
        }
    }
}
