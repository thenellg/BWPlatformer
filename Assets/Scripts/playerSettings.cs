using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSettings : MonoBehaviour
{
    [Header("General")]
    public Material colorChange;

    public colorScheme[] colorOptions;
    public int chosenColor;

    public int activeLevel;

    [Header("Unlocks")]
    public bool downSmashUnlock = false;
    public bool dashUnlock = true;

    [Header("Castle Level")]
    public int castleCollectibles;
    public int castleCollected = 0;
    public int castleCurrentCam;
    public bool castleBeaten = false;

    [Header("Space Level")]
    public int spaceCollectibles;
    public int spaceCollected = 0;
    public int spaceCurrentCam;
    public bool spaceBeaten = false;

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

            PlayerController player = GameObject.Find("Player").GetComponent<PlayerController>();
            player.colorA = new Color(colorOptions[chosenColor].colorA.r, colorOptions[chosenColor].colorA.g, colorOptions[chosenColor].colorA.b);
            player.colorB = new Color(colorOptions[chosenColor].colorB.r, colorOptions[chosenColor].colorB.g, colorOptions[chosenColor].colorB.b);
        }
    }
}
