using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour {

	[Header("General")]
	public CharacterController2D controller;

	public float runSpeed = 40f;

	private float horizontalMove = 0f;
	private bool jump = false;
	private bool crouch = false;

	[SerializeField] bool onWhite = true;

	public Vector3 spawnPoint;
	public int deathCount;

	public TextMeshProUGUI visDeathCounter;

	//[Header("Audio")]

	private void Awake()
    {
		spawnPoint = this.transform.position;
		deathCount = PlayerPrefs.GetInt("deathCount", 0);
    }

    void Update()
	{
		visDeathCounter.text = deathCount.ToString();

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
		}

        if (Input.GetKeyDown(KeyCode.Q))
        {
			controller.gravFlip();

		}

		if (controller.whiteStuff.activeSelf)
			visDeathCounter.color = new Color32(25, 25, 25, 255);
		else
			visDeathCounter.color = new Color32(230, 230, 230, 255);
	}

	void FixedUpdate ()
	{
		controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
		jump = false;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.tag == "Death")
			onDeath();

		if (collision.tag == "camSwap")
        {
			Debug.Log("collision");
			collision.gameObject.GetComponent<cameraSwitch>().camSwap();
        }
    }

	public void setDouble()
    {
		controller.doubleJump = true;
    }

    public void onDeath()
    {
		Debug.Log("DED");
		Invoke("resetLevel", 0f);
	}

	private void resetLevel()
    {
		if (this.GetComponent<Rigidbody2D>().gravityScale < 0)
		{
			controller.gravFlip();
			this.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
		}

		controller.whiteStuff.SetActive(true);
		controller.blackStuff.SetActive(false);

		deathCount += 1;
		this.transform.position = spawnPoint;
	}


}
