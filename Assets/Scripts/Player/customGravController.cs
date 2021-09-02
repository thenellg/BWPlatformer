using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customGravController : MonoBehaviour
{
    //reverse Gravity Controller

    public float tempGravScale;
    public float tempDashSpeed;
    private CharacterController2D mCharacterController2D = null;

    public float grav = 0.48f;
    public bool active = true;
    private Rigidbody2D player;
    public Vector2 gravity = new Vector2(0, -0.48f);
    public float dashSpeed;

    public string direction;
    public bool isPlayer = false;

    private void Start()
    {
        player = this.GetComponent<Rigidbody2D>();
        tempGravScale = player.gravityScale;

        if (isPlayer)
        {
            mCharacterController2D = this.GetComponent<CharacterController2D>();
            tempDashSpeed = mCharacterController2D.dashSpeed;
        }
        else
            mCharacterController2D = null;
    }

    private void Update()
    {
        Transform temp = player.transform;
        if (active)
        {
            if (mCharacterController2D)
            {
                mCharacterController2D.forcedGrav = true;
                mCharacterController2D.dashSpeed = dashSpeed;
                mCharacterController2D.gravDirection = direction;
            }

            player.gravityScale = 0;

            player.AddForce(gravity, ForceMode2D.Force);

            if (direction == "up")
            {
                gravity = new Vector2(0, -grav);
                temp.rotation = Quaternion.Euler(Vector3.zero);
            }
            else if (direction == "down")
            {
                gravity = new Vector2(0, grav);
                temp.rotation = Quaternion.Euler(Vector3.zero);
                Vector3 theScale = temp.localScale;
                if (theScale.y > 0)
                {
                    theScale.y *= -1;
                    temp.localScale = theScale;
                }
            }
            else if (direction == "left")
            {
                gravity = new Vector2(-grav, 0);
                temp.rotation = Quaternion.Euler(new Vector3(temp.rotation.x, temp.rotation.y, -90));
            }
            else if (direction == "right")
            {
                gravity = new Vector2(grav, 0);
                temp.rotation = Quaternion.Euler(new Vector3(temp.rotation.x, temp.rotation.y, 90));
            }
        }
        else
        {
            temp.rotation = Quaternion.Euler(Vector3.zero);
            Vector3 theScale = temp.localScale;
            if (theScale.y < 0)
            {
                theScale.y *= -1;
                temp.localScale = theScale;
            }

            exitForcedGrav();
        }
    }

    public void setForcedGrav(string setDirection)
    {
        //player.AddRelativeForce(new Vector2(gravity.x * 10f, gravity.y * 10f), ForceMode2D.Impulse);

        direction = setDirection;
    }
    
    public void exitForcedGrav()
    {
        if (isPlayer)
        {
            mCharacterController2D.gravDirection = "up";

            mCharacterController2D.forcedGrav = false;
            mCharacterController2D.dashSpeed = tempDashSpeed;
        }
        player.gravityScale = tempGravScale;
    }

}
