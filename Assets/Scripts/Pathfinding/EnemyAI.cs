using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{



    public Transform target;

    public float speed = 300f;

    //how close the enemy needs to be to a waypoint before moving to the next one.
    public float nextWaypointDistance = 2f;

    //current path we are following
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    //public Transform enemyGFX;

    //TESTING
    private Vector2 direction;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        //update the path every 0.5f seconds
        //InvokeRepeating("updatePath", 0f, 0.5f);
        InvokeRepeating("updatePath", 0f, 5f);

    }

    void updatePath()
    {
        //not calculating a path then calculate a new one.
        if (seeker.IsDone())
        {
            if(target == null)
            {
                print("Null");
            }
            seeker.StartPath(rb.position, target.position, onPathCompleted);
        }
        
    }
    private List<Vector3> testV = new List<Vector3>();
    void onPathCompleted(Path p)
    {
        //make sure no error
        if (!p.error)
        {
            //path smoothing.
            //p.vectorPath = pathSmoothing(p);

            testV = pathSmoothing(p);



            //current path = the newly generated path
            path = p;
            p.vectorPath = testV;
            currentWaypoint = 0;
        }
    }

    

    void OnDrawGizmos()
    {

        // Draws a blue line from this transform to the target

        for (int i = 0; i < testV.Count; i += 2)
        {
            Gizmos.color = Color.cyan;

            if(testV.Count % 2 == 0)
            {
                Gizmos.DrawLine(testV[i], testV[i + 1]);
            }
            else
            {
                if(i+1 == testV.Count)
                {
                    Gizmos.DrawLine(testV[i-1], testV[i]);
                }
                else
                {
                    Gizmos.DrawLine(testV[i], testV[i + 1]);
                }
            }
            

        }

        //for (int i = 0; i < path.vectorPath.Count; i += 2)
        //{
        //    Gizmos.color = Color.gray;
        //    Gizmos.DrawLine(path.vectorPath[i], path.vectorPath[i + 1]);

        //}

    }

    private List<Vector3> pathSmoothing(Path p)
    {
        Vector3 currentPosition = rb.transform.position;
        List<Vector3> newVectorPath = new List<Vector3>();
        Vector3 previousPosition = Vector3.zero;

        //adding the start.
        newVectorPath.Add(currentPosition);
        previousPosition = currentPosition;
        int count = 0;
        //print("Vector path count: " + p.vectorPath.Count);
        //foreach (Vector2 nextWaypoint in p.vectorPath)
        //{
        //    //if(previousPosition == nextWaypoint && count == 0)
        //    //{
        //    //    count = 1;
        //    //    continue;
                
        //    //}

        //    float distance = Vector2.Distance(currentPosition, nextWaypoint);

        //    Vector2 to = ((Vector2)nextWaypoint - currentPosition);

        //    Debug.DrawRay(currentPosition, to, Color.red, 10.0f);
        //    RaycastHit2D hit = Physics2D.Raycast(currentPosition, to, distance, 1 << LayerMask.NameToLayer("Obstacle"));

        //    if (hit)
        //    {
        //        //we hit something:
        //        print("HIT HIT HIT");
        //        print(hit.transform.gameObject.name);
        //        newVectorPath.Add(previousPosition);
        //        currentPosition = previousPosition;

        //    }
        //    previousPosition = nextWaypoint;



        //}

        for(int i = 0; i < p.vectorPath.Count; i++)
        {
            Vector3 nextWaypoint = p.vectorPath[i];

            float distance = Vector2.Distance(currentPosition, nextWaypoint);

            Vector3 to = (nextWaypoint - currentPosition);

            //Debug.DrawRay(currentPosition, to, Color.red, 10.0f);
            RaycastHit2D hit = Physics2D.Raycast(currentPosition, to, distance, 1 << LayerMask.NameToLayer("Obstacle"));

            if (hit)
            {
                //we hit something:
                print("HIT HIT HIT");
                print(hit.transform.gameObject.name);
                newVectorPath.Add(previousPosition);
                currentPosition = previousPosition;
                i -= 1;

            }
            previousPosition = nextWaypoint;

        }


        // add the target at the end.
        newVectorPath.Add(target.position);

        

        return newVectorPath;
    }


    private void FixedUpdate()
    {

        if(path == null)
        {
            return;
        }

        //make sure we haven't reached the end.
        //path.vectorPath.Count => The total amount of waypoints in our path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }else
        {
            reachedEndOfPath = false;
        }

        //A vector from our current position to the next position.
        //Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

    }

    public Vector2 getDirection()
    {
        return direction;
    }

}
