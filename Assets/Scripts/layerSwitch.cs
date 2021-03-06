﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class layerSwitch : MonoBehaviour
{
    [Header("Main Level")]
    public GameObject mainLevel;
    public GameObject mainLevelWhite;
    public GameObject mainLevelBlack;
    public GameObject mainLevelItems;

    [Header("Back Level")]
    public GameObject backLevel;
    public GameObject backLevelWhite;
    public GameObject backLevelBlack;
    public GameObject backLevelItems;

    private CharacterController2D controller;
    [SerializeField] private bool onMainLevel = true;
    [SerializeField] private bool changeable = false;
    [SerializeField] private Color colorReplace = Color.white;

    public float changeTimer = 1f;

    private void Start()
    {
        resetLayers();
        controller = FindObjectOfType<CharacterController2D>();
    }

    private void Update()
    {
        if (changeable)
        {
            //adjust to hold the button down
            if (Input.GetButton("Interact"))
            {
                if (changeTimer <= 0)
                {
                    changeLevels();
                    changeTimer = 3f;
                }
                else
                    changeTimer -= Time.deltaTime;
            }
            if (Input.GetButtonUp("Interact"))
            {
                changeTimer = 1f;
            }
        }
    }

    public void resetLayers()
    {
        setActive(mainLevel);
        setActive(mainLevelItems);

        setUnactive(backLevel);
        setActive(backLevelItems);
    }

    void setActive(GameObject setting)
    {
        BoxCollider2D[] platforms = setting.GetComponentsInChildren<BoxCollider2D>();
        SpriteRenderer[] renders = setting.GetComponentsInChildren<SpriteRenderer>();
        EdgeCollider2D[] edges = setting.GetComponentsInChildren<EdgeCollider2D>();

        colorReplace.a = 1;

        foreach (SpriteRenderer render in renders)
        {
            render.color = colorReplace;
        }

        foreach (BoxCollider2D collider in platforms)
        {
            collider.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }

        foreach (EdgeCollider2D edge in edges)
        {
            edge.gameObject.GetComponent<EdgeCollider2D>().enabled = true;
        }
    }

    void setUnactive(GameObject remove)
    {
        BoxCollider2D[] platforms = remove.GetComponentsInChildren<BoxCollider2D>();
        SpriteRenderer[] renders = remove.GetComponentsInChildren<SpriteRenderer>();
        EdgeCollider2D[] edges = remove.GetComponentsInChildren<EdgeCollider2D>();

        colorReplace.a = 0.15f;

        foreach (SpriteRenderer render in renders)
        {
            render.color = colorReplace;
        }

        foreach (BoxCollider2D collider in platforms)
        {
            collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }

        foreach (EdgeCollider2D edge in edges)
        {
            edge.gameObject.GetComponent<EdgeCollider2D>().enabled = false;
        }
    }

    private void changeLevels()
    {
        if (onMainLevel)
        {
            setActive(mainLevel);
            setActive(mainLevelItems);

            setUnactive(backLevel);
            setActive(backLevelItems);

            controller.whiteStuff = mainLevelWhite;
            controller.blackStuff = mainLevelBlack;

            onMainLevel = false;
        }
        else
        {
            setActive(backLevel);
            setActive(backLevelItems);

            setUnactive(mainLevel);
            setActive(mainLevelItems);

            controller.whiteStuff = backLevelWhite;
            controller.blackStuff = backLevelBlack;

            onMainLevel = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //show prompt
        if (collision.tag == "Player")
            changeable = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            changeable = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //hide prompt
        if (collision.tag == "Player")
            changeable = false;
    }
}
