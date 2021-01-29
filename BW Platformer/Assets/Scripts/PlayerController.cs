using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[Header("General")]
	public CharacterController2D controller;

	public float runSpeed = 40f;

	private float horizontalMove = 0f;
	private bool jump = false;
	private bool crouch = false;

	public Vector3 spawnPoint;
	public int deathCount;

    private void Awake()
    {
		spawnPoint = this.transform.position;
    }

    void Update()
	{

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
		}

        if (Input.GetKeyDown(KeyCode.Q))
        {
			controller.gravFlip();

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
			onDeath();
    }


    public void onDeath()
    {
		Debug.Log("DED");

		if (this.GetComponent<Rigidbody2D>().gravityScale < 0)
        {
			controller.gravFlip();
			this.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
		}

		deathCount += 1;
		this.transform.position = spawnPoint;
    }


}
