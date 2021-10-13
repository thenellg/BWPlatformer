using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class SkateboardController : MonoBehaviour
{
	[Header("General")]
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	//[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	public Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching
	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	public bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	public Rigidbody2D m_Rigidbody2D;
	public bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 velocity = Vector3.zero;

	[Header("Specialized Jump")]
	private PlayerController m_PlayerController;
	public int coyoteTimer = 3;
	public bool doubleJump = false;
	public AudioClip[] jumpSFX;
	public AudioClip deathSFX;
	public float dashSpeed = 1.2f;
	public bool _dashing = false;
	public bool canDash = true;

	private Animator PlayerAnim;
	bool canMove = true;
	[SerializeField] private Vector2 dashVector;
	float shakeTimer = 0;
	bool crouching = false;
	private float moveCheck;
	int jumpCounter = 0;

	[Header("Gravity")]
	public bool forcedGrav = false;
	public string gravDirection = "up";
	public float tempGravScale;


	float speed = 2f;
	public float angle = 0.5f;
	Vector3 normalVec;
	public SkateboardTrigger m_SkateboardTrigger;
	//Dash speed= 2.5-3? Needs testing

	private void Awake()
	{
		//Set private values and disable stuff
		PlayerAnim = this.GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		m_PlayerController = GetComponent<PlayerController>();

		tempGravScale = m_Rigidbody2D.gravityScale;
		this.GetComponent<colorSwap>().blackStuff.SetActive(false);
	}

	public void resetColliders()
    {

    }

	private void Update()
	{
		//Running coyote timer and using canMove
		if (coyoteTimer > 0 && m_Grounded == false)
		{
			coyoteTimer--;
		}
		else if (coyoteTimer <= 0 && m_Grounded == true)
		{
			coyoteTimer = 30;
		}

		if (shakeTimer > 0)
		{
			shakeTimer -= Time.deltaTime;
		}
		else
		{
			CinemachineVirtualCamera vcam = Camera.main.gameObject.GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
			CinemachineBasicMultiChannelPerlin shake = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
			shake.m_AmplitudeGain = 0f;
		}

		canMove = m_PlayerController.canMove;

		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		if (horizontal == 0 && vertical == 0)
		{
			if (m_FacingRight)
				dashVector = new Vector2(1.25f, 0).normalized;
			else
				dashVector = new Vector2(-1.25f, 0).normalized;
		}
		else
		{
			dashVector = new Vector2(horizontal, vertical).normalized;

		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl && GetComponent<PlayerController>().isSkateboarding)
		{
			m_SkateboardTrigger.m_isSkateboarding = GetComponent<PlayerController>().isSkateboarding;

			float move = speed;
			//transform.up = normalVec;

			if (!m_FacingRight)
				move *= -1;

			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);

			if (gravDirection == "right")
			{
				targetVelocity = new Vector2(m_Rigidbody2D.velocity.x, move * 10f);
			}
			else if (gravDirection == "left")
			{
				targetVelocity = new Vector2(m_Rigidbody2D.velocity.x, -move * 10f);
			}

			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);     // And then smoothing it out and applying it to the 
		}
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
				canDash = true;
			}
		}

		PlayerAnim.SetBool("isGrounded", m_Grounded);

	}

	void playerJump()
	{

		//Adjusting for reverse gravity
		/*
		if (m_Rigidbody2D.gravityScale < 0)
			jumpForce = -m_JumpForce;
		else
			jumpForce = m_JumpForce;
		*/


		transform.up = Vector3.up;
		m_Grounded = false;

		if (gravDirection == "up")
			m_Rigidbody2D.AddForce(new Vector2(0, m_JumpForce));
		else if (gravDirection == "down")
			m_Rigidbody2D.AddForce(new Vector2(0, -m_JumpForce));
		else if (gravDirection == "left")
			m_Rigidbody2D.AddForce(new Vector2(m_JumpForce, 0));
		else if (gravDirection == "right")
			m_Rigidbody2D.AddForce(new Vector2(-m_JumpForce, 0));

	}

	/*
	public void facingRightCheck()
    {
		float test = m_Rigidbody2D.velocity.x;
//		Debug.Log(test < 0 && m_FacingRight || test > 0 && !m_FacingRight);
		if (test < 0 && m_FacingRight || test > 0 && !m_FacingRight)
		{
			Invoke("forceFlip", 0.4f);
		}
	}
	*/

	public void forceFlip()
	{
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		m_SkateboardTrigger.rotateFlip();
	}

	public void dashing()
	{
		//Exit board
		this.GetComponent<CharacterController2D>().enabled = true;
		m_SkateboardTrigger.m_isSkateboarding = false;
		CharacterController2D m_CharacterController2D = this.GetComponent<CharacterController2D>();
		GameObject board = null;

		foreach (Transform child in GetComponentsInChildren<Transform>())
        {
			if (child.tag == "Skateboard")
            {
				board = child.gameObject;
				child.transform.parent = this.transform.parent;
				Debug.Log("stepping off skateboard");
			}
		}
		//collision.gameObject.transform.parent = this.transform;
		//collision.transform.localPosition = new Vector2(-1.00291f, -8.157125f);

		if (board != null)
		{
			//Turn off skateboard's trigger and turn it back on after stepping off board
			foreach (BoxCollider2D collider in board.GetComponents<BoxCollider2D>())
			{
				if (collider.isTrigger)
					collider.enabled = false;
			}
			board.AddComponent<Rigidbody2D>();
			board.layer = 7;
			board.transform.rotation = Quaternion.Euler(Vector3.zero);
			this.transform.rotation = Quaternion.Euler(Vector3.zero);

			this.GetComponent<PlayerController>().isSkateboarding = false;
			m_CharacterController2D.dashVector = dashVector;
			m_CharacterController2D.dashing();

			this.GetComponent<SkateboardController>().enabled = false;
			Invoke("resetBoardColliders", 0.5f);
		}

		//Set off script in board to reset colliders
		//Invoke("resetDash", 0.4f);
	}
	
	void resetBoardColliders()
    {
		m_SkateboardTrigger.resetColliders();
		m_SkateboardTrigger = null;

	}

	public void deadSFX()
	{
		this.GetComponent<AudioSource>().PlayOneShot(deathSFX);
	}

	public void Move(float move, bool jump, bool dash, bool crouch)
	{
		moveCheck = move;

		if (canMove)
		{
			if (m_Grounded && crouch)
			{
				if (!crouching)
				{
					PlayerAnim.SetTrigger("Crouch");
					crouching = true;
				}


				PlayerAnim.SetBool("crouching", true);
				PlayerAnim.SetBool("isWalking", false);
			}
			else if (m_Grounded && !crouch)
			{
				crouching = false;
				PlayerAnim.SetBool("crouching", false);
				PlayerAnim.SetBool("isWalking", false);
			}
			else if (!m_Grounded)
			{
				PlayerAnim.SetTrigger("fall");
				PlayerAnim.SetBool("isWalking", false);
				PlayerAnim.SetBool("crouching", false);
			}
			else
			{
				PlayerAnim.SetBool("isWalking", false);
				PlayerAnim.SetBool("crouching", false);
			}

			//only control the player if grounded or airControl is turned on
			if (m_Grounded || m_AirControl)
			{
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
			if (coyoteTimer > 0 && jump && jumpCounter < 1)// || doubleJump && jump)
			{
				PlayerAnim.SetTrigger("Jump");

				coyoteTimer = 0;
				jumpCounter++;
				playerJump();

				int n = Random.Range(0, 2);
				this.GetComponent<AudioSource>().PlayOneShot(jumpSFX[n]);

				Invoke("swapColors", this.GetComponent<colorSwap>().colorDelay);

				//if (!doubleJump)
				//	doubleJump = true;
				//else

				if (doubleJump)
					doubleJump = false;
			}
			else
			{
				if (!m_Grounded)
				{

					if (Input.GetAxis("Vertical") < 0 && m_Rigidbody2D.gravityScale > 0)
					{
						m_Rigidbody2D.AddForce(new Vector2(0, -3));
					}
					else if (Input.GetAxis("Vertical") < 0 && m_Rigidbody2D.gravityScale < 0)
					{
						m_Rigidbody2D.AddForce(new Vector2(0, 3));
					}
				}
                else
                {
					jumpCounter = 0;
                }
			}
			
			if (m_Grounded && !_dashing)
				canDash = true;

			if (dash && canDash)
			{
				dashing();
				canDash = false;
			}
		}
	}

	public void swapColors()
	{
		this.GetComponent<colorSwap>().swapColors();
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

	public void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		RaycastHit2D hit = Physics2D.Raycast(m_GroundCheck.position, Vector2.down);
		transform.up = hit.normal;

		Vector3 theScale = transform.localScale;
		if (m_FacingRight)
		{
			if (theScale.x < 0)
				theScale.x *= -1;
			transform.localScale = theScale;
		}
		else if (!m_FacingRight)
		{
			if (theScale.x > 0)
				theScale.x *= -1;
			transform.localScale = theScale;
		}

		m_SkateboardTrigger.rotateFlip();
	}

	public void gravFlip()
	{
		m_Rigidbody2D.gravityScale = -m_Rigidbody2D.gravityScale;
		Vector3 theScale = transform.localScale;
		theScale.y *= -1;
		transform.localScale = theScale;
	}

	public Vector3 rotate(Vector3 ground){
		normalVec = ground;
		if (!m_FacingRight)
			normalVec *= -1;

		return normalVec;
	}

	public void movingPlatform(Transform platform)
    {
		this.transform.parent = platform.parent;
	}

	public void balloon(GameObject _balloon)
    {
		playerJump();
		_balloon.SetActive(false);
	}

}
