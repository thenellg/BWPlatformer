using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningMenu : MonoBehaviour
{
    //public Animator menuAnim;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            load();
        }        
    }

    void load()
    {
        SceneManager.LoadScene(2);
    }
}
