using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkateboardTrigger : MonoBehaviour
{
	public Vector3 originalPos;
	float angle = 0.5f;
	SkateboardController m_SkateboardController;
	public Vector3 normVec = Vector3.zero;
	public bool m_isSkateboarding = false;

	private void Update()
    {
		if (m_isSkateboarding)
		{
			//if grounded
			Quaternion angle = new Quaternion();
			angle.eulerAngles = normVec;
			transform.localRotation = angle;

			//if not grounded, transform.localRotation = Quaternion.Euler(Vector3.zero)
		}
	}

	public void rotateFlip()
    {
		normVec *= -1;
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
		if (collision.gameObject.tag == "MovingPlatform")
		{
			m_SkateboardController.movingPlatform(collision.transform);
		}

		if (collision.gameObject.tag == "Balloon")
		{
			m_SkateboardController.balloon(collision.gameObject);
		}
	}

    private void OnCollisionStay2D(Collision2D collision)
    {
		if (collision.gameObject.layer == 3 && collision.gameObject.tag != "Wall")
		{
			normVec = m_SkateboardController.rotate(collision.transform.rotation.eulerAngles);
			Debug.Log(normVec);
		}
		else if (collision.gameObject.layer == 3 && collision.gameObject.tag == "Wall")
		{
			normVec = Vector3.zero;
		}
	}
}
