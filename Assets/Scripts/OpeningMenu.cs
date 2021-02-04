using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningMenu : MonoBehaviour
{
    public Animator menuAnim;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            menuAnim.SetTrigger("transition");
            Invoke("load", 2);
        }        
    }

    void load()
    {
        SceneManager.LoadScene(2);
    }
}
