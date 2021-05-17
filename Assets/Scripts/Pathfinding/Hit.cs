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

        //remove from the list.
        if (collision.gameObject.tag == "Agent")
        {

            FlockAgent agentToDelete = collision.gameObject.GetComponent<FlockAgent>();

            flock.agents.Remove(agentToDelete);
            collision.gameObject.GetComponent<EnemyDieScript>().Die();

            //update the score when we are shooting with the missle:
            if (this.gameObject.tag == "Missle")
            {
                score = GameObject.Find("Canvas").GetComponent<Canvas>().GetComponent<Score>();
                score.updateScore(10);

                //add reward for the agent
                ab.AddReward(10);
            }

            if (this.gameObject.tag == "Player")
            {
                ab.AddReward(-30);
                health.takeDamage(1.0f);
            }

        }

      

        //destroying the missle. If statement because the player has the same
        //script attached to it.
        if (this.gameObject.tag == "Missle")
        {
            Destroy(this.gameObject);
        }

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        flock = getFlock();
        //remove from the list.
        if (collision.gameObject.tag == "Agent")
        {

            FlockAgent agentToDelete = collision.gameObject.GetComponent<FlockAgent>();
            flock.agents.Remove(agentToDelete);
            collision.gameObject.GetComponent<EnemyDieScript>().Die();

            //update the score when we are shooting with the missle:
            
            score = GameObject.Find("Canvas").GetComponent<Canvas>().GetComponent<Score>();
            score.updateScore(10);
            ab.AddReward(10);
        }

       
    }


    public void OnTriggerStay2D(Collider2D collision)
    {
        flock = getFlock();
       
    }


    private void resetStickToOverlordAgents(Flock flock)
    {
        foreach (FlockAgent agent in flock.agents)
        {
            if (!agent.isOverlord)
            {
                agent.stickingToOverlord = false;
            }

        }
    }

}
