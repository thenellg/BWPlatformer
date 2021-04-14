using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorSwap : MonoBehaviour
{
    public GameObject whiteStuff;
    public GameObject blackStuff;
    public float colorDelay = 0.2f;

    public GameObject whiteMoving;
    public GameObject blackMoving;

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
        }
        else
        {
            whiteStuff.SetActive(true);
            blackStuff.SetActive(false);
        }

        swapMoving(whiteMoving.transform);
        swapMoving(blackMoving.transform);
    }

    void swapMoving(Transform layer)
    {
        //Debug.Log("testA");

        foreach (Transform child in layer)
        {
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

    }
}
