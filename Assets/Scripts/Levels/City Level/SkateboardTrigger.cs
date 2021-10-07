using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkateboardTrigger : MonoBehaviour
{
	public Vector3 originalPos;
	float angle = 0.5f;
	SkateboardController m_SkateboardController;
	public Vector3 normVec;

	private void Update()
    {

    }

    private void Start()
    {
        originalPos = transform.position;
		m_SkateboardController = GameObject.FindGameObjectWithTag("Player").GetComponent<SkateboardController>();
	}

	public void resetColliders()
    {
		foreach (BoxCollider2D collider in gameObject.GetComponents<BoxCollider2D>())
		{
			if (collider.isTrigger)
				collider.enabled = true;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.layer == 3 && collision.gameObject.tag != "Wall")
		{
			//???
		}

		if (collision.gameObject.tag == "MovingPlatform")
		{
			m_SkateboardController.movingPlatform(collision.transform);
		}

		if (collision.gameObject.tag == "Balloon")
		{
			m_SkateboardController.balloon(collision.gameObject);
		}
	}
}
