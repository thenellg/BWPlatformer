using UnityEngine;

public class Skateboard : MonoBehaviour
{
    public Vector2 originalPos;
    Vector3 playerPos = new Vector3(-0.67f, 4.54f, 0f);
    public bool moving = false;
    public bool moving2 = false;
    public float speed = 12f;
    public float angle = 0.5f;
    public float jumpForce = 140f;
    private float speedBackUp;

    public CharacterController2D m_CharacterController2D;
    public Animator playerAnim;

    public bool facingRight = true;
    public Rigidbody2D rb;
    public Vector2 normalVec;
    private Transform player;
    private int coyoteTimer = 30;
    public PhysicsMaterial2D slippery;

    public bool isGrounded = false;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    public CapsuleCollider2D m_CapsuleCollider;
    public BoxCollider2D m_Trigger;
    private Vector3 velocity = Vector3.zero;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    bool frozen = true;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        m_CapsuleCollider = GetComponent<CapsuleCollider2D>();

        foreach (BoxCollider2D box in GetComponents<BoxCollider2D>())
        {
            if (box.isTrigger)
                m_Trigger = box;
        }
        speedBackUp = speed;
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        moving2 = true;
        rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (coyoteTimer > 0 && isGrounded == false)
        {
            coyoteTimer--;
        }
        else if (coyoteTimer <= 0 && isGrounded == true)
        {
            coyoteTimer = 30;
        }

        if (moving)
        {
            float move = Input.GetAxisRaw("Horizontal");
            if (move > 0 && !facingRight)
            {
                Flip();
                speed = speed * -1;
            }
            else if (move < 0 && facingRight)
            {
                Flip();
                speed = speed * -1;
            }


                Vector2 targetVelocity;
            if (facingRight)    
                targetVelocity = new Vector2(speed * 10f, rb.velocity.y);
            else
                targetVelocity = new Vector2(-speed * 10f, rb.velocity.y);

            //Get the 90* to the normal vector 
            if (moving2) 
                rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, m_MovementSmoothing);

            player.localPosition = playerPos;
            player.localRotation = Quaternion.Euler(Vector3.zero);
        }
        else if (frozen)
        {
            rb.velocity = Vector2.zero;
        }

        transform.up = normalVec;
    }

    private void FixedUpdate()
    {
        isGrounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
            }
        }


    }

    void playerJump()
    {
        Vector2 temp = normalVec;
        temp.y *= jumpForce;

        rb.velocity = new Vector2(0, 0);
        rb.AddForce(temp, ForceMode2D.Impulse);
        normalVec = Vector2.up;
    }

    void swapColors()
    {
        m_CharacterController2D.swapColors();
    }

    public void Move(bool jump, bool dash)
    {
        if (dash)
        {
            dismount();
        }
        else if(!dash && jump && isGrounded && coyoteTimer > 0)
        {
            playerAnim.SetTrigger("Jump");
            playerJump();
            Invoke("swapColors", 0.2f);
        }
    }

    public void dismount()
    {
        //collision.transform.localPosition = playerPos;
        Debug.Log("Dismount Started");

        m_CapsuleCollider.enabled = false;
        

        moving = false;
        player.transform.parent = null;
        m_CharacterController2D.dashing();
        playerAnim = null;
        player = null;
        
        rb.sharedMaterial = null;

        Invoke("setZero", 0.5f);

        //m_CharacterController2D.facingRightCheck();
        m_CharacterController2D = null;

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().skateboarding = null;
        Debug.Log("Dismount Finished");
    }

    void setZero()
    {
        if (isGrounded)
        {
            rb.velocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            frozen = true;
        }
        else
        {
            Invoke("setZero", 0.1f);
        }

        m_Trigger.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("Wall - ", collision.gameObject);
            moving2 = false;
            rb.velocity = new Vector2(0f, 0f);
            speed = 0;
        }
        else if (collision.gameObject.layer == 3 && collision.gameObject.tag != "Wall")
        {
            Vector2 temp = collision.contacts[0].normal;
            if (temp.x <= angle && temp.x >= -angle && temp.y > 0)
            {
                normalVec = temp;
            }

            if (speedBackUp != speed && moving2)
                speed = speedBackUp;
        }

        if (collision.gameObject.tag == "MovingPlatform")
        {
            this.transform.parent = collision.transform.parent;
        }
        
        if (collision.gameObject.tag == "Balloon")
        {
            playerJump();
            collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = collision.transform;
            player.gameObject.GetComponent<PlayerController>().skateboarding = this.GetComponent<Skateboard>();
            collision.transform.parent = transform;
            collision.transform.localPosition = playerPos;

            playerAnim = collision.gameObject.GetComponent<Animator>();
            m_CharacterController2D = collision.gameObject.GetComponent<CharacterController2D>();
            if (m_CharacterController2D.m_FacingRight != facingRight)
            {
                Flip();
                m_CharacterController2D.forceFlip();
            }

            rb.sharedMaterial = slippery;
            rb.constraints = RigidbodyConstraints2D.None;

            m_CapsuleCollider.enabled = true;

            //collision.gameObject.GetComponent<Rigidbody2D>().simulated = false;
            frozen = false;
            moving = true;
            m_Trigger.enabled = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (moving && collision.gameObject.layer == 3)
        {
            isGrounded = false;
        }

        if (collision.gameObject.tag == "MovingPlatform")
        {
            this.transform.parent = null;
        }
    }
}
