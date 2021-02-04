using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class OpeningVid : MonoBehaviour
{
    public VideoClip clip;
    public int sceneIndex;

    private void Start()
    {
        float time = (float)clip.length;
        Invoke("changeScene", time + 3);
    }

    void changeScene()
    {
        SceneManager.LoadScene(1);
    }

}
