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
    public SpriteRenderer sr;
    private Health health;

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




}


