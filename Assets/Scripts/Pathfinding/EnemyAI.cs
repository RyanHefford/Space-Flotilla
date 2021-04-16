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
    private List<Vector2> testV = new List<Vector2>();
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
            currentWaypoint = 0;
        }
    }

    

    void OnDrawGizmos()
    {

        // Draws a blue line from this transform to the target

        for (int i = 0; i < testV.Count; i += 2)
        {
            Gizmos.color = Color.cyan;
            print(i);

            if(testV.Count % 2 != 0)
            {
                Gizmos.DrawLine(testV[i], testV[i + 1]);
            }
            else
            {
                if(i == testV.Count)
                {
                    Gizmos.DrawLine(testV[i-2], testV[i - 1]);
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

    private List<Vector2> pathSmoothing(Path p)
    {
        Vector2 currentPosition = rb.transform.position;
        List<Vector2> newVectorPath = new List<Vector2>();
        Vector2 previousPosition = Vector2.zero;

        //adding the start.
        newVectorPath.Add(currentPosition);
        previousPosition = currentPosition;
        int count = 0;
        print("Vector path count: " + p.vectorPath.Count);
        foreach (Vector2 nextWaypoint in p.vectorPath)
        {
            if(previousPosition == nextWaypoint && count == 0)
            {
                count = 1;
                continue;
                
            }
            float distance = Vector2.Distance(currentPosition, target.position);
            print(distance);
            //Debug.DrawRay(currentPosition, ((Vector2)target.position - currentPosition) * 10, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(currentPosition, ((Vector2)target.position - currentPosition), distance, 1 << LayerMask.NameToLayer("Obstacle"));

            if (hit)
            {
                //we hit something:
                print("HIT HIT HIT");
                print(hit.transform.gameObject.name);
                newVectorPath.Add(previousPosition);
                currentPosition = previousPosition;

            }
            previousPosition = nextWaypoint;



        }
        // add the target at the end.
        newVectorPath.Add(target.position);

        print(newVectorPath.Count);

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

        //rb.AddForce(force);

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
