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

    [SerializeField] private Vector3 originalPosition;
    [SerializeField] private float originalZoom;

    public Vector3 newPosition;
    public float newZoom;
    public float zoomSpeed = 0.05f;
    public bool zoomedOut = false;
    //float speed = 5;

    public void Start()
    {
        originalPosition = vcam.gameObject.transform.position;
        originalZoom = vcam.m_Lens.OrthographicSize;

        anim = this.GetComponent<Animator>();
    }

    public void Update()
    {
        if (zoomedOut)
        {
            if (vcam.m_Lens.OrthographicSize <= newZoom)
                vcam.m_Lens.OrthographicSize += zoomSpeed;
        }
        else
        {
            if (vcam.m_Lens.OrthographicSize >= originalZoom)
                vcam.m_Lens.OrthographicSize -= zoomSpeed;
        }
    }

    public void setCheckpoint()
    {
        //something something animation
        anim.SetTrigger("change");
        checkpointActive = true;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            zoomedOut = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        zoomedOut = false;
    }

}
