using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{



    public Transform target;

    public float speed = 600f;

    //how close the enemy needs to be to a waypoint before moving to the next one.
    public float nextWaypointDistance = 3f;

    //current path we are following
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    public Transform enemyGFX;

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
            seeker.StartPath(rb.position, target.position, onPathCompleted);
        }
        
    }

    void onPathCompleted(Path p)
    {
        //make sure no error
        if (!p.error)
        {
            //current path = the newly generated path
            path = p;
            currentWaypoint = 0;
        }
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
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

    }

}
