using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashpadTrigger : MonoBehaviour
{
    public float dashSpeed = 2f;
    public float timer = 2f;
    public Vector2 dashDirection;
    public bool active = true;
    private SkateboardController player;

    private void Start()
    {
        player = FindObjectOfType<SkateboardController>();
    }

    public void dashpad(Rigidbody2D dasher)
    {
        if (active)
        {
            dasher.AddForce(dashDirection * dashSpeed, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.GetComponent<PlayerController>().isSkateboarding)
            {
                //turn player and up their speed
                if (player.m_FacingRight && dashDirection.x < 0)
                {
                    player.Flip();
                }
                else if (!player.m_FacingRight && dashDirection.x > 0)
                {
                    player.Flip();
                }

                if (player.speed == player.baseSpeed)
                    player.speed *= 1.25f;

                Invoke("slowDown", timer);

                dashpad(collision.GetComponent<Rigidbody2D>());
            }
        }
    }

    void slowDown()
    {
        player.speed = player.baseSpeed;
    }
}
