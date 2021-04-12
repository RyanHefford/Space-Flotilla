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
        InvokeRepeating("updatePath", 0f, 0.5f);

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

    void onPathCompleted(Path p)
    {
        //make sure no error
        if (!p.error)
        {
            //path smoothing.
            //p.vectorPath = pathSmoothing(p);

            

            //current path = the newly generated path
            path = p;
            currentWaypoint = 0;
        }
    }

    private List<Vector3> pathSmoothing(Path p)
    {
        Vector3 currentPosition = rb.transform.position;
        List<Vector3> newVectorPath = new List<Vector3>();

        foreach (Vector3 nextWaypoint in p.vectorPath)
        {

            


        }

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
