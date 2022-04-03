using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fanPush : MonoBehaviour
{
    public Transform fanOrigin;
    public float fanIntensity;
    public bool horizontal = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 fanDirection = Vector2.zero;
        if (horizontal)
        {
            fanDirection.x = collision.transform.position.x - fanOrigin.position.x;
        }
        else
        {
            fanDirection.y = collision.transform.position.y - fanOrigin.position.y;
        }

        fanDirection = fanDirection.normalized * fanIntensity;

        if (collision.tag == "Player")
            collision.GetComponent<CharacterController2D>().fanSet(fanDirection);
        else if (collision.tag == "Box")
            collision.GetComponent<pushableObject>().fanSet(fanDirection);

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<CharacterController2D>().fanDeset();
        }
        else if (collision.tag == "Box")
        {
            collision.GetComponent<pushableObject>().fanDeset();
        }
    }
}
