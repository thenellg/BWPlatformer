using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour
{
	[Header("General")]
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching
	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 velocity = Vector3.zero;

	[Header("Specialized Jump")]
	private PlayerController m_PlayerController;
	public int coyoteTimer = 3;
	public bool doubleJump = false;
	public GameObject whiteStuff;
	public GameObject blackStuff;
	public float colorDelay = 0.2f;
	public AudioClip[] jumpSFX;
	public AudioClip deathSFX;

	[Header("WallJump")]
	public float wallJumpTime = 0.2f;
	public float wallSlideSpeed = 0.3f;
	public float wallDistance = 1.3f;
	[SerializeField] bool isWallSliding = false;
	[SerializeField] int jumpCounter = 0;
	RaycastHit2D wallCheckHit;
	float jumpTime;

	private Animator PlayerAnim;
	bool canMove = true;

	private void Awake()
	{
		//Set private values and disable stuff
		PlayerAnim = this.GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		m_PlayerController = GetComponent<PlayerController>();
		blackStuff.SetActive(false);
	}

	private void Update()
	{
		//Running coyote timer and using canMove
		if (coyoteTimer > 0 && m_Grounded == false)
		{
			coyoteTimer--;
		}
		
		canMove = m_PlayerController.canMove;
	}

	private void FixedUpdate()
	{
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				coyoteTimer = 30;
			}
		}

		PlayerAnim.SetBool("isGrounded", m_Grounded);

	}

	void jump()
    {
		float jumpForce;

		//Adjusting for reverse gravity
		if (m_Rigidbody2D.gravityScale < 0)
			jumpForce = -m_JumpForce;
		else
			jumpForce = m_JumpForce;

		Debug.Log(new Vector2(0f, jumpForce));

		m_Grounded = false;

		m_Rigidbody2D.velocity = new Vector2(0, 0);
		m_Rigidbody2D.AddForce(new Vector2(0, jumpForce));
	}

	public void deadSFX()
    {
		this.GetComponent<AudioSource>().PlayOneShot(deathSFX);
	}

	public void Move(float move, bool jump)
	{
		if (canMove)
		{
			if (move != 0 && m_Grounded)
			{
				PlayerAnim.SetBool("isWalking", true);
			}
			else if (!m_Grounded)
            {
				PlayerAnim.SetTrigger("fall");
				PlayerAnim.SetBool("isWalking", false);
			}
			else
			{
				PlayerAnim.SetBool("isWalking", false);
			}

			//only control the player if grounded or airControl is turned on
			if (m_Grounded || m_AirControl)
			{
				Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);                                                 // Move the character by finding the target velocity
				m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);     // And then smoothing it out and applying it to the character

				// If the input is moving the player right and the player is facing left...
				if (move > 0 && !m_FacingRight)
				{
					Flip();             // ... flip the player.
				}
				// Otherwise if the input is moving the player left and the player is facing right...
				else if (move < 0 && m_FacingRight)
				{
					Flip();             // ... flip the player.
				}
			}
			// If the player should jump...
			if (coyoteTimer > 0 && jump && jumpCounter < 1 || isWallSliding && jump && jumpCounter < 1 || doubleJump && jump)
			{
				PlayerAnim.SetTrigger("Jump");

				jumpCounter++;
				Invoke("jump", 0.1f);

				int n = Random.Range(0, 2);
				this.GetComponent<AudioSource>().PlayOneShot(jumpSFX[n]);

				Invoke("swapColors", colorDelay);

				//if (!doubleJump)
				//	doubleJump = true;
				//else

				if(doubleJump)
					doubleJump = false;
			}
            else
            {
				if(!m_Grounded)
					PlayerAnim.SetTrigger("fall");
			}

			//Wall Jump
			if (m_FacingRight)
			{
				wallCheckHit = Physics2D.Raycast(transform.position, new Vector2(wallDistance, 0f), wallDistance, m_WhatIsGround);
				Debug.DrawRay(transform.position, new Vector2(wallDistance, 0), Color.red);
			}
			else
			{
				wallCheckHit = Physics2D.Raycast(transform.position, new Vector2(-wallDistance, 0f), wallDistance, m_WhatIsGround);
				Debug.DrawRay(transform.position, new Vector2(-wallDistance, 0), Color.red);
			}

			if (wallCheckHit && !m_Grounded && Input.GetAxis("Horizontal") != 0)
			{
				isWallSliding = true;
				jumpTime = Time.time + wallJumpTime;
			}
			else if (jumpTime < Time.time)
			{
				isWallSliding = false;
			}

			if (isWallSliding)
			{
				if (m_Rigidbody2D.gravityScale < 0)
					m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, Mathf.Clamp(-m_Rigidbody2D.velocity.y, -wallSlideSpeed, float.MaxValue));
				else
					m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, Mathf.Clamp(m_Rigidbody2D.velocity.y, wallSlideSpeed, float.MaxValue));
			}
		}
	}

	void swapColors()
	{
		if (whiteStuff.activeSelf)
		{
			whiteStuff.SetActive(false);
			blackStuff.SetActive(true);
		}
		else
		{
			whiteStuff.SetActive(true);
			blackStuff.SetActive(false);
		}
	}

	public void stopVelocity()
	{
		PlayerAnim.SetBool("isWalking", false);
		m_Rigidbody2D.velocity = Vector3.zero;
	}

	public void endLevel()
	{
		PlayerAnim.SetTrigger("Fade");
		Invoke("transition", 1.5f);
	}

	void transition()
	{
		GameObject.Find("UI").GetComponent<Animator>().SetTrigger("endLevel");
		Invoke("toLevelMenu", 1);
	}

	void toLevelMenu()
    {
		SceneManager.LoadScene("Level Menu");
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void gravFlip()
    {
		m_Rigidbody2D.gravityScale = -m_Rigidbody2D.gravityScale;
		Vector3 theScale = transform.localScale;
		theScale.y *= -1;
		transform.localScale = theScale;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.layer == 3)
		{
			jumpCounter = 0;

		}
	}

}
