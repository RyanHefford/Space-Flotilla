using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    //When a flock hits a player.
    private Flock flock;

    private Score score;

    private OverlordManager om;


    private void Start()
    {
        //getting the OverlordManager script from the Manager GameObject
        om = GameObject.Find("Manager").GetComponent<OverlordManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //remove from the list.
        if (collision.gameObject.tag == "Agent")
        {
            flock = GameObject.Find("Flock").GetComponent<Flock>();
            FlockAgent agentToDelete = collision.gameObject.GetComponent<FlockAgent>();
            flock.agents.Remove(agentToDelete);
            Destroy(collision.gameObject);

            //update the score when we are shooting with the missle:
            if(this.gameObject.tag == "Missle")
            {
                score = GameObject.Find("Canvas").GetComponent<Canvas>().GetComponent<Score>();
                score.updateScore(10);
            }
            
        }

        //If we hit an overlord with the missle.
        if(collision.gameObject.tag == "Overlord")
        {

            om.overlordExists = false;
            //more points for destroying an overlord
            //update the score when we are shooting with the missle:
            if (this.gameObject.tag == "Missle")
            {
                score = GameObject.Find("Canvas").GetComponent<Canvas>().GetComponent<Score>();
                score.updateScore(30);
            }
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
