using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChageTextSkillTree : MonoBehaviour
{
    public Text _text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("BossChest") == null)
        {
            GameObject.Find("Normal").SetActive(false);
            GameObject.Find("Normal_XRay").SetActive(true);
        }
    }
}
