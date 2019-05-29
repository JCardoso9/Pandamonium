using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    //LifeManager lm;
    //GameObject player;
    GameMaster gm;
    //GameObject oldBoss;
    //Vector2 oldBossPosition;
    //public GameObject prefab;

    private void Start() {
        //player = GameObject.FindGameObjectWithTag("Player");
        //lm = player.GetComponent<LifeManager>();
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        //oldBoss = GameObject.FindGameObjectWithTag("Boss");
        //oldBossPosition = oldBoss.transform.position;
    }

    public void Controls()
    {
        Debug.Log("Controls");
    }

    public void Restart()
    {
        SceneManager.LoadScene("Initial Menu");
        //lm.restoreLife(100);
        //player.transform.position = gm.lastCheckpoint;
        //DestroyObject(oldBoss);
        //Instantiate(prefab);

        //SceneManager.LoadScene("Main");
        //SceneManager.LoadScene("Main");


        //string mySavedScene = PlayerPrefs.GetString("sceneName");
        //SceneManager.LoadScene(mySavedScene);

        //SceneManager.LoadScene("Main");
        // StartCoroutine(LoadScene());
    }

    /*IEnumerator LoadScene() {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Main");
    }*/
}