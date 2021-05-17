using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AgentBehavior : Agent
{

    private Flock flock;
    private PlayerMovement pm;
    private PlayerShoot ps;

    private void Start()
    {
        flock = transform.parent.Find("Flock").GetComponent<Flock>();
        pm = GetComponent<PlayerMovement>();
        ps = GetComponent<PlayerShoot>();
    }

    public override void OnEpisodeBegin()
    {

        //reset player position
        transform.localPosition = new Vector3(-4, -6, 0);
        //reset the flocks
        destroyAgents();
        flock.startNewEp();

        

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //Things the agent need to know in the world.
        sensor.AddObservation(transform.localPosition);

    }


    //where the actions of the player happens.
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];
        float mouseX = actions.ContinuousActions[2];
        float mouseY = actions.ContinuousActions[3];
        //transform.localPosition += new Vector3(moveX, moveY, 0) * Time.deltaTime * 10;
        pm.moveX = moveX;
        pm.moveY = moveY;
        pm.mouseX = mouseX;
        pm.mouseY = mouseY;

        ps.shoot = actions.DiscreteActions[0];
    }


    //function to test out the game before running the trainning
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
        Vector3 mousePos = Input.mousePosition;
        continuousActions[2] = mousePos.x;
        continuousActions[3] = mousePos.y;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            discreteActions[0] = 1;
        }
        else
        {
            discreteActions[0] = 0;
        }

        
    }

    public void reward(int r)
    {
        AddReward(r);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Agent")
        {
            AddReward(-15f);
            EndEpisode();
        }
        else{ 

            AddReward(-5f);
            EndEpisode();
        }
    }

    private void destroyAgents()
    {
        for (int i = 0; i < flock.agents.Count; i++)
        {
            //Destroy the game objects (Agents)
            Destroy(flock.agents[i].gameObject);
        }
        //clear the list.
        flock.agents.Clear();

    }




}
