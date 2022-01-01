using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class directionalLayerSwap : MonoBehaviour
{
    public AudioClip transitionSFX;

    [Header("Directional Info")]
    [Space]
    [Header("(See directionCheck() in script for how Directional Info works)")]
    public bool up = false;
    public bool down = false;
    public bool left = false;
    public bool right = false;
    public Vector2 enterDirection;
    public Vector2 leaveDirection;

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
    //[SerializeField] private bool changeable = false;
    [SerializeField] private Color colorReplace = Color.white;

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

    private bool directionCheck()
    {
        // Don't mess with the boolean values. They're going to be adjusted at runtime anyways.
        // For each of these directions, it's what direction the player is moving where you want it to switch layers. (i.e. left means when the player is moving out of the switch to the left)
        // IMPORTANT: You cannot pick both left and right or both up and down. If you want both use the normal layer switch prefab.
        // If you have any questions message them to me and I'll do my best to answer.
        // - Griffin

        if (left)
        {
            if (leaveDirection.x < 0)
                return true;
        }

        if (right)
        {
            if (leaveDirection.x > 0)
                return true;
        }

        if (down)
        {
            if (leaveDirection.y < 0)
                return true;
        }

        if (up)
        {
            if (leaveDirection.y > 0)
                return true;
        }

        return false;
    }

    void setDirection()
    {
        if (enterDirection.y > 0)
        {
            up = true;
        }
        else
        {
            down = true;
        }

        if (enterDirection.x > 0)
        {
            right = true;
        }
        else
        {
            left = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //Get Vector
            Vector2 temp = transform.position - collision.transform.position;
            enterDirection = temp.normalized;
            setDirection();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //Get Vector
            Vector2 temp = collision.transform.position - transform.position;
            leaveDirection = temp.normalized;

            
            //if Vector equates what we want
            if (directionCheck())
            {
                changeLevels(collision.gameObject);
                collision.transform.parent = null;
                collision.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                collision.GetComponent<Rigidbody2D>().freezeRotation = true;
                collision.GetComponent<AudioSource>().PlayOneShot(transitionSFX);
            }

            up = false;
            down = false;
            left = false;
            right = false;
        }
    }
}
