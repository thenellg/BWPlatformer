using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pushableObject : MonoBehaviour
{
    Rigidbody2D _rb;

    private void Start()
    {
        _rb = this.GetComponent<Rigidbody2D>();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _rb.velocity = Vector3.zero;
        }
    }
}
