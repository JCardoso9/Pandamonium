using UnityEngine;

public class InputController : MonoBehaviour
{
    
    public GameObject skill;
    private bool skillIsActive = false;

    public GameObject menu;
    public GameObject text;
    public GameObject player; //check if player is not dead

    private bool pause = false;

    void Start()
    {
        Time.timeScale = 1;
    }

    private void Update()
    {
        //player is not dead
        if(player.GetComponent<LifeManager>().life > 0)
        {
            if (Input.GetButtonDown("SkillTree"))
            {
                if (skillIsActive)
                {
                    skill.SetActive(false);
                    Time.timeScale = 1;
                }
                else
                {
                    skill.SetActive(true);
                    Time.timeScale = 0;
                }
                skillIsActive = !skillIsActive;
            }

            if (Input.GetButtonDown("Pause"))
            {
                //check if player is dead 
                pause = !pause;
                if (pause)
                {
                    text.GetComponent<UnityEngine.UI.Text>().text = "Paused";
                    menu.SetActive(true);
                    Time.timeScale = 0;
                }
                else
                {
                    Time.timeScale = 1;
                    menu.SetActive(false);
                }

            }
        }
    }      
}
