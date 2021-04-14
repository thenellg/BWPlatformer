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

	public Color colorA = new Color32(230, 230, 230, 255);
	public Color colorB = new Color32(25, 25, 25, 255);

	public bool hasKey = false;
	[SerializeField] private bool areDead = false;

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
	public SpriteRenderer background;
	public TextMeshProUGUI visDeathCounter;
	public string levelPlayerPref;
	public GameObject pauseMenu;


	public Transform[] breakables;
	public Transform[] moveables;

	//[Header("Audio")]

	private void Awake()
    {
		//GetComponent<SpriteRenderer>().material.SetColor("Color_58b6362338eb49a389b0ee3dd2199e6d", colorA);
		//GetComponent<SpriteRenderer>().material.SetColor("Color_51d760d7e6674a1fb00f2d86a2b06abc", colorB);

		//Setting item refreshes and spawn point
		pauseMenu.SetActive(false);

		objects = items.GetComponentsInChildren<Transform>();
		spawnPoint = this.transform.position;
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
		}

		//Adjusting objects to colors that don't auto adjust
		if (this.GetComponent<colorSwap>().whiteStuff.activeSelf)
		{
			visDeathCounter.color = colorB;
			reset.color = colorA;
			background.color = colorA;
		}
		else
		{
			visDeathCounter.color = colorA;
			reset.color = colorB;
			background.color = colorB;
		}
	}

	void FixedUpdate ()
	{
		controller.Move(horizontalMove * Time.fixedDeltaTime, jump, dash, crouch);
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
			_key.following = true;
        }

		else if (collision.tag == "DoubleJump")
        {
			controller.doubleJump = true;
			collision.gameObject.SetActive(false);
        }

		else if (collision.tag == "Door")
        {
			if (!hasKey)
				collision.gameObject.GetComponent<Door>().locked();
			else {
				_key.speed = 8;
				PlayerPrefs.SetInt(levelPlayerPref, 1);
				canMove = false;
				transform.position = collision.transform.position;
				controller.stopVelocity();
				_key.followSpot = collision.transform;
			}
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

		else if (collision.tag == "MovingPlatform")
        {
			this.transform.parent = collision.gameObject.transform;
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
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
		if (collision.gameObject.tag == "MovingPlatform")
		{
			this.transform.parent = null;
		}
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
		_key.resetKey();
		hasKey = false;

		//if layer switch exists, reset it
		if (GameObject.FindGameObjectWithTag("LayerSwitch"))
			GameObject.FindGameObjectWithTag("LayerSwitch").GetComponent<layerSwitch>().resetLayers();

		//Makes sure that white is set to active
		this.GetComponent<colorSwap>().whiteStuff.SetActive(true);
		this.GetComponent<colorSwap>().whiteStuff.SetActive(false);

		//Resets all breakable objects
		foreach (Transform platform in breakables)
			platform.gameObject.SetActive(true);

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
