using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlordManager : MonoBehaviour
{
    public Flock flock;
    private List<FlockAgent> agents;
    public int randomCorner = 0;
    public bool overlordExists;
    public bool creatingOverlord = false;
    private int agentsToSendToCorner = 5;
    private OverlordManager om;
    private MapScript ms;

    private int timeToChangeDirection = 5;
    public float timeRemaining = 5;
    public Vector2 wanderLocation;

    //For corners and creating an overlord
    private Transform[] corners = new Transform[4];
    private GameObject cornersGO;

    // Start is called before the first frame update
    void Start()
    {
        agents = flock.agents;
        overlordExists = true;
        om = GameObject.Find("Manager").GetComponent<OverlordManager>();
        ms = GameObject.Find("Background").GetComponent<MapScript>();
        
        //Get corners game object
        cornersGO = GameObject.Find("Corners");
        //Add children to array
        for (int i = 0; i < cornersGO.transform.childCount; i++)
        {
            corners[i] = cornersGO.transform.GetChild(i).transform;
        }
        //setup the wander location for the overlord:
        float xDist = Random.Range(ms.getEastEdge(), ms.getWestEdge());
        float yDist = Random.Range(ms.getSouthEdge(), ms.getNorthEdge());
        wanderLocation = new Vector2(xDist, yDist);

    }

    // Update is called once per frame
    void Update()
    {
        
        agents = flock.agents;
        //If I want random agents to go to corners
        if (!overlordExists && !creatingOverlord && agents.Count >= 5)
        {
            creatingOverlord = true;
            //generate a random number between 0-3 to get a random corner.
            getRandomCorner();
            //Selecting 5 random agents to go to a specific corner.
            selectRandomAgents();

            //countPlayer();
        }

        //check for if the agent which was going to the corner got destroyed or not.
        if(agents.Count >= 5)
        {
            checkingAgentsForOverlordCreation();
        }

        //timer:
        timeRemaining -= Time.deltaTime;

        if(timeRemaining < -0.5)
        {
            //reset timer
            timeRemaining = timeToChangeDirection;
            //new location
            float xDist = Random.Range(ms.getEastEdge(), ms.getWestEdge());
            float yDist = Random.Range(ms.getSouthEdge(), ms.getNorthEdge());
            wanderLocation = new Vector2(xDist, yDist);
        }

    }


    private void checkingAgentsForOverlordCreation()
    {

        //check how many agents have entered.
        int entered = CornerCount._instance.countHit;

        //check how many agents are going now
        int count = 0;
        foreach (FlockAgent agent in flock.agents)
        {
            if (agent.goToCorner)
            {
                count += 1;
            }
        }

        if(entered + count != agentsToSendToCorner && !om.overlordExists)
        {
            //Then one or more agents got destroyed along the way!!
            int numAgentsNeeded = agentsToSendToCorner - (entered + count);
            selectNewAgents(numAgentsNeeded);
            print("Selected a new Agent!");

        }

    }

    private void getRandomCorner()
    {
        randomCorner = Random.Range(0, 4);
    }

    private void selectNewAgents(int numOfAgents)
    {
        //setting their goToCorner bool to true.
        //5 random agents
        int count = 0;
        print("USED");

        while (count < numOfAgents)
        {
            bool finished = false;
            while (!finished)
            {
                int agentNumber = Random.Range(0, agents.Count - 1);
                if (!agents[agentNumber].goToCorner)
                {
                    agents[agentNumber].goToCorner = true;
                    agents[agentNumber].GetComponent<EnemyAI>().enabled = true;
                     agents[agentNumber].nowPathFinding = true;
                    agents[agentNumber].enemyAI.target = corners[om.randomCorner];
                    finished = true;
                }

            }
            count += 1;
        }


    }

    private void selectRandomAgents()
    {
        //setting their goToCorner bool to true.
        //5 random agents
        int count = 0;
        print("Selected agents");
  
        while (count < agentsToSendToCorner)
        {
            bool finished = false;
            while (!finished)
            {
                int agentNumber = Random.Range(0, agents.Count - 1);
                if (!agents[agentNumber].goToCorner)
                {
                    agents[agentNumber].goToCorner = true;
                    agents[agentNumber].GetComponent<EnemyAI>().enabled = true;
                    agents[agentNumber].nowPathFinding = true;
                    agents[agentNumber].enemyAI.target = corners[om.randomCorner];
                    finished = true;
                }

            }
            count += 1;
        }
        print("FINISHED SELECTING AGENTS");
        print(count);


    }

    private void countPlayer()
    {
        int count = 0;
        foreach(FlockAgent agent in flock.agents)
        {
            if (agent.GetComponent<EnemyAI>().enabled)
            {
                count += 1;
            }
        }
        print("Count is " + count);
    }

    private int wanderRingDistance = 200;
    private int wanderRingRadius = 50;

    private Vector2 wander()
    {
        //get the overlord object.
        FlockAgent overlord = null;

        foreach(FlockAgent agent in flock.agents)
        {
            if (agent.isOverlord)
            {
                overlord = agent;
                break;
            }
        }

        //return if no overlord exists.
        if(overlord == null)
        {
            return Vector2.zero;
        }

        //calculate the next waypoint for the overlord to go to.

        //the circle position infront of the overlord
        Vector2 circlePos = (Vector2)overlord.transform.position + overlord.GetComponent<Rigidbody2D>().velocity.normalized * wanderRingDistance;
        float yDist = Random.Range(ms.getSouthEdge(), ms.getNorthEdge());
        Vector2 target = circlePos + new Vector2(wanderRingRadius, yDist);
        return target;
    }



}
