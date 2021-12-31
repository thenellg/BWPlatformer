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
    private bool defaultMoving = false;
    private Transform movingParent = null;
    private bool initialActive = false;

    private void Start()
    {
        if (gameObject.activeSelf)
        {
            initialActive = true;
        }

        _rb = this.GetComponent<Rigidbody2D>();
        initialSpot = transform.position;
        normalState = transform.parent.gameObject;
        if (normalState.tag == "MovingPlatform")
        {
            defaultMoving = true;
            movingParent = normalState.transform;
            normalState = normalState.transform.parent.gameObject;
            initialSpot = transform.localPosition;
        }
    }

    public void moveBack()
    {
        if (defaultMoving)
            transform.parent = movingParent;

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = 0f;

        if (defaultMoving)
            transform.localPosition = initialSpot;
        else
            transform.position = initialSpot;

        gameObject.SetActive(initialActive);
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
            this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            frozen = false;

            if (defaultMoving)
            {
                transform.parent = normalState.transform;
                collision.transform.parent = null;
            }
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
        this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
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
