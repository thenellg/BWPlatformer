using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reverseGravity : MonoBehaviour
{
    float tempGravScale;
    public bool active = false;

    public Rigidbody2D player;
    public Vector2 gravity = new Vector2(0, -9.8f);
    public enum GravityDirection { Down, Left, Up, Right };

    private void Update()
    {
        if (active)
            player.AddForce(gravity, ForceMode2D.Force);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>())
        {
            player = collision.gameObject.GetComponent<Rigidbody2D>();
            tempGravScale = player.gravityScale;
            player.gravityScale = 0;



            active = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>())
        {
            player.gravityScale = tempGravScale;

            active = false;
        }
    }

    public void setGrav(GravityDirection direction)
    {

    }

    /*
    public void gravFlip(GameObject flippin)
    {

        Rigidbody2D m_Rigidbody2D = flippin.GetComponent<Rigidbody2D>();

        Debug.Log("grav");
        if (m_Rigidbody2D.gravityScale > 0 && gravDown == false || m_Rigidbody2D.gravityScale < 0 && gravDown == true)
        {
            m_Rigidbody2D.gravityScale = -m_Rigidbody2D.gravityScale;
            Vector3 theScale = flippin.transform.localScale;
            theScale.y *= -1;
            flippin.transform.localScale = theScale;
        }
    }
    */
}
