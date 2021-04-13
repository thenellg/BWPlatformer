using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slideReset : MonoBehaviour
{
    float temp;
    public float speed = 10f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>())
        {
            temp = collision.gameObject.GetComponent<Rigidbody2D>().gravityScale;
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = temp * speed;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>())
        {
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = temp;
        }
    }
}
