using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fanPush : MonoBehaviour
{
    public Transform fanOrigin;
    public float fanIntensity;
    public bool horizontal = true;
    public bool inverted = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 fanDirection = Vector2.zero;
        if (horizontal)
            fanDirection.x = 1;
        else
            fanDirection.y = 1;
        fanDirection = fanDirection.normalized * fanIntensity;

        if (inverted)
            fanDirection = fanDirection * -1;

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
            if (horizontal)
                collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}
