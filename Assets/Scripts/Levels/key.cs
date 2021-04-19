using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour
{
    private Vector3 originSpot;
    private Transform parent;
    public float speed = 5;

    public CharacterController2D player;
    public bool following;
    public Transform followSpot;
    bool end = true;

    private Animator keyAnim;
    bool inPosition = false;

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
        
        if (inPosition)
        {
            Color temp = this.GetComponent<SpriteRenderer>().color;
            temp.a = temp.a - 0.1f;
            this.GetComponent<SpriteRenderer>().color = temp;
        }
    }

    private void FixedUpdate()
    {
        if (transform.position == GameObject.FindGameObjectWithTag("Door").transform.position && end)
        {
            keyAnim.SetTrigger("Fade");
            inPosition = true;
            GameObject.FindGameObjectWithTag("Door").GetComponent<Door>().unlocked();
            Invoke("boom", 1.35f);
            end = false;
        }
    }

    void boom()
    {
        //player.endLevel();
        this.gameObject.SetActive(false);
    }

    public void setFollow()
    {
        following = true;
    }

    public void resetKey()
    {
        speed = 5;
        this.transform.position = originSpot;
        this.transform.parent = parent;
        following = false;
        inPosition = false;
    }
}
