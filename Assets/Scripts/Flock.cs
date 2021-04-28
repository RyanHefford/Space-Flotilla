using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    public FlockAgent overlordPrefab;
    public List<FlockAgent> agents = new List<FlockAgent>();
    public List<FlockAgent> agentsAttacking = new List<FlockAgent>();
    public FlockBehaviour behaviour;

    // populating flock values
    [Range(2, 500)]
    public int startingCount = 30;
    public int replenishAmount = 3;
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

    private int numOfOverlordsStart = 0;
    //for pathfinding:
    private bool startPathfinding = false;

    // for spawn chance with tileMap
    private float agentSpawnChance = 0.7f;

    //For corners and creating an overlord
    private Transform[] corners = new Transform[4];
    private GameObject cornersGO;
    private bool findingCorner = false;
    public OverlordManager om;

    public bool initiatedAnAttack = false;
    public int timerForEachAttack = 3;
    public float attackingTimeLeft = 3;

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
            //while (Physics2D.OverlapCircle(newLocation, 1.25f) != null)
            //{
            // Keep getting new locations until one doesn't overlap.
            newLocation = Random.insideUnitCircle * startingCount * AgentDensity;
            //}

            FlockAgent newAgent = null;

            int verticalTiles = GameObject.Find("Background").GetComponent<MapGeneration>().getVerticleTiles();
            int horizontalTiles = GameObject.Find("Background").GetComponent<MapGeneration>().getHorizontalTiles();

            for (int j = 0; j < verticalTiles; j++) {
                for (int k = 0; k < horizontalTiles; k++) {
                    // Checking if space is free
                    if (!GameObject.Find("Background").GetComponent<MapGeneration>().checkTileMap(j,k)) {
                        // Decide if spawning an agent
                        if (UnityEngine.Random.Range(0, 1f) < agentSpawnChance) {

                            if (numOfOverlordsStart == 0)
                            {
                                // setting spawn location of the current tilemap vector
                                newLocation = new Vector3((k + 1) * 10 - (horizontalTiles / 2) * 10 - 5, (verticalTiles / 2) * 10 - 5 - (j) * 10);

                                newAgent = Instantiate(
                                overlordPrefab,
                                newLocation,
                                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                                transform
                                );
                                newAgent.name = "Overlord";
                                agents.Add(newAgent);

                                newAgent.isOverlord = true;

                                newAgent.GetComponent<EnemyAI>().enabled = false;

                                numOfOverlordsStart = 1;
                            }
                            else
                            {
                                //setting spawnlocation of the current tilemap vector
                                newLocation = new Vector3((k + 1) * 10 - (horizontalTiles / 2) * 10 - 5, (verticalTiles / 2) * 10 - 5 - (j) * 10); ;

                                // Then spawn the agent in that location.
                                newAgent = Instantiate(
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

                        }
                    }
                }
            }
        }

        //Get corners game object
        cornersGO = GameObject.Find("Corners");
        //Add children to array
        for (int i = 0; i < cornersGO.transform.childCount; i++)
        {
            corners[i] = cornersGO.transform.GetChild(i).transform;
        }

        //initializing the overlord manager.
        om = GameObject.Find("Manager").GetComponent<OverlordManager>();

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

            ////enable pathfinding.
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    agent.nowPathFinding = true;
            //    startPathfinding = true;
            //    findingCorner = false;
            //}
            ////disable the pathfinding behavior:
            //if (Input.GetKeyDown(KeyCode.B))
            //{
            //    agent.nowPathFinding = false;
            //    startPathfinding = false;
            //    findingCorner = false;

            //}



            if (agent.nowPathFinding && !agent.goToCorner)
            {
                //DO PATHFINDING NOW TO PLAYER
                agent.GetComponent<EnemyAI>().enabled = true;
                agent.enemyAI.target = GameObject.Find("Player").transform;

                //move *= agent.enemyAI.getDirection();
                //agent.GetComponent<Rigidbody2D>().rotation += 1f;
                //agent.Move(move);



            }

            agent.Move(move);

            ////If I want random agents to go to corners
            if (!om.overlordExists && !om.creatingOverlord)
            {
                //agent.nowPathFinding = true;
                startPathfinding = true;
                findingCorner = true;
            }

            if (attackingTimeLeft > 0.0f)
            {
                attackingTimeLeft -= Time.deltaTime;
            }

            if (attackingTimeLeft < 0.0f)
            {
                //finished attacking
                initiatedAnAttack = false;
                //disable the pathfinding ON the player.
                resetAgentsAttacking();
            }

        }

        if (agents.Count < (startingCount - replenishAmount))
        {
            for (int i = (startingCount - replenishAmount); i < startingCount; i++)
            {

                int verticalTiles = GameObject.Find("Background").GetComponent<MapGeneration>().getVerticleTiles();
                int horizontalTiles = GameObject.Find("Background").GetComponent<MapGeneration>().getHorizontalTiles();

                for (int j = 0; j < verticalTiles; j++)
                {
                    for (int k = 0; k < horizontalTiles; k++)
                    {
                        // Checking if space is free
                        if (!GameObject.Find("Background").GetComponent<MapGeneration>().checkTileMap(j, k))
                        {
                            // Decide if spawning an agent
                            if (UnityEngine.Random.Range(0, 1f) < agentSpawnChance)
                            {
                                Vector2 newLocation = Random.insideUnitCircle * startingCount * AgentDensity;
                                //setting spawnlocation of the current tilemap vector
                                newLocation = new Vector3((k + 1) * 10 - (horizontalTiles / 2) * 10 - 5, (verticalTiles / 2) * 10 - 5 - (j) * 10); ;

                                FlockAgent newAgent = null;

                                newAgent = Instantiate(
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
                        }
                    }
                }
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

    private void resetAgentsAttacking()
    {
        foreach (FlockAgent attackingAgent in agentsAttacking)
        {
            attackingAgent.GetComponent<EnemyAI>().enabled = false;
            attackingAgent.attacking = false;
            attackingAgent.nowPathFinding = false;
        }
    }


}