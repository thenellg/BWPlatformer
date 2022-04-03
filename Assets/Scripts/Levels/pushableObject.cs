using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pushableObject : MonoBehaviour
{
    Rigidbody2D _rb;
    public GameObject normalState = null;
    public Vector3 initialSpot;
    public bool hanging;
    public bool frozen = true;
    public bool hidden = true;
    private bool defaultMoving = false;
    private Transform movingParent = null;
    private bool initialActive = false;
    public bool breaksOnDamage = false;
    public bool instantFall = false;
    public float fallTime = 0.32f;

    public bool fanActive = false;
    public Vector2 fanForce;

    GameObject player;

    private void Start()
    {
        if (gameObject.activeSelf)
        {
            initialActive = true;
            hidden = false;
        }

        _rb = this.GetComponent<Rigidbody2D>();
        initialSpot = transform.position;

        if (transform.parent)
            normalState = transform.parent.gameObject;

        if (normalState.tag == "MovingPlatform")
        {
            defaultMoving = true;
            movingParent = normalState.transform;
            normalState = normalState.transform.parent.gameObject;
            initialSpot = transform.localPosition;
        }
    }

    private void Update()
    {
        if (fanActive)
            _rb.AddForce(fanForce);
    }

    public void moveBack()
    {
        if (defaultMoving)
            transform.parent = movingParent;

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        if (_rb)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = 0f;
        }

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
            transform.SetSiblingIndex(0);
        }
        if (_rb)
            _rb.velocity = Vector3.zero;

        if (collision.gameObject.tag == "Player" && hanging && collision.gameObject.GetComponent<PlayerController>().downwardDash == true)
        {
            player = collision.gameObject;
            detachBox();
        }
        else if(collision.gameObject.tag == "Player" && hanging && instantFall)
        {
            player = collision.gameObject;
            if(player.GetComponent<CharacterController2D>().m_Grounded == true)
                Invoke("detachBox", fallTime);
        }

        /*
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
        */

        _rb.velocity = Vector3.zero;

    }

    void detachBox()
    {
        this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        frozen = false;

        if (defaultMoving)
        {
            transform.parent = normalState.transform;
            player.transform.parent = null;
        }
        player = null;
    }

    void breakBox()
    {
        GetComponent<SpriteRenderer>().enabled = false;

        if (!hanging)
        {
            _rb.constraints = RigidbodyConstraints2D.None;
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            frozen = false;
        }
        moveBack();

        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
    }

    public void fanSet(Vector2 fanVelocity)
    {
        fanForce = fanVelocity;
        fanActive = true;
    }

    public void fanDeset()
    {
        _rb.AddForce(fanForce, ForceMode2D.Force);
        fanForce = Vector2.zero;
        fanActive = false;
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
            //THIS IS BEING TRIGGERED ON COLOR SWAPS CAUSING BOXES TO BREAK AWAY FROM MOVING PLATFORMS
            if(!hidden)
                transform.parent = normalState.transform;

            _rb.velocity = Vector3.zero;
        }
    
        if (collision.gameObject.tag == "Box")
        {
            collision.transform.parent = collision.gameObject.GetComponent<pushableObject>().normalState.transform;
            _rb.velocity = Vector3.zero;
        }

         _rb.velocity = Vector3.zero;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Death")
        {

            if (breaksOnDamage)
            {
                //trigger break animation
                GetComponent<BoxCollider2D>().enabled = false;
                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
                frozen = true;

                Invoke("breakBox", 0f);//Adjust time to fit animation
            }
        }
    }
}
