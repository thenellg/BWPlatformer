using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customGravitySet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            bool temp = collision.gameObject.GetComponent<customGravController>().active;

            if (temp)
            {
                collision.gameObject.GetComponent<customGravController>().active = false;
                collision.gameObject.GetComponent<CharacterController2D>().gravDirection = "up";
            }
            else
            {
                collision.gameObject.GetComponent<customGravController>().active = true;
            }
        }

    }
}
