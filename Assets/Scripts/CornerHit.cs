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
            FlockAgent newAgent = Instantiate(CornerCount._instance.overlordPrefab,
                                    CornerCount._instance.overlordSpawn[om.randomCorner].position,
                                    Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)));

            //Get the flock agents list
            flock = GameObject.Find("Flock").GetComponent<Flock>();
            newAgent.name = "Overlord";
            //add the newly instantiated overlord to the list of agents
            flock.agents.Add(newAgent);

            //cancel all objects going to the corner
            cancelCorner();
            //resent count
            CornerCount._instance.countHit = 0;

            //set the overlord to exist.
            om.overlordExists = true;
            om.creatingOverlord = false;
        }
    }



    private void cancelCorner()
    {
        foreach(FlockAgent agent in flock.agents)
        {
            //disable pathfinding on that object.
            if (agent.goToCorner)
            {
                agent.goToCorner = false;
                agent.nowPathFinding = false;
                agent.GetComponent<EnemyAI>().enabled = false;
            }

        }
    }

}
