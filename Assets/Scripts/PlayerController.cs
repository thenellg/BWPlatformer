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
	private bool jump = false;
	private bool crouch = false;

	public bool hasKey = false;

	public Vector3 spawnPoint;
	public int deathCount;
	public bool canMove = true;

	public key _key;
	public GameObject items;
	public CinemachineVirtualCamera startCam;
	public CinemachineBrain camBrain;

	public Animator UIAnimation;
	public Image reset;
	public TextMeshProUGUI visDeathCounter;
	private Transform[] objects;
	public string levelPlayerPref;

	[SerializeField] private bool areDead = false;
	//[Header("Audio")]

	private void Awake()
    {
		objects = items.GetComponentsInChildren<Transform>();
		spawnPoint = this.transform.position;
		deathCount = PlayerPrefs.GetInt("deathCount", 0);
    }

    void Update()
	{
        if (Input.GetKeyDown("escape"))
        {
			SceneManager.LoadScene("Level Menu");
        }

		visDeathCounter.text = deathCount.ToString();

		if (canMove)
		{
			horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

			if (Input.GetButtonDown("Jump"))
			{
				jump = true;
			}
		}

		if (controller.whiteStuff.activeSelf)
		{
			visDeathCounter.color = new Color32(25, 25, 25, 255);
			reset.color = new Color32(230, 230, 230, 255);
		}
		else
		{
			visDeathCounter.color = new Color32(230, 230, 230, 255);
			reset.color = new Color32(25, 25, 25, 255);
		}
	}

	void FixedUpdate ()
	{
		controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
		jump = false;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.tag == "Death")
		{
			if (!areDead)
			{
				areDead = true;
				canMove = false;
				UIAnimation.SetTrigger("Died");
				Invoke("onDeath", 0.4f);
			}
		}

		if (collision.tag == "camSwap")
        {
			Debug.Log("collision");
			collision.gameObject.GetComponent<cameraSwitch>().camSwap();
        }

		if (collision.tag == "Key")
        {
			hasKey = true;
			_key.following = true;
        }

		if (collision.tag == "ReverseGrav")
        {
			controller.gravFlip();
			collision.gameObject.SetActive(false);
        }

		if (collision.tag == "DoubleJump")
        {
			controller.doubleJump = true;
			collision.gameObject.SetActive(false);
        }
    
		if (collision.tag == "Door")
        {
			if (!hasKey)
				collision.gameObject.GetComponent<Door>().locked();
			else {
				PlayerPrefs.SetInt(levelPlayerPref, 1);
				canMove = false;
				transform.position = collision.transform.position;
				controller.stopVelocity();
				_key.followSpot = collision.transform;
			}
		}
	
		if (collision.tag == "Checkpoint")
        {
			//change checkpoint image (probably going to be an animation)
			if (collision.GetComponent<checkpoint>().checkpointActive == false)
			{
				spawnPoint = collision.gameObject.transform.position;
				collision.GetComponent<checkpoint>().setCheckpoint();
				startCam = collision.GetComponent<checkpoint>().vcam;
			}

        }
	}

	public void setDouble()
    {
		controller.doubleJump = true;
    }

    public void onDeath()
    {
			Debug.Log("DED");
			controller.deadSFX();
			deathCount += 1;
			Invoke("resetLevel", 0f);
	}

	public void itemReset()
    {
		foreach (Transform item in objects)
			item.gameObject.SetActive(true);
	}

	private void resetLevel()
    {
		if (this.GetComponent<Rigidbody2D>().gravityScale < 0)
		{
			controller.gravFlip();
			this.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
		}

		_key.resetKey();
		hasKey = false;

		controller.whiteStuff.SetActive(true);
		controller.blackStuff.SetActive(false);

		itemReset();

		camBrain.m_DefaultBlend.m_Time = 0.05f;
		camBrain.ActiveVirtualCamera.Priority = 0;
		startCam.Priority = 1;
		camBrain.m_DefaultBlend.m_Time = 1f;

		Invoke("setCharacter", 0.1f);

		this.transform.position = spawnPoint;
		areDead = false;
	}

	void setCharacter()
    {
		canMove = true;
		
	}

}