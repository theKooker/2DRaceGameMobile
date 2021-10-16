using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public GameObject breaks;
    public void goToLeft()
    {
        if (player.transform.position.x > -1.5f && !player.GetComponent<PlayerCollision>().isCrashed())
            player.transform.position = new Vector3(player.transform.position.x - 1.5f, player.transform.position.y, player.transform.position.z);

    }

    public void goToRight()
    {
        if (player.transform.position.x < 1.5f && !player.GetComponent<PlayerCollision>().isCrashed())
            player.transform.position = new Vector3(player.transform.position.x + 1.5f, player.transform.position.y, player.transform.position.z);
    }

    public void goUp()
    {
        breaks.GetComponent<Breaks>().increaseSpeed();
        if (player.transform.position.y <= 2.5f && !player.GetComponent<PlayerCollision>().isCrashed())
        {
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + .5f, player.transform.position.z);
        }
    }

    public void goDown()
    {
        breaks.GetComponent<Breaks>().decreaseSpeed();
        if (player.transform.position.y >= -2.5f && !player.GetComponent<PlayerCollision>().isCrashed())
        {
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - .5f, player.transform.position.z);
        }
    }

}
