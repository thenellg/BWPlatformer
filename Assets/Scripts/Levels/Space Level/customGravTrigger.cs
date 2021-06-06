using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customGravTrigger : MonoBehaviour
{
    //reverse Gravity Trigger
    public bool setActive = false;

    public bool down = false;
    public bool up = false;
    public bool left = false;
    public bool right = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<customGravController>())
        {

            customGravController temp = collision.gameObject.GetComponent<customGravController>();
            if (temp.isPlayer)
            {
                if (up)
                    temp.setForcedGrav("up");
                else if (down)
                    temp.setForcedGrav("down");
                else if (left)
                    temp.setForcedGrav("left");
                else if (right)
                    temp.setForcedGrav("right");
            }
            /*
            else if (!temp.isPlayer && setActive == false)
            {
                setActive = true;

                if (up)
                    temp.setForcedGrav("up");
                else if (down)
                    temp.setForcedGrav("down");
                else if (left)
                    temp.setForcedGrav("left");
                else if (right)
                    temp.setForcedGrav("right");
            }
            */
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>())
        {
            if (down)
            {
                Transform temp = collision.transform;
                Vector3 theScale = temp.localScale;
                if (theScale.y < 0)
                {
                    theScale.y *= -1;
                    temp.localScale = theScale;
                }
            }

            setActive = false;
        }
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
