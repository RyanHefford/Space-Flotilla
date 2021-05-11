using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    //When a flock hits a player.
    private Flock flock;

    public Health health;
    private Score score;

    private OverlordManager om;

    private void Start()
    {
        //getting the OverlordManager script from the Manager GameObject
        om = GameObject.Find("Manager").GetComponent<OverlordManager>();
        flock =  getFlock();
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
            }

            if (this.gameObject.tag == "Player")
            {
                health.takeDamage(1.0f);
            }

        }

        //If we hit an overlord with the missle.
        if (collision.gameObject.tag == "Overlord" && om.overlordExists)
        {


            collision.gameObject.GetComponent<OverlordHealth>().takeDamage(1.0f);


            if(collision.gameObject.GetComponent<OverlordHealth>().getPlayerHealth() <= 0.0f)
            {
                //Whe overlord dies.
                Flock flock = GameObject.Find("Flock").GetComponent<Flock>();
                //reset the agents that are sticking to the overlord
                //and make them not STICk to the overlord
                resetStickToOverlordAgents(flock);
                //remove the Overlord.
                FlockAgent agentToDelete = collision.gameObject.GetComponent<FlockAgent>();
                //Remove overlord from list
                flock.agents.Remove(agentToDelete);
                //overlord does not exist
                om.overlordExists = false;
                //Destroy(collision.gameObject);
                collision.gameObject.GetComponent<EnemyDieScript>().Die();
                
            }


            //more points for destroying an overlord
            //update the score when we are shooting with the missle:
            if (this.gameObject.tag == "Missle")
            {
                score = GameObject.Find("Canvas").GetComponent<Canvas>().GetComponent<Score>();
                score.updateScore(30);
            }



            if (this.gameObject.tag == "Player")
            {
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

        }

        //If we hit an overlord with the missle.
        if (collision.gameObject.tag == "Overlord")
        {
            collision.gameObject.GetComponent<OverlordHealth>().takeDamage(1.0f);


            if (collision.gameObject.GetComponent<OverlordHealth>().getPlayerHealth() <= 0.0f)
            {
                //Whe overlord dies.
                Flock flock = GameObject.Find("Flock").GetComponent<Flock>();
                //reset the agents that are sticking to the overlord
                //and make them not STICk to the overlord
                resetStickToOverlordAgents(flock);
                //remove the Overlord.
                FlockAgent agentToDelete = collision.gameObject.GetComponent<FlockAgent>();
                //Remove overlord from list
                flock.agents.Remove(agentToDelete);
                //overlord does not exist
                om.overlordExists = false;
                //Destroy(collision.gameObject);
                collision.gameObject.GetComponent<EnemyDieScript>().Die();

            }
            //more points for destroying an overlord
            
            score = GameObject.Find("Canvas").GetComponent<Canvas>().GetComponent<Score>();
            score.updateScore(30);
            
        }
    }


    public void OnTriggerStay2D(Collider2D collision)
    {
        flock = getFlock();
        if (collision.gameObject.tag == "Overlord")
        {
            collision.gameObject.GetComponent<OverlordHealth>().takeDamage(1.0f);


            if (collision.gameObject.GetComponent<OverlordHealth>().getPlayerHealth() <= 0.0f)
            {
                //Whe overlord dies.
                Flock flock = GameObject.Find("Flock").GetComponent<Flock>();
                //reset the agents that are sticking to the overlord
                //and make them not STICk to the overlord
                resetStickToOverlordAgents(flock);
                //remove the Overlord.
                FlockAgent agentToDelete = collision.gameObject.GetComponent<FlockAgent>();
                //Remove overlord from list
                flock.agents.Remove(agentToDelete);
                //overlord does not exist
                om.overlordExists = false;
                //Destroy(collision.gameObject);
                collision.gameObject.GetComponent<EnemyDieScript>().Die();

            }
            //more points for destroying an overlord

            score = GameObject.Find("Canvas").GetComponent<Canvas>().GetComponent<Score>();
            score.updateScore(30);

        }
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
