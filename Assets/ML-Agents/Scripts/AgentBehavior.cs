﻿using System.Collections;
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
    public SpriteRenderer sr;
    private Health health;

    public SpriteRenderer test;

    private void Start()
    {
        flock = transform.parent.Find("Flock").GetComponent<Flock>();
        pm = GetComponent<PlayerMovement>();
        ps = GetComponent<PlayerShoot>();
        health = GetComponent<Health>();
    }

    private void Update()
    {
        if(flock.agents.Count == 0)
        {
            win();
            AddReward(100);
            EndEpisode();
        }
    }

    public override void OnEpisodeBegin()
    {

        //reset player position
        transform.localPosition = new Vector3(0, -6.5f, 0);
        //reset the flocks
        destroyAgents();
        flock.startNewEp();

        //reset player health
        health.playerHealth = 10;

        

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //Things the agent need to know in the world.
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(flock.agents.Count);
        sensor.AddObservation(health.playerHealth);

        //Add player rotation?


    }


    //where the actions of the player happens.
    public override void OnActionReceived(ActionBuffers actions)
    {
        //float moveX = actions.ContinuousActions[0];
        //float moveY = actions.ContinuousActions[1];
        //float mouseX = actions.ContinuousActions[2];
        //float mouseY = actions.ContinuousActions[3];
        ////transform.localPosition += new Vector3(moveX, moveY, 0) * Time.deltaTime * 10;
        //pm.moveX = moveX;
        //pm.moveY = moveY;


        //ps.shoot = actions.DiscreteActions[0];

        //int xDiscrete = actions.DiscreteActions[1];
        //int yDiscrete = actions.DiscreteActions[2];

        //if(mouseX < 0)
        //{
        //    pm.mouseX = xDiscrete * -1 + mouseX;
        //}
        //else
        //{
        //    pm.mouseX = xDiscrete + mouseX;
        //}
        //if(mouseY < 0)
        //{
        //    pm.mouseY = yDiscrete * -1 + mouseY;
        //}
        //else
        //{
        //    pm.mouseY = yDiscrete + mouseY;
        //}

        float movementSpeed = 3.0f;
        int movement = actions.DiscreteActions[0];
        int rotation = actions.DiscreteActions[1];

        Vector2 movementForceDirection = movementVector(movement);

        //add the movement force
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
       // rb.AddForce(movementForceDirection * movementSpeed);

       


        var impulse = (120 * Mathf.Deg2Rad) * rb.inertia;
        rb.AddTorque(impulse, ForceMode2D.Impulse);


        //test.transform.localPosition = new Vector3(pm.mouseX, pm.mouseY, 0);

    }



    //function to test out the game before running the trainning
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        //continuousActions[0] = Input.GetAxisRaw("Horizontal");
        //continuousActions[1] = Input.GetAxisRaw("Vertical");
        //Vector3 mousePos = Input.mousePosition;

        //continuousActions[2] = mousePos.x;
        //continuousActions[3] = mousePos.y;
        

        if (Input.GetKey(KeyCode.Mouse0))
        {
            discreteActions[0] = 1;
        }
        else
        {
            discreteActions[0] = 0;
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float rotationSpeed = 2;



    }

    public void reward(int r)
    {
        AddReward(r);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            lose();
            AddReward(-10f);
            EndEpisode();
        }

    }

    private void destroyAgents()
    {

        foreach(Transform child in flock.transform)
        {
            Destroy(child.gameObject);
        }
        
        //clear the list.
        flock.agents.Clear();

    }

    public void lose()
    {
        sr.color = Color.red;
    }
    public void win()
    {
        sr.color = Color.green;
    }


    private Vector2 movementVector(int movement)
    {
        Vector2 forceDirection = Vector2.zero;

        switch (movement)
        {
            case 0:
                //no movement
                forceDirection = Vector2.zero;
                break;
            case 1:
                //Move right
                forceDirection = new Vector2(1.0f, 0.0f);
                break;
            case 2:
                //Move left
                forceDirection = new Vector2(-1.0f, 0.0f);
                break;
            case 3:
                //Move up
                forceDirection = new Vector2(0.0f, 1.0f);
                break;
            case 4:
                //Move down
                forceDirection = new Vector2(0.0f, -1.0f);
                break;
            case 5:
                //Move top-right
                forceDirection = new Vector2(1.0f, 1.0f);
                break;
            case 6:
                //Move top-left
                forceDirection = new Vector2(-1.0f, 1.0f);
                break;
            case 7:
                //Move bottom-right
                forceDirection = new Vector2(1.0f, -1.0f);
                break;
            case 8:
                //Move left
                forceDirection = new Vector2(-1.0f, -1.0f);
                break;
            default:
                //Don't move
                forceDirection = Vector2.zero;
                break;
        }
        return forceDirection;
    }


}


