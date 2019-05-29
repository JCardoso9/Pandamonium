using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialMenu : MonoBehaviour
{
    public GameObject[] toDisable;
    public GameObject tabelaTeclas;
    public GameObject back;
    public GameObject credits;

    public void Playgame()
    {
        SceneManager.LoadScene(sceneName: "Main");
    }

    public void Credits()
    {
        credits.SetActive(true);
        back.SetActive(true);

        foreach (GameObject obj in toDisable)
        {
            obj.SetActive(false);
        }

    }

    public void Controls()
    {
        tabelaTeclas.SetActive(true);
        back.SetActive(true);

        foreach (GameObject obj in toDisable)
        {
            obj.SetActive(false);
        }
    }

    public void QuitGame()
    {
        //i think that only works when build, to lazy to try
        Application.Quit();
    }

    public void goBack()
    {
        foreach (GameObject obj in toDisable)
        {
            obj.SetActive(true);
        }

        back.SetActive(false);
        tabelaTeclas.SetActive(false);
        credits.SetActive(false);
    }
}