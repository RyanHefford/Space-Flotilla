﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AgentBehavior : Agent
{

    [SerializeField]
    private Transform goalTransform;

    private Flock flock;

    private void Start()
    {
        flock = transform.parent.Find("Flock").GetComponent<Flock>();
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
        sensor.AddObservation(goalTransform.localPosition);

    }


    //where the actions of the player happens.
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];

        transform.localPosition += new Vector3(moveX, moveY, 0) * Time.deltaTime * 10;
    }


    //function to test out the game before running the trainning
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;

        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle") { 

            AddReward(5f);
            EndEpisode();

        }
        else if (collision.gameObject.tag == "Agent")
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
