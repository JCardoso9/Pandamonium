using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Skill Node")]
public class SkillNode : ScriptableObject
{
    public string description;
    public string key;
    public int attributePoints;
    public bool unlockable;
    public Sprite thumbnail;
    public SkillNode previousUnlock;

    public void setValues(GameObject skillObject)
    {
        if (skillObject)
        {
            DisplaySkill ds = skillObject.GetComponent<DisplaySkill>();
            ds.skillName.text = name;
            ds.skillDescription.text = description;
            if (ds.skillIconActive)
            {
                ds.skillIconActive.sprite = thumbnail;
                ds.skillIconDisabled.sprite = thumbnail;
                ds.skillIconEnabled.sprite = thumbnail;
            }
            if (ds.skillDescription)
            {
                ds.skillDescription.text = description;
            }
            if (ds.skillCost)
            {
                ds.skillCost.text = attributePoints.ToString();
            }
            ds.previousUnlock = previousUnlock;
        }
    }

    public bool CheckIfCanUnlockSkill(Player player)
    {
        if (player.GetAttributePoints() >= attributePoints)
        {
            return true;
        }
        return false;
    }

    public bool CheckIfPlayerHasSkill(Player player)
    {
        List<SkillNode>.Enumerator skills = player.getEnabledSkills().GetEnumerator();
        while (skills.MoveNext())
        {
            Debug.Log(skills.Current.name);
            var currentSkill = skills.Current;
            if (currentSkill.name == this.name)
            {
                Debug.Log(currentSkill.name);
                Debug.Log(this.name);
                return true;
            }
            
        }
        return false;
    }

    public bool GetSkill(Player player, bool unlockable)
    {
        if (CheckIfCanUnlockSkill(player) && !CheckIfPlayerHasSkill(player)){
            player.getEnabledSkills().Add(this);
            if (unlockable) {
                player.DecrementAttributePoints();
            }
            return true;
        }
        return false;
    }
}
