using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour
{
    private Vector3 originSpot;
    private Transform parent;
    public float speed;

    public PlayerController player;
    public bool following;
    public Transform followSpot;

    

    private void Start()
    {
        originSpot = this.transform.position;
        parent = this.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if (following && Vector2.Distance(transform.position, followSpot.position) > 0.3)
            transform.position = Vector2.MoveTowards(transform.position, followSpot.position, speed * Time.deltaTime);
    }

    public void setFollow()
    {
        following = true;
    }

    public void resetKey()
    {
        this.transform.position = originSpot;
        this.transform.parent = parent;
        following = false;
    }
}
