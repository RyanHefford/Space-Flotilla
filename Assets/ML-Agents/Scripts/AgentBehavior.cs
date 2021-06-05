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
    private Rigidbody2D rb;

    private void Start()
    {
        flock = transform.parent.Find("Flock").GetComponent<Flock>();
        pm = GetComponent<PlayerMovement>();
        ps = GetComponent<PlayerShoot>();
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        if(flock.agents.Count == 0)
        {
            win();
            EndEpisode();
        }
        wallDetection();
        //obstacleDetection();


    }

    public int getTotalCount()
    {
        return Academy.Instance.TotalStepCount;
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

        //reset player rotation
        transform.rotation = new Quaternion(0,0, 0, 0);


    }

    public override void CollectObservations(VectorSensor sensor)
    {
        
        //Things the agent need to know in the world.
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.localRotation);
        sensor.AddObservation(flock.agents.Count);
        sensor.AddObservation(health.playerHealth);
        
        //50 steps = 1 second
        sensor.AddObservation((float)StepCount / MaxStep);
        //shoot
        sensor.AddObservation(ps.shoot);
        sensor.AddObservation(rb.velocity);

        //getting obstacles
        Transform obstacles = transform.parent.Find("Obstacles").transform;
        foreach(Transform c in obstacles)
        {
            //Adding all the obstacle locations
            sensor.AddObservation(c.localPosition);
        }

    }


    //where the actions of the player happens.
    public override void OnActionReceived(ActionBuffers actions)
    {
        

        float movementSpeed = 4.0f;
        float rotationSpeed = 5.0f;
        float rotationAngle = 10.0f;
        int movement = actions.DiscreteActions[0];
        int rotation = actions.DiscreteActions[1];
        int shoot = actions.DiscreteActions[2];

        Vector2 movementForceDirection = movementVector(movement);

        //add the movement force
        //Rigidbody2D rb = GetComponent<Rigidbody2D>();
        //rb.AddForce(movementForceDirection * movementSpeed);



        transform.localPosition += (Vector3)movementForceDirection * movementSpeed * Time.deltaTime;

        //positive = <-
        //negative = ->
        switch (rotation)
        {
            case 0:
                //no rotation
                //transform.localRotation.Set(0, 0, 0, rotationAngle * Time.deltaTime * rotationSpeed);
                transform.localEulerAngles += new Vector3(0, 0, 0);
                //transform.Rotate(new Vector3(0, 0, 0), rotationAngle * Time.deltaTime * rotationSpeed);
                break;
            case 1:
                //rotate right
                //transform.localRotation.Set(0, 0, -1, rotationAngle * Time.deltaTime * rotationSpeed);
                //transform.localRotation = Quaternion.Euler(0, 0, -1 + rotationAngle * Time.deltaTime * rotationSpeed);
                transform.localEulerAngles += new Vector3(0, 0, -1) * rotationAngle * Time.deltaTime * rotationSpeed;
                //transform.Rotate(new Vector3(0, 0, -1), rotationAngle * Time.deltaTime * rotationSpeed);
                break;
            case 2:
                //rotate left
                //transform.localRotation.Set(0, 0, 1, rotationAngle * Time.deltaTime * rotationSpeed);
                //transform.localRotation = Quaternion.Euler(0, 0, 1 + rotationAngle * Time.deltaTime * rotationSpeed);
                transform.localEulerAngles += new Vector3(0, 0, 1) * rotationAngle * Time.deltaTime * rotationSpeed;
                //transform.Rotate(new Vector3(0, 0, 1), rotationAngle * Time.deltaTime * rotationSpeed);
                break;
            default:
                //no rotation
                //transform.localRotation.Set(0, 0, 0, rotationAngle * Time.deltaTime * rotationSpeed);
                //transform.localRotation = Quaternion.Euler(0, 0, 0);
                transform.localEulerAngles += new Vector3(0, 0, 0);
                //transform.Rotate(new Vector3(0, 0, 0), rotationAngle * Time.deltaTime * rotationSpeed);
                break;
        }

        ps.shoot = shoot;

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


    private void wallDetection()
    {
        Vector2[] positions = new Vector2[4];
        positions[0] = this.transform.parent.Find("Background").Find("Wall1").transform.localPosition;
        positions[1] = this.transform.parent.Find("Background").Find("Wall2").transform.localPosition;
        positions[2] = this.transform.parent.Find("Background").Find("Wall3").transform.localPosition;
        positions[3] = this.transform.parent.Find("Background").Find("Wall4").transform.localPosition;

        for(int i = 0; i < positions.Length; i++)
        {
            float distance = Mathf.Abs(Vector2.Distance(transform.localPosition, positions[i]));

            if(distance < 1.5f)
            {
                AddReward(-0.1f);
            }
        }

    }
    private void obstacleDetection()
    {
        Vector2[] positions = new Vector2[4];
        positions[0] = this.transform.parent.Find("DebriObs").transform.localPosition;
       // positions[1] = this.transform.parent.Find("DebriObs1").transform.localPosition;
        positions[2] = this.transform.parent.Find("DebriObs2").transform.localPosition;
        positions[3] = this.transform.parent.Find("DebriObs3").transform.localPosition;

        for (int i = 0; i < positions.Length; i++)
        {
            float distance = Mathf.Abs(Vector2.Distance(transform.localPosition, positions[i]));

            if (distance < 1f)
            {
                AddReward(-0.3f);
            }
        }

    }


}


