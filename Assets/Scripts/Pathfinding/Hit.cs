using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    //When a flock hits a player.
    private Flock flock;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //remove from the list.
        if (collision.gameObject.tag == "Agent")
        {
            flock = GameObject.Find("Flock").GetComponent<Flock>();
            FlockAgent agentToDelete = collision.gameObject.GetComponent<FlockAgent>();
            flock.agents.Remove(agentToDelete);
            Destroy(collision.gameObject);
            
        }

        //destroying the missle. If statement because the player has the same
        //script attached to it.
        if (this.gameObject.tag == "Missle")
        {
            Destroy(this.gameObject);
        }

    }

}
