using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeTextController : MonoBehaviour
{

    public Text abilityPointsText;
    public Player player;
    private string initialText = "Ability Points : ";
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Attr points before : " + player.GetAttributePoints());
        player.OnAbilityPointChange += AbilityPointHandler;
        string text = initialText + player.GetAttributePoints();
        abilityPointsText.text = text;
        Debug.Log("Attr points after : " + player.GetAttributePoints());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*private void OnEnable()
    {
        Debug.Log("Attr points before : " + player.GetAttributePoints());
        string text = "Ability Points : " + player.GetAttributePoints();
        abilityPointsText.text = text;
    }*/

    private void AbilityPointHandler(int newValue)
    {
        string text = initialText + newValue.ToString();
        abilityPointsText.text = text;
    }

    
}
