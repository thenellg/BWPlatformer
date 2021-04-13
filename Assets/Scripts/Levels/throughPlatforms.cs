using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throughPlatforms : MonoBehaviour
{
    private PlatformEffector2D effector;
    float waitTime;
    private Animator PlayerAnim;

    void Start()
    {
        PlayerAnim = GameObject.Find("Player").GetComponent<Animator>();
        effector = this.GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        if(Input.GetAxis("Vertical") >= 0)
        {
            waitTime = 0.5f;
        }
        else if(Input.GetAxis("Vertical") < 0)
        {
            if(waitTime <= 0 && Input.GetButtonDown("Jump"))
            {
                PlayerAnim.SetBool("crouching", false);
                GameObject.Find("Player").GetComponent<PlayerController>().jump = false;
                effector.rotationalOffset = 180f;
                waitTime = 0.5f;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    
        if (FindObjectOfType<PlayerController>().GetComponent<PlayerController>().jump)
        {
            effector.rotationalOffset = 0f;
            waitTime = 0.5f;
        }
    }
}
