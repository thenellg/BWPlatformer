using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSettings : MonoBehaviour
{
    public Material colorChange;

    public colorScheme[] colorOptions;
    public int chosenColor;

    public bool downSmashUnlock = false;

    // Start is called before the first frame update
    void Start()
    {
        colorSet();
    }

    private void FixedUpdate()
    {
        if (colorOptions[chosenColor] != null)
        {
            colorSet();
        }
    }

    void colorSet()
    {
        if (colorOptions[chosenColor] != null)
        {
            colorChange.SetColor("Color1", colorOptions[chosenColor].colorA);
            colorChange.SetColor("Color2", colorOptions[chosenColor].colorB);

            GameObject.Find("Player").GetComponent<PlayerController>().colorA = new Color(colorOptions[chosenColor].colorA.r, colorOptions[chosenColor].colorA.g, colorOptions[chosenColor].colorA.b);
            GameObject.Find("Player").GetComponent<PlayerController>().colorB = new Color(colorOptions[chosenColor].colorB.r, colorOptions[chosenColor].colorB.g, colorOptions[chosenColor].colorB.b);
        }
    }
}
