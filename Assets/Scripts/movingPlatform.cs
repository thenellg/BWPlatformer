using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    public bool moving;

    public Vector3 positionA;
    public Vector3 positionB;
    public float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        Vector3 movePosition;

        if (moving)
            movePosition = positionB;
        else
            movePosition = positionA;


        if (transform.position == positionB)
            moving = false;
        else if (transform.position == positionA)
            moving = true;

        transform.position = Vector2.MoveTowards(transform.position, movePosition, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.transform.parent = this.transform;
    }
}
