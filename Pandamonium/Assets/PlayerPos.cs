using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPos : MonoBehaviour
{
    GameMaster gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();

        if (gm.playerDead) {
            transform.position = gm.lastCheckpoint + new Vector2(-4,0);
            gm.playerDead = false;
        }
    }


    public void setPlayerPosition() {
        transform.position = gm.lastCheckpoint;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
