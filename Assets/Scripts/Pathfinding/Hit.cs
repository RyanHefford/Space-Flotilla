using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

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
            float penalty = -50;

            Destroy(this.gameObject);
            
            float scale_factor = Mathf.Min(1, ab.getTotalCount() / 150000.0f);
            float scaled_penalty = penalty * scale_factor;
            Debug.Log(ab.getTotalCount());
            Debug.Log(scaled_penalty);

            ab.AddReward(scaled_penalty);

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
                ab.AddReward(50);
            }

            if (this.gameObject.tag == "Player")
            {
                ab.AddReward(-30);
                health.takeDamage(1.0f);
            }

        }

        if (this.gameObject.tag == "Player" && collision.gameObject.tag == "Wall")
        {
            ab.AddReward(-50);
        }



    }



}
