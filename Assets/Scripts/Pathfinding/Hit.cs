﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    //When a flock hits a player.
    private Flock flock;
    private GameObject player;
    private Score score;

    private OverlordManager om;


    private void Start()
    {
        //getting the OverlordManager script from the Manager GameObject
        om = GameObject.Find("Manager").GetComponent<OverlordManager>();

        player = GameObject.Find("Player");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //remove from the list.
        if (collision.gameObject.tag == "Agent")
        {
            flock = GameObject.Find("Flock").GetComponent<Flock>();
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
                player.GetComponent<Health>().takeDamage(1.0f);
            }

        }

        //If we hit an overlord with the missle.
        if (collision.gameObject.tag == "Overlord")
        {
            flock = GameObject.Find("Flock").GetComponent<Flock>();
            //reset the agents that are sticking to the overlord
            //and make them not STICk to the overlord
            resetStickToOverlordAgents();
            //remove the Overlord.
            FlockAgent agentToDelete = collision.gameObject.GetComponent<FlockAgent>();
            flock.agents.Remove(agentToDelete);
            //overlord does not exist
            om.overlordExists = false;

            //more points for destroying an overlord
            //update the score when we are shooting with the missle:
            if (this.gameObject.tag == "Missle")
            {
                score = GameObject.Find("Canvas").GetComponent<Canvas>().GetComponent<Score>();
                score.updateScore(30);
            }

            if (this.gameObject.tag == "Player")
            {
                player.GetComponent<Health>().takeDamage(1.0f);
            }

            collision.gameObject.GetComponent<EnemyDieScript>().Die();
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
        //remove from the list.
        if (collision.gameObject.tag == "Agent")
        {
            flock = GameObject.Find("Flock").GetComponent<Flock>();
            FlockAgent agentToDelete = collision.gameObject.GetComponent<FlockAgent>();
            flock.agents.Remove(agentToDelete);
            collision.gameObject.GetComponent<EnemyDieScript>().Die();
            score = GameObject.Find("Canvas").GetComponent<Canvas>().GetComponent<Score>();
            score.updateScore(10);
            

        }

        //If we hit an overlord with the missle.
        if (collision.gameObject.tag == "Overlord")
        {
            flock = GameObject.Find("Flock").GetComponent<Flock>();
            //reset the agents that are sticking to the overlord
            //and make them not STICk to the overlord
            resetStickToOverlordAgents();
            //remove the Overlord.
            FlockAgent agentToDelete = collision.gameObject.GetComponent<FlockAgent>();
            flock.agents.Remove(agentToDelete);
            //overlord does not exist
            om.overlordExists = false;

            score = GameObject.Find("Canvas").GetComponent<Canvas>().GetComponent<Score>();
            score.updateScore(30);

            collision.gameObject.GetComponent<EnemyDieScript>().Die();
        }
    }

    private void resetStickToOverlordAgents()
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
