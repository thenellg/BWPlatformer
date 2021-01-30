using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class cameraSwitch : MonoBehaviour
{
    public CinemachineVirtualCamera cam1;
    public CinemachineVirtualCamera cam2;
    [SerializeField] int temp;


    public void camSwap()
    {
        temp = cam1.m_Priority;
        cam1.Priority = cam2.m_Priority;
        cam2.Priority = temp;
    }
}
