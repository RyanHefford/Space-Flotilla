using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    //When a flock hits a player.
    private Flock flock;

    private Score score;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //remove from the list.
        if (collision.gameObject.tag == "Agent")
        {
            flock = GameObject.Find("Flock").GetComponent<Flock>();
            FlockAgent agentToDelete = collision.gameObject.GetComponent<FlockAgent>();
            flock.agents.Remove(agentToDelete);
            collision.gameObject.GetComponent<EnemyAI>().Die();

            //update the score when we are shooting with the missle:
            if(this.gameObject.tag == "Missle" || this.gameObject.tag == "HyperBeam")
            {
                score = GameObject.Find("Canvas").GetComponent<Canvas>().GetComponent<Score>();
                score.updateScore(10);
            }

            if (this.gameObject.tag == "Player")
            {
                GetComponent<Health>().takeDamage(1.0f);
            }

        }

        //destroying the missle. If statement because the player has the same
        //script attached to it.
        if (this.gameObject.tag == "Missle")
        {
            Destroy(this.gameObject);
        }

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Agent")
        {
            flock = GameObject.Find("Flock").GetComponent<Flock>();
            FlockAgent agentToDelete = other.gameObject.GetComponent<FlockAgent>();
            flock.agents.Remove(agentToDelete);
            other.gameObject.GetComponent<EnemyAI>().Die();
        }
    }
}
