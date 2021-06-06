using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class ShipBehavior : Agent
{

    private Flock flock;
    private PlayerMovement playerMovement;
    private PlayerShoot playerShoot;
    public GameObject successIndicator;
    private Health health;


    private void Start()
    {
        flock = transform.parent.Find("Flock").GetComponent<Flock>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShoot = GetComponent<PlayerShoot>();
        health = GetComponent<Health>();

        //set number of agents according to curriculum
        flock.startingCount = (int)Academy.Instance.EnvironmentParameters.GetWithDefault("num_enemies", flock.startingCount);
    }

    private void Update()
    {
        if(flock.agents.Count == 0)
        {
            win();
            Reward(1);
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
        health.playerHealth = 1;

        playerShoot.newEpisode();

        //reset player rotation
        transform.rotation = new Quaternion(0,0,0,0);
        //set rotation randomly so ml cannot shoot forward for best score
        transform.Rotate(0, 0, Random.Range(0, 360));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //Things the agent need to know in the world.
        sensor.AddObservation(transform.localPosition);
        //sensor.AddObservation(flock.agents.Count);
        //sensor.AddObservation(health.playerHealth);
        sensor.AddObservation(transform.rotation);
        //50 steps = 1 second
        sensor.AddObservation((float)StepCount / MaxStep);
        //next 10 are reserved for agents
        for(int i = 0; i < flock.startingCount; i++)
        {
            if(i < flock.agents.Count)
            {
                if (flock.agents[i] == null)
                {
                    sensor.AddObservation(Vector3.zero);
                    continue;
                }
                Vector3 agentPos = flock.agents[i].transform.localPosition;
                sensor.AddObservation(agentPos);
            }
            else
            {
                sensor.AddObservation(Vector3.zero);
                continue;
            }
            
        }

    }


    //where the actions of the player happens.
    public override void OnActionReceived(ActionBuffers actions)
    {
        

        float movementSpeed = 3.0f;
        float rotationSpeed = 5.0f;
        float rotationAngle = 20.0f;
        int movement = actions.DiscreteActions[0];
        int rotation = actions.DiscreteActions[1];
        int shoot = actions.DiscreteActions[2];

        Vector2 movementForceDirection = movementVector(movement);

        //add the movement force
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        //rb.AddForce(movementForceDirection * movementSpeed);



        transform.position += (Vector3)movementForceDirection * movementSpeed * Time.deltaTime;

        //positive = <-
        //negative = ->
        switch (rotation)
        {
            case 0:
                //no rotation
                break;
            case 1:
            //rotate right
                transform.Rotate(new Vector3(0, 0, -1), rotationAngle * Time.deltaTime * rotationSpeed);
                break;
            case 2:
                //rotate left
                transform.Rotate(new Vector3(0, 0, 1), rotationAngle * Time.deltaTime * rotationSpeed);
                break;
            default:
                //no rotation
                break;
        }

        playerShoot.shoot = shoot == 1;

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

    public void Reward(float reward)
    {
        AddReward(reward);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Agent")
        {
            Reward(-0.1f);
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
        foreach (SpriteRenderer asteroid in successIndicator.GetComponentsInChildren<SpriteRenderer>())
        {

            asteroid.material.color = Color.red;
        }
    }
    public void win()
    {
        foreach (SpriteRenderer asteroid in successIndicator.GetComponentsInChildren<SpriteRenderer>())
        {

            asteroid.material.color = Color.green;
        }
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


