using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator doorAnim;
    public bool isLocked = true;
    public AudioClip doorOpenSFX;

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
        this.GetComponent<AudioSource>().PlayOneShot(doorOpenSFX);
    }
}
