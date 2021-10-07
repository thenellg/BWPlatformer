using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	[Header("General")]
	public CharacterController2D controller;

	public float runSpeed = 40f;

	private float horizontalMove = 0f;
	public bool jump = false;
	public bool dash = false;
	public bool crouch = false;
	public bool hold = false;
	public bool downwardDash = false;

	public Color colorA = new Color32(230, 230, 230, 255);
	public Color colorB = new Color32(25, 25, 25, 255);

	public bool hasKey = false;
	[SerializeField] private bool areDead = false;
	public SkateboardController skateboarding;
	public bool isSkateboarding = false;

	[Header("Respawn")]
	public Vector3 spawnPoint;
	public int deathCount = 0;
	public bool canMove = true;

	public key _key;
	public GameObject items;
	private Transform[] objects;
	public CinemachineVirtualCamera startCam;
	public CinemachineBrain camBrain;

	[Header("UI")]
	public Animator UIAnimation;
	public Image reset;
	//public SpriteRenderer background;
	public SpriteRenderer backgroundA;
	public SpriteRenderer backgroundB;

	public TextMeshProUGUI visDeathCounter;
	public string levelPlayerPref;
	public GameObject pauseMenu;


	public Transform[] breakables;
	public Transform[] moveables;
	public Transform[] hanging;
	public SpriteRenderer[] rips;

	//[Header("Audio")]

	private void Awake()
    {
		//Setting item refreshes and spawn point
		pauseMenu.SetActive(false);

		objects = items.GetComponentsInChildren<Transform>();
		spawnPoint = this.transform.position;

		if (this.GetComponent<SkateboardController>())
			this.GetComponent<SkateboardController>().enabled = false;

	}

	private void Start()
    {
		Invoke("setBackground", 0.1f);
	}

	void setBackground()
    {
		backgroundA.color = colorA;
		backgroundB.color = colorB;
	}

    void Update()
	{
		// This will be for an eventual pause menu we still need to build
		if (Input.GetKeyDown("escape"))
        {
			//SceneManager.LoadScene("Level Menu");
			if (pauseMenu.activeSelf == true)
			{
				pauseMenu.SetActive(false);
				Time.timeScale = 1f;
			}
			else
			{
				pauseMenu.SetActive(true);
				Time.timeScale = 0f;
			}
		
		}

		//Setting out death counter
		visDeathCounter.text = deathCount.ToString();

		if (canMove)
		{
			//movement, but it's handled in CharacterController2D.cs
			horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

			if(Input.GetAxisRaw("Vertical") < 0)
            {
				crouch = true;
			}

			if (Input.GetButtonDown("Jump"))
			{
				jump = true;
			}
			else if (Input.GetButtonDown("Dash"))
            {
				dash = true;
            }

			if (Input.GetButtonDown("Hold"))
            {
				hold = true;
            }
			else if (Input.GetButtonUp("Hold"))
            {
				hold = false;
            }
		}

		//Adjusting objects to colors that don't auto adjust
		if (this.GetComponent<colorSwap>().whiteStuff.activeSelf)
		{
			visDeathCounter.color = colorB;
			reset.color = colorA;
			//background.color = colorA;

			foreach (SpriteRenderer rip in rips)
				rip.color = colorA;
		}
		else
		{
			visDeathCounter.color = colorA;
			reset.color = colorB;
			//background.color = colorB;
			foreach (SpriteRenderer rip in rips)
				rip.color = colorB;
		}
	}

	void FixedUpdate ()
	{
		if (isSkateboarding)
			skateboarding.Move(horizontalMove, jump, dash, crouch);
		else
			controller.Move(horizontalMove * Time.fixedDeltaTime, jump, dash, crouch, hold);

		jump = false;
		dash = false;
		crouch = false;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
		//Running all of player collision from this script. These are all generally pretty self-explanatory based off of their tags
		//It might not be a bad idea to redo this in the future but for now, this works.

		if (collision.tag == "Death")
		{
			if (!areDead)
			{
				areDead = true;
				canMove = false;
				UIAnimation.SetTrigger("Died");
				this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				Invoke("onDeath", 0.4f);
				//onDeath();
			}
		}

		else if (collision.tag == "camSwap")
        {
			Debug.Log("collision");
			collision.gameObject.GetComponent<cameraSwitch>().camSwap();
        }

		else if (collision.tag == "Key")
        {
			hasKey = true;
			collision.gameObject.transform.parent = transform;
			collision.gameObject.GetComponent<key>().following = true;
			collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;

		}

		else if (collision.tag == "DoubleJump")
        {
			controller.doubleJump = true;
			collision.gameObject.SetActive(false);
        }

		else if (collision.tag == "Checkpoint")
        {
			//change checkpoint image (probably going to be an animation)
			if (collision.GetComponent<checkpoint>().checkpointActive == false)
			{
				spawnPoint = collision.gameObject.transform.position;
				collision.GetComponent<checkpoint>().setCheckpoint();
				startCam = collision.GetComponent<checkpoint>().vcam;
			}

        }

		else if (collision.tag == "Spring")
		{
			controller.m_Rigidbody2D.AddForce(new Vector2(0f, 5f), ForceMode2D.Impulse);
			controller.canDash = true;
		}

		else if (collision.tag == "MovingPlatform")
		{
			this.transform.parent = collision.gameObject.transform;
		}

		else if (collision.tag == "Skateboard")
		{
			Debug.Log("landed on skateboard");
			collision.gameObject.transform.parent = this.transform;
			collision.transform.localPosition = new Vector2(-1.00291f, -8.157125f);
			collision.gameObject.layer = 7;
			Destroy(collision.gameObject.GetComponent<Rigidbody2D>());


			SkateboardController boardController = this.GetComponent<SkateboardController>();
			CharacterController2D playerController = this.GetComponent<CharacterController2D>();

			boardController.enabled = true;
			boardController.m_SkateboardTrigger = collision.GetComponent<SkateboardTrigger>();
			if (boardController.m_FacingRight != playerController.m_FacingRight)
					boardController.Flip();


			playerController.enabled = false;

			skateboarding.m_GroundCheck = collision.GetComponentInChildren<Transform>();
			isSkateboarding = true;
		}
	}

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Breakable")
        {
			if (controller._dashing)
				collision.gameObject.SetActive(false);
        }

		if (collision.gameObject.tag == "MovingPlatform")
        {
			this.transform.parent = collision.transform.parent;
        }

		if (collision.gameObject.tag == "Door")
		{
			if (!hasKey)
				collision.gameObject.GetComponent<Door>().locked();
			else
			{
				this.GetComponentInChildren<key>().speed = 8;
				controller.stopVelocity();
				this.GetComponentInChildren<key>().followSpot = collision.transform;
			}
		}
	}

    private void OnCollisionExit2D(Collision2D collision)
    {
		if (collision.gameObject.tag == "MovingPlatform")
		{
			this.transform.parent = null;
		}

		else if (collision.gameObject.tag == "Skateboard")
        {
			resetBoard(collision.gameObject);
        }
	}


	private void resetBoard(GameObject board)
    {
		foreach (BoxCollider2D collider in board.GetComponents<BoxCollider2D>())
			collider.enabled = true;
	}

    public void setDouble()
    {
		controller.doubleJump = true;
    }

    public void onDeath()
    {
			//death has become overly complicated but the short version is this. Play the sound, add to the count, reset
			controller.deadSFX();
			deathCount += 1;
			Invoke("resetLevel", 0f);
	}

	public void itemReset()
    {
		//Reseting the items. This is run every death and every time the player switches cameras
		foreach (Transform item in objects)
			item.gameObject.SetActive(true);
	}

	private void resetLevel()
    {
		//Resets gravity if needed
		if (this.GetComponent<Rigidbody2D>().gravityScale < 0)
		{
			controller.gravFlip();
			this.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
		}

		//Resets key if needed
		if (this.GetComponentInChildren<key>())
		{
			this.GetComponentInChildren<key>().resetKey();
			hasKey = false;
		}

		//if layer switch exists, reset it
		GameObject temp;
        if (GameObject.FindGameObjectWithTag("LayerSwitch"))
        {
			temp = GameObject.FindGameObjectWithTag("LayerSwitch");
			if (temp.GetComponent<layerSwitch>())
				temp.GetComponent<layerSwitch>().resetLayers();
			else
				temp.GetComponent<directionalLayerSwap>().resetLayers();
		}
		if (this.GetComponent<colorSwap>().onBack)
			this.GetComponent<colorSwap>().swapLayers();

		//Makes sure that white is set to active
		this.GetComponent<colorSwap>().whiteStuff.SetActive(true);
		this.GetComponent<colorSwap>().whiteStuff.SetActive(false);

		//Resets all breakable objects
		foreach (Transform platform in breakables)
			platform.gameObject.SetActive(true);

		//resets hanging boxes
		downwardDash = false;
		foreach (Transform box in hanging)
		{
			box.GetComponent<pushableObject>().moveBack();
			box.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
			box.GetComponent<pushableObject>().frozen = true;
		}

		foreach (Transform box in moveables)
			box.GetComponent<pushableObject>().moveBack();

		itemReset();

		//Move the camera back
		camBrain.m_DefaultBlend.m_Time = 0.05f;
		camBrain.ActiveVirtualCamera.Priority = 0;
		startCam.Priority = 1;
		camBrain.m_DefaultBlend.m_Time = 1f;

		Invoke("setCharacter", 0.1f);
		
		//Move character to spawn point
		this.transform.position = spawnPoint;
		areDead = false;
	}

	void setCharacter()
    {
		//Reenable movement
		canMove = true;
	}

}
