using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    //When a flock hits a player.
    private Flock flock;

    public Health health;
    private Score score;
    private AgentBehavior ab;

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
            newFlock = this.transform.parent.transform.parent.Find("Flock").GetComponent<Flock>();
        }

        return newFlock;
    }
    private AgentBehavior getAgentBehavior()
    {
        AgentBehavior ab = null;
        if (this.transform.gameObject.tag == "Player")
        {
            ab = GetComponent<AgentBehavior>();
        }
        else if (this.transform.gameObject.tag == "Missle" || this.transform.gameObject.tag == "HyperBeam")
        {
            ab = this.transform.parent.GetComponent<AgentBehavior>();
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
            ab.AddReward(-0.10f);
            // Debugging player shot a wall
            Debug.Log("Player shot a wall");
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

                //add reward for the agent destroyed via missile
                ab.AddReward(0.30f);
                // Debugging agent shot by missile
                Debug.Log("Agent shot by a missile");
            }

            // Losing when player and agent collide, ending episode
            if (this.gameObject.tag == "Player")
            {
                ab.lose();
                ab.AddReward(-0.40f);
                // Debugging agent colliding with player
                Debug.Log("Agent collided with player");
                health.takeDamage(1.0f);
                ab.EndEpisode();
            }

        }

        // player collides with walls, negative reward
        if (this.gameObject.tag == "Player" && collision.gameObject.tag == "Wall")
        {
            ab.AddReward(-0.05f);
            // Debugging player ran into a wall
            Debug.Log("Player ran into a wall");
        }



    }



}
