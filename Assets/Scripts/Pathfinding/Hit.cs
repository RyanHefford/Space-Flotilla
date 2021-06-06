using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    //When a flock hits a player.
    private Flock flock;
    public Health health;
    private Score score;
    private ShipBehavior ab;
    private GameObject placeHolderParent;

    private void Start()
    {
        //getting the OverlordManager script from the Manager GameObject
       // om = GameObject.Find("Manager").GetComponent<OverlordManager>();
        flock =  getFlock();
        ab = getAgentBehavior();
    }

    private Flock getFlock()
    {
        Flock newFlock = null;
        if(this.transform.gameObject.tag == "Player")
        {
            newFlock =  this.transform.parent.Find("Flock").GetComponent<Flock>();
        }
        else if(this.transform.gameObject.tag == "Missle" || this.transform.gameObject.tag == "HyperBeam")
        {
            newFlock = placeHolderParent.transform.parent.Find("Flock").GetComponent<Flock>();
        }

        return newFlock;
    }
    private ShipBehavior getAgentBehavior()
    {
        ShipBehavior ab = null;
        if (this.transform.gameObject.tag == "Player")
        {
            ab = GetComponent<ShipBehavior>();
        }
        else if (this.transform.gameObject.tag == "Missle" || this.transform.gameObject.tag == "HyperBeam")
        {
            ab = placeHolderParent.GetComponent<ShipBehavior>();
        }

        return ab;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        flock = getFlock();


        //destroying the missle. If statement because the player has the same
        //script attached to it.
        if (this.gameObject.tag == "Missle" && collision.gameObject.tag == "Wall")
        {
            Destroy(this.gameObject);
            ab.Reward(-(1 / flock.startingCount) * 0.2f);
        }

        //remove from the list.
        if (collision.gameObject.tag == "Agent")
        {

            FlockAgent agentToDelete = collision.gameObject.GetComponent<FlockAgent>();

            flock.agents.Remove(agentToDelete);
            //collision.gameObject.GetComponent<EnemyDieScript>().Die();
            Destroy(agentToDelete.gameObject);

            //update the score when we are shooting with the missle:
            if (this.gameObject.tag == "Missle")
            {
                //score = GameObject.Find("Canvas").GetComponent<Canvas>().GetComponent<Score>();
                //score.updateScore(10);

                //add reward for the agent
                ab.Reward(1 / flock.startingCount);
            }

            if (this.gameObject.tag == "Player")
            {
                ab.Reward(-1);
                ab.lose();
                ab.EndEpisode();
            }

        }



    }

    //For missles so that rotation isnt applied to it
    public void AddParent(GameObject parent)
    {
        placeHolderParent = parent;
    }

}
