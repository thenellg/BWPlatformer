using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class checkpoint : MonoBehaviour
{
    //public Animator checkpointAnim;
    public bool checkpointActive = false;
    public CinemachineVirtualCamera vcam;
    private Animator anim;

    public void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    public void setCheckpoint()
    {
        //something something animation
        anim.SetTrigger("change");
        checkpointActive = true;
    }
}
