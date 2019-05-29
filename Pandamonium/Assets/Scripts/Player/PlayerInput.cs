using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    private Player player;
    private float dir;
    private bool directionFrozen = false;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (directionFrozen)
        {
            if (directionalInput.x == dir)
            {
                directionalInput.x = 0f;
            }
        }
        player.SetDirectionalInput(directionalInput);

        if (Input.GetButtonDown("Jump"))
        {
            player.OnJumpInputDown();
        }

        if (Input.GetButtonUp("Jump"))
        {
            player.OnJumpInputUp();
        }

        if (Input.GetButtonDown("Rage"))
        {
            player.Rage();
        }

        if (Input.GetButtonDown("QAbility"))
        {
            player.handleQInput();
        }

        if (Input.GetButtonDown("Attack")) {
            player.Attack();
        }

        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
        {
            player.Dash(true);
        }

        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0)
        {
            player.Dash(false);
        }


    }

    public void FreezeDirection(float dir)
    {
        if (dir != 0f) {
            directionFrozen = true;
        }
        else
        {
            directionFrozen = false;
        }
        this.dir = dir;
    }
}
