using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerHit : MonoBehaviour
{
    private static CornerHit _instance;

    public static CornerHit Instance { get { return _instance; } }

    private int countHit = 0;
    public Transform overlordSpawn;
    public GameObject overlordPrefab;
    private Flock flock;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Agent")
        {
            countHit += 1;
            flock = GameObject.Find("Flock").GetComponent<Flock>();
            FlockAgent agentToDelete = collision.gameObject.GetComponent<FlockAgent>();
            flock.agents.Remove(agentToDelete);
            Destroy(collision.gameObject);
            print(countHit);
        }

        if(countHit == 5)
        {
            //instantiate a new overlord
            Instantiate(overlordPrefab, overlordSpawn);
            //cancel all objects going to the corner
            cancelCorner();
            //resent count
            countHit = 0;

        }
    }



    private void cancelCorner()
    {
        foreach(FlockAgent agent in flock.agents)
        {
            //disable pathfinding on that object.
            agent.goToCorner = false;
            agent.nowPathFinding = false;
            agent.GetComponent<EnemyAI>().enabled = false;
        }
    }

}
