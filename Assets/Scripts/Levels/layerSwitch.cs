using System.Collections;
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

    public void resetLayers()
    {
        setActive(mainLevel);
        setActive(mainLevelItems);
        //mainLevel.transform.localScale = new Vector3(1f, 1f, 1f);

        setUnactive(backLevel);
        setUnactive(backLevelItems);
        //backLevel.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
    }

    void setActive(GameObject setting)
    {
        BoxCollider2D[] platforms = setting.GetComponentsInChildren<BoxCollider2D>();
        SpriteRenderer[] renders = setting.GetComponentsInChildren<SpriteRenderer>();
        EdgeCollider2D[] edges = setting.GetComponentsInChildren<EdgeCollider2D>();
        Rigidbody2D[] _rbs = setting.GetComponentsInChildren<Rigidbody2D>();
        colorReplace.a = 1;

        foreach (SpriteRenderer render in renders)
        {
            render.color = colorReplace;
        }

        foreach (BoxCollider2D collider in platforms)
        {
            if (collider.gameObject.tag == "Spring")
            {
                BoxCollider2D[] springs = collider.gameObject.GetComponents<BoxCollider2D>();
                foreach (BoxCollider2D spring in springs)
                    spring.enabled = true;
            }
            else
            {
                collider.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        foreach (EdgeCollider2D edge in edges)
        {
            edge.gameObject.GetComponent<EdgeCollider2D>().enabled = true;
        }

        foreach (Rigidbody2D rb in _rbs)
        {
            if (rb.gameObject.GetComponent<pushableObject>() && rb.gameObject.GetComponent<pushableObject>().hanging && rb.gameObject.GetComponent<pushableObject>().frozen)
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            else
                rb.constraints = RigidbodyConstraints2D.None;
        }

        //setting.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    void setUnactive(GameObject remove)
    {
        BoxCollider2D[] platforms = remove.GetComponentsInChildren<BoxCollider2D>();
        SpriteRenderer[] renders = remove.GetComponentsInChildren<SpriteRenderer>();
        EdgeCollider2D[] edges = remove.GetComponentsInChildren<EdgeCollider2D>();
        Rigidbody2D[] _rbs = remove.GetComponentsInChildren<Rigidbody2D>();

        colorReplace.a = 0.15f;

        foreach (SpriteRenderer render in renders)
        {
            render.color = colorReplace;
        }

        foreach (BoxCollider2D collider in platforms)
        {
            if (collider.gameObject.tag == "Spring")
            {
                BoxCollider2D[] springs = collider.gameObject.GetComponents<BoxCollider2D>();
                foreach (BoxCollider2D spring in springs)
                    spring.enabled = false;
            }
            else
            {
                collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        }

        foreach (EdgeCollider2D edge in edges)
        {
            edge.gameObject.GetComponent<EdgeCollider2D>().enabled = false;
        }

        foreach (Rigidbody2D rb in _rbs)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        //remove.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
    }

    private void changeLevels(GameObject player)
    {
        if (!onMainLevel)
        {
            setActive(mainLevel);
            setActive(mainLevelItems);

            setUnactive(backLevel);
            setUnactive(backLevelItems);

            //this.GetComponent<colorSwap>().whiteStuff = mainLevelWhite;
            //this.GetComponent<colorSwap>().blackStuff = mainLevelBlack;

            onMainLevel = true;
        }
        else
        {
            setActive(backLevel);
            setActive(backLevelItems);

            setUnactive(mainLevel);
            setUnactive(mainLevelItems);

            // this.GetComponent<colorSwap>().whiteStuff = backLevelWhite;
            // this.GetComponent<colorSwap>().blackStuff = backLevelBlack;

            onMainLevel = false;
        }
        player.GetComponent<colorSwap>().swapLayers();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //show prompt

        if (collision.tag == "Player")
        {
            changeLevels(collision.gameObject);
            collision.transform.parent = null;
            collision.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            collision.GetComponent<Rigidbody2D>().freezeRotation = true;
        }
        //    changeable = true;
    }

}
