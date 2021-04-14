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
            colorChange.SetColor("Color_58b6362338eb49a389b0ee3dd2199e6d", colorOptions[chosenColor].colorA);
            colorChange.SetColor("Color_51d760d7e6674a1fb00f2d86a2b06abc", colorOptions[chosenColor].colorB);

            GameObject.Find("Player").GetComponent<PlayerController>().colorA = colorOptions[chosenColor].colorA;
            GameObject.Find("Player").GetComponent<PlayerController>().colorB = colorOptions[chosenColor].colorB;
        }
    }
}
