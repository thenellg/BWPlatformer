using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour
{
    private Vector3 originSpot;
    private Transform parent;
    public float speed;

    public CharacterController2D player;
    public bool following;
    public Transform followSpot;

    private Animator keyAnim;

    private void Start()
    {
        keyAnim = this.gameObject.GetComponent<Animator>();
        originSpot = this.transform.position;
        parent = this.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if (following && Vector2.Distance(transform.position, followSpot.position) > 0)
            transform.position = Vector2.MoveTowards(transform.position, followSpot.position, speed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (transform.position == GameObject.FindGameObjectWithTag("Door").transform.position)
        {
            keyAnim.SetTrigger("Fade");
            GameObject.FindGameObjectWithTag("Door").GetComponent<Door>().unlocked();
            Invoke("boom", 1.35f);
        }
    }

    void boom()
    {
        player.endLevel();
        this.gameObject.SetActive(false);
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
