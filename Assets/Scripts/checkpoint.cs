using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class checkpoint : MonoBehaviour
{
    //public Animator checkpointAnim;
    public Sprite set;
    public bool checkpointActive = false;
    public CinemachineVirtualCamera vcam;

    public void setCheckpoint()
    {
        //something something animation
        this.GetComponent<SpriteRenderer>().sprite = set;
        checkpointActive = true;
    }
}
