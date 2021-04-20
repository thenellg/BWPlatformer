using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    public bool moving;

    public Vector3 positionA;
    public Vector3 positionB;
    public float speed = 1f;

    Vector3 movePosition;

    private void Start()
    {
        movePosition = positionB;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 movePosition;

        if (transform.localPosition == positionA)
        {
            movePosition = positionB;
            moving = false;
        }
        else if (transform.localPosition == positionB)
        {
            movePosition = positionA;
            moving = true;
        }

        transform.localPosition = Vector2.MoveTowards(transform.localPosition, movePosition, speed * Time.deltaTime);

        if (Input.GetKey("o"))
        {
            Debug.Log(transform.localPosition);
        }
    }

}
