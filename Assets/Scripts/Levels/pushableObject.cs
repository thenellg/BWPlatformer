using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pushableObject : MonoBehaviour
{
    Rigidbody2D _rb;
    public GameObject normalState;
    public Vector3 initialSpot;
    public bool hanging;
    public bool frozen = true;

    private void Start()
    {
        _rb = this.GetComponent<Rigidbody2D>();
        initialSpot = transform.position;
        normalState = transform.parent.gameObject;
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

        if (collision.gameObject.tag == "Player" && hanging && collision.gameObject.GetComponent<PlayerController>().downwardDash == true)
        {
            this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            frozen = false;
        }

        if (collision.gameObject.tag == "Box")
        {
            Debug.Log("box on box");
            if (collision.gameObject.GetComponent<Rigidbody2D>().velocity.y < 0)
            {
                collision.gameObject.GetComponent<pushableObject>().transform.parent = this.transform;
                _rb.velocity = Vector3.zero;
                //collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero
            }
        }

        _rb.velocity = Vector3.zero;

    }

    void detach(GameObject player)
    {
        this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        frozen = false;
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
    
        if (collision.gameObject.tag == "Box")
        {
            collision.transform.parent = collision.gameObject.GetComponent<pushableObject>().normalState.transform;
            _rb.velocity = Vector3.zero;
        }

         _rb.velocity = Vector3.zero;

    }
}
