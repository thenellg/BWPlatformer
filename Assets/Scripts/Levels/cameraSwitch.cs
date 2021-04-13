using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class cameraSwitch : MonoBehaviour
{
    public CinemachineVirtualCamera vcam1;
    public CinemachineVirtualCamera vcam2;

    public void camSwap()
    {
        Invoke("go", 0.3f);

        if (vcam1.Priority > vcam2.Priority)
        {
            vcam1.Priority = 0;
            vcam2.Priority = 1;
        }
        else
        {
            vcam1.Priority = 1;
            vcam2.Priority = 0;
        }
    }

    private void go()
    {
        GameObject.FindObjectOfType<PlayerController>().itemReset();
    }
}
