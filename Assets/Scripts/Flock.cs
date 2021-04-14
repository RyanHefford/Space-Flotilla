using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    public List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehaviour behaviour;

    // populating flock values
    [Range(10, 500)]
    public int startingCount = 250;
    const float AgentDensity = 0.08f;

    // variables we can change with sliders for how the flock behaves
    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 10f)]
    public float neighborRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    // utility variables
    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }


    //for pathfinding:
    private bool startPathfinding = false;

    //For corners and creating an overlord
    private Transform[] corners = new Transform[4];
    private GameObject cornersGO;
    private bool findingCorner = false;
    public OverlordManager om;
    // Start is called before the first frame update
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < startingCount; i++)
        {
            Vector2 newLocation = Random.insideUnitCircle * startingCount * AgentDensity;

            // Returns true if there are any colliders overlapping the sphere defined by position and radius in world coordinates.
            while (Physics2D.OverlapCircle(newLocation, 1.5f) != null)
            {
                // Keep getting new locations until one doesn't overlap.
                newLocation = Random.insideUnitCircle * startingCount * AgentDensity;
            }

            // Then spawn the agent in that location.
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                newLocation,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                transform
                );
            newAgent.name = "Agent " + i;
            agents.Add(newAgent);

            //for pathfinding
            newAgent.GetComponent<EnemyAI>().enabled = false;
        }

        //Get corners game object
        cornersGO = GameObject.Find("Corners");
        //Add children to array
        for (int i = 0; i < cornersGO.transform.childCount; i++)
        {
            corners[i] = cornersGO.transform.GetChild(i).transform;
        }

        print("CHILDREN: " + corners.Length);

    }

    // Update is called once per frame
    void Update()
    {
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);

            //FOR DEMO ONLY
            //agent.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, context.Count / 6f);

            Vector2 move = behaviour.CalculateMove(agent, context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }

            //enable pathfinding.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                agent.nowPathFinding = true;
                startPathfinding = true;
                findingCorner = false;
            }
            //disable the pathfinding behavior:
            if (Input.GetKeyDown(KeyCode.B))
            {
                agent.nowPathFinding = false;
                startPathfinding = false;
                findingCorner = false;

            }



            if (startPathfinding && agent.nowPathFinding && !findingCorner)
            {
                //DO PATHFINDING NOW
                agent.GetComponent<EnemyAI>().enabled = true;
                agent.enemyAI.target = GameObject.Find("Player").transform;

                //move *= agent.enemyAI.getDirection();
                //agent.GetComponent<Rigidbody2D>().rotation += 1f;
                agent.Move(move);



            }
            else if(startPathfinding && agent.nowPathFinding && findingCorner && agent.goToCorner)
            {
                //DO PATHFINDING NOW
                agent.GetComponent<EnemyAI>().enabled = true;

                agent.enemyAI.target = corners[om.randomCorner];
                agent.Move(move);
            }
            else
            {
                agent.GetComponent<EnemyAI>().enabled = false;
                agent.Move(move);
            }


            //If I want random agents to go to corners
            if (Input.GetKeyDown(KeyCode.C))
            {
                agent.nowPathFinding = true;
                startPathfinding = true;
                findingCorner = true;
            }



        }
    }

    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);
        foreach (Collider2D c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }

    

}
