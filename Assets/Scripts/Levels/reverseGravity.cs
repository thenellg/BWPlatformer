using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reverseGravity : MonoBehaviour
{
    public bool normalGrav = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>())
        {
            gravFlip(collision.gameObject);
        }
    }
    
    public void gravFlip(GameObject flippin)
    {

        Rigidbody2D m_Rigidbody2D = flippin.GetComponent<Rigidbody2D>();

        Debug.Log("grav");
        if (m_Rigidbody2D.gravityScale > 0 && normalGrav == false || m_Rigidbody2D.gravityScale < 0 && normalGrav == true)
        {
            m_Rigidbody2D.gravityScale = -m_Rigidbody2D.gravityScale;
            Vector3 theScale = flippin.transform.localScale;
            theScale.y *= -1;
            flippin.transform.localScale = theScale;
        }
    }
}
