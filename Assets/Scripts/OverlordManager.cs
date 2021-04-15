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
    // Start is called before the first frame update
    void Start()
    {
        agents = flock.agents;
        overlordExists = true;
    }

    // Update is called once per frame
    void Update()
    {
        //If I want random agents to go to corners
        if (!overlordExists && !creatingOverlord)
        {
            creatingOverlord = true;
            //generate a random number between 0-3 to get a random corner.
            getRandomCorner();
            //Selecting 5 random agents to go to a specific corner.
            selectRandomAgents();
            
            
        }
        //check for if the agent which was going to the corner got destroyed or not.

    }

    private void getRandomCorner()
    {
        randomCorner = Random.Range(0, 4);
    }
    private void selectRandomAgents()
    {
        //setting their goToCorner bool to true.
        //5 random agents
        int count = 0;

        try
        {

  

            while (count < 5)
            {
                bool finished = false;
                while (!finished)
                {
                    int agentNumber = Random.Range(0, agents.Count - 1);
                    if (!agents[agentNumber].goToCorner)
                    {
                        agents[agentNumber].goToCorner = true;
                        finished = true;
                    }

                }
                count += 1;
            }
        }
        catch (System.IndexOutOfRangeException ex)
        {
            print("Out of bounds!");

        }

    }
}
