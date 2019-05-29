using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySkill : MonoBehaviour
{
    public SkillNode skill;
    public Text skillName;
    public Text skillCost;
    public Image skillIconActive;
    public Image skillIconEnabled;
    public Image skillIconDisabled;

    public Text skillDescription;
    public Player player;

    public SkillNode previousUnlock;

    private Button button;
    // Start is called before the first frame update
    void Start()
    {
       if (skill)
        {
            skill.setValues(this.gameObject);
            if (skill.name == "Double Jump" || skill.name == "Wall Break" || skill.name == "Wall Jump")
            {
                GetSkill(false);
            }
        }
        player.OnAbilityPointChange += AbilityPointHandler;
    }

    public void EnableSkills()
    {
        if (player && skill && skill.CheckIfPlayerHasSkill(player))
        {
            Debug.Log("Turning skill icon on for " + name);
            TurnOnSkillIcon();
        }
        else if ((player && skill && !previousUnlock & skill.unlockable && skill.CheckIfCanUnlockSkill(player)) || (player && skill && previousUnlock & skill.unlockable && skill.CheckIfCanUnlockSkill(player) && previousUnlock.CheckIfPlayerHasSkill(player)))
        {
            TurnOnCanChooseIcon();
        }
        else
        {
            Debug.Log("Turning skill icon off for " + name);
            TurnOffSkillIcon();
        }
    }


    private void OnEnable()
    {
        button = this.GetComponent<Button>();
        
        EnableSkills();
    }

    public void GetSkill(bool unlockable)
    {
        if (previousUnlock)
        {
            if (previousUnlock.CheckIfPlayerHasSkill(player) && skill.GetSkill(player, unlockable))
            {
                TurnOnSkillIcon();
            }
        }
        else {
            if (skill.GetSkill(player, unlockable))
            {
                TurnOnSkillIcon();
            }
        }
    }

    private void TurnOnSkillIcon()
    {
        if (button) {
            button.interactable = false;
        }
        this.transform.Find("Icon").gameObject.GetComponent<Image>().enabled = true;
        this.transform.Find("Icon").Find("Available").gameObject.SetActive(false);
        this.transform.Find("Icon").Find("Disabled").gameObject.SetActive(false);
    }

    private void TurnOffSkillIcon()
    {
        if (button)
        {
            button.interactable = false;
        }
        this.transform.Find("Icon").gameObject.GetComponent<Image>().enabled = false;
        this.transform.Find("Icon").Find("Available").gameObject.SetActive(false);
        this.transform.Find("Icon").Find("Disabled").gameObject.SetActive(true);
    }

    private void TurnOnCanChooseIcon()
    {
        if (button)
        {
            button.interactable = true;
        }
        this.transform.Find("Icon").gameObject.GetComponent<Image>().enabled = false;
        this.transform.Find("Icon").Find("Available").gameObject.SetActive(true);
        this.transform.Find("Icon").Find("Disabled").gameObject.SetActive(false);
    }

    void AbilityPointHandler(int newVal)
    {
        if (newVal < this.skill.attributePoints && !skill.CheckIfPlayerHasSkill(player))
        {
            TurnOffSkillIcon();
        }
    }
}
