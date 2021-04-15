using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerHit : MonoBehaviour
{
    private static CornerHit _instance;

    public static CornerHit Instance { get { return _instance; } }

    private Flock flock;

    private OverlordManager om;

    private void Start()
    {
        om = GameObject.Find("Manager").GetComponent<OverlordManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Agent")
        {
            CornerCount._instance.countHit += 1;
            flock = GameObject.Find("Flock").GetComponent<Flock>();
            FlockAgent agentToDelete = collision.gameObject.GetComponent<FlockAgent>();
            flock.agents.Remove(agentToDelete);
            Destroy(collision.gameObject);
            print(CornerCount._instance.countHit);
        }

        if(CornerCount._instance.countHit == 5)
        {
            //instantiate a new overlord
            Instantiate(CornerCount._instance.overlordPrefab, CornerCount._instance.overlordSpawn);
            //cancel all objects going to the corner
            cancelCorner();
            //resent count
            CornerCount._instance.countHit = 0;

            //set the overlord to exist.
            om.overlordExists = true;

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
