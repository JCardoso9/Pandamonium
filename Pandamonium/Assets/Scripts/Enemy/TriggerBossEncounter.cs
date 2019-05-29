using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerBossEncounter : MonoBehaviour
{

    public GameObject wall;
    public Slider bossHealthBar;
    public GameObject boss;

    // Start is called before the first frame update
    void Start()
    {
        bossHealthBar.gameObject.SetActive(false);

        wall.gameObject.SetActive(false);

        boss.gameObject.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D other) {
        wall.gameObject.SetActive(true);
        boss.gameObject.SetActive(true);
        bossHealthBar.gameObject.SetActive(true);
    }
}
