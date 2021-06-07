using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator doorAnim;
    public bool isLocked = true;

    void Start()
    {
        doorAnim = this.GetComponent<Animator>();
        doorAnim.SetBool("unlocked", false);
    }

    public void locked()
    {
        doorAnim.SetTrigger("locked");
    }

    public void unlocked()
    {
        doorAnim.SetTrigger("unlocked");
        isLocked = false;
    }
}
