using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject text;
    public Slider lifeBar;
    public float invincibileTimeAfterHurt = 2;

    private float _life = 200.0f;
    private Animator myAnim;

    //WRITE TO FILE
    private GameObject writer;

    private void Awake() {
        myAnim = this.gameObject.GetComponent<Animator>();

    }

    // Start is called before the first frame update
    void Start()
    {
        lifeBar.value = 1;
        writer = GameObject.Find("/WriteOnFile");
    }

    // Update is called once per frame
    void Update()
    {
        float rage0to1 = lifeToShowInBar(_life);
        lifeBar.value = rage0to1;
    }


    //retorna o valor a preencher na barra da vida
    //recebe um valor de 0 a 100 e passa o para 0 a 1
    private float lifeToShowInBar(float life)
    {
        return Mathf.Clamp01(life /200.0f);
    }

    public float life
    {
        get
        {
            return _life;
        }
        set
        {
            _life = value;
        }
    }

    public void takeHit(float damage) {
        _life = _life - damage;
        TriggerHurt(invincibileTimeAfterHurt);
        if (_life <= 0) {
            _life = 0;

            Debug.Log("Game Over");

            //write to file
            writer.GetComponent<WriteOnFile>().WriteToFile("Dead");

            //pause the game
            Time.timeScale = 0;
            text.GetComponent<UnityEngine.UI.Text>().text = "Game Over!";
            menu.SetActive(true);
        }
    }

    public void TriggerHurt(float hurtTime) {
        StartCoroutine(HurtBlinker(hurtTime));
    }

    IEnumerator HurtBlinker(float hurtTime) {
        // Ignore collision with Enemies
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        int playerLayer = LayerMask.NameToLayer("Player");
        int obstacleLayer = LayerMask.NameToLayer("Obstacle");
        Physics2D.IgnoreLayerCollision(enemyLayer, playerLayer);
        Physics2D.IgnoreLayerCollision(playerLayer, obstacleLayer);


        // Start looping blink anim
        myAnim.SetLayerWeight(1, 1);

        //Wait for invincibility to end
        yield return new WaitForSeconds(hurtTime);

        // Stop blinking and re-enable collision
        Physics2D.IgnoreLayerCollision(enemyLayer, playerLayer, false);
        Physics2D.IgnoreLayerCollision(playerLayer, obstacleLayer, false);
        myAnim.SetLayerWeight(1, 0);
    }
    

    public void restoreLife(float restore)
    {
        _life = _life + restore;
        if (_life >= 200)
        {
            _life = 200;
        }
    }
}
