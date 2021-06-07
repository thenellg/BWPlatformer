using UnityEngine;

public class Skateboard : MonoBehaviour
{
    public Vector2 originalPos;
    Vector3 playerPos = new Vector3(-0.67f, 4.54f, 0f);
    public bool moving = false;
    public float speed = 12f;

    public CharacterController2D m_CharacterController2D;
    public bool facingRight = true;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    // Update is called once per frame
    void Update()
    {
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

            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //collision.gameObject.GetComponent<PlayerController>();
            collision.transform.parent = transform;
            collision.transform.localPosition = playerPos;
            collision.gameObject.GetComponent<Rigidbody2D>().simulated = false;
            //collision.gameObject.GetComponent<CharacterController2D>().skateboarding = true;
            moving = true;
        }
        if (collision.gameObject.tag == "MovingPlatform")
        {
            this.transform.parent = collision.transform.parent;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MovingPlatform")
        {
            this.transform.parent = null;
        }
    }
}
