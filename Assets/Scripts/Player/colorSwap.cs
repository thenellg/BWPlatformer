using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorSwap : MonoBehaviour
{
    public GameObject whiteStuff;
    public GameObject blackStuff;
    public float colorDelay = 0.2f;
    public bool onBack = false;

    public GameObject whiteMoving;
    public GameObject blackMoving;

    public Animator backgroundAnim;

    public GameObject[] backPieces = new GameObject[4];

    private void Start()
    {
        swapMoving(blackMoving.transform);
        swapMoving(backPieces[3].transform);

    }

    public void swapColors()
    {
        if (whiteStuff.activeSelf)
        {
            whiteStuff.SetActive(false);
            blackStuff.SetActive(true);

            //backPieces[0].SetActive(false);
            //backPieces[1].SetActive(true);

            backgroundAnim.speed = 3f;
            backgroundAnim.SetTrigger("hide");
            backgroundAnim.SetBool("hidden", false);
        }
        else
        {
            whiteStuff.SetActive(true);
            blackStuff.SetActive(false);

            //backPieces[0].SetActive(true);
            //backPieces[1].SetActive(false);

            backgroundAnim.speed = 3f;
            backgroundAnim.SetBool("hidden", true);
            backgroundAnim.SetTrigger("show");
        }

        swapMoving(whiteMoving.transform);
        swapMoving(blackMoving.transform);
    }

    void swapMoving(Transform layer)
    {
        //Debug.Log("testA");

        foreach (Transform child in layer)
        {
            if (child.gameObject.GetComponent<pushableObject>())
            {
                if (child.gameObject.GetComponent<SpriteRenderer>().enabled)
                {
                    //child.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    Physics2D.IgnoreCollision(child.gameObject.GetComponent<BoxCollider2D>(), this.GetComponent<CapsuleCollider2D>(), true);
                    child.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                }
                else
                {
                    Physics2D.IgnoreCollision(child.gameObject.GetComponent<BoxCollider2D>(), this.GetComponent<CapsuleCollider2D>(), false);
                    child.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                }
            }

            foreach (Transform subchild in child)
            {
                //Debug.Log("test");
                if (subchild.gameObject.activeSelf == true)
                {
                    subchild.gameObject.SetActive(false);
                }
                else
                {
                    subchild.gameObject.SetActive(true);
                }
            }
        }
    }

    public void swapLayers()
    {
        GameObject[] temp = new GameObject[4];
        temp[0] = whiteStuff;
        temp[1] = blackStuff;
        temp[2] = whiteMoving;
        temp[3] = blackMoving;

        whiteStuff = backPieces[0];
        blackStuff = backPieces[1];
        whiteMoving = backPieces[2];
        blackMoving = backPieces[3];

        for (int i = 0; i < 4; i++)
        {
            backPieces[i] = temp[i];
        }

        onBack = !onBack;
    }
}
