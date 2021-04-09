using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pushableObject : MonoBehaviour
{
    Rigidbody2D _rb;
    public GameObject normalState;
    public Vector3 initialSpot;

    private void Start()
    {
        _rb = this.GetComponent<Rigidbody2D>();
        initialSpot = transform.position;
    }

    public void moveBack()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = 0f;
        transform.position = initialSpot;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MovingPlatform")
        {
            transform.parent = collision.gameObject.transform.parent;
        }
        _rb.velocity = Vector3.zero;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _rb.velocity = Vector3.zero;
        }

        if (collision.gameObject.tag == "MovingPlatform")
        {
            transform.parent = normalState.transform.parent;
            _rb.velocity = Vector3.zero;
        }
    }
}
