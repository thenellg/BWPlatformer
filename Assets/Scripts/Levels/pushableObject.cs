using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pushableObject : MonoBehaviour
{
    Rigidbody2D _rb;
    public GameObject normalState;
    public Vector3 initialSpot;
    public bool hanging;

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

        if (collision.gameObject.tag == "Player")
        {
            if (hanging && collision.gameObject.GetComponent<PlayerController>().downwardDash == true)
            {
                Debug.Log("down strike");
                this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                //this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -5), ForceMode2D.Impulse);
            }
        }
    }

    void detach(GameObject player)
    {
        this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!hanging)
                _rb.velocity = Vector3.zero;
        }

        if (collision.gameObject.tag == "MovingPlatform")
        {
            transform.parent = normalState.transform.parent;
            _rb.velocity = Vector3.zero;
        }
    
        if (!hanging)
            _rb.velocity = Vector3.zero;

    }
}
