using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkateboardTrigger : MonoBehaviour
{
	public Vector3 originalPos;
	float angle = 0.5f;
	SkateboardController m_SkateboardController;

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


	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == 3 && collision.gameObject.tag != "Wall")
		{
			Debug.Log("ROTATE TEST");
			m_SkateboardController.rotate(collision.contacts[0].normal);
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
