using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathObject : MonoBehaviour
{
    //This is just to store types of death for the player to interact with

    /* TYPES OF DEATHS
     * 0. Falling (Void)
     * 1. Spikes
     * 2. Box Falls on Character
     */
    public int typeOfObject;
    public AudioClip[] deathSFX;

    public AudioClip sendDeathAudio()
    {
        int n = Random.Range(0, deathSFX.Length);
        return deathSFX[n];
    }
}
