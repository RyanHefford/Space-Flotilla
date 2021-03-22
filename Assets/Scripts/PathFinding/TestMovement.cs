using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{


    public Transform target;

    float speed = 0.5f;
    Vector2[] path;
    int pathIndex = 0;

    public Pathfinding pf;


    private void Start()
    {

        path = pf.findPath((Vector2)transform.position, (Vector2)target.position);
        
        followPath();
    }

    private void followPath()
    {
        Vector2 currentPoint = path[0];

        while (true)
        {
            if((Vector2) transform.position == currentPoint)
            {
                pathIndex++;
                if(pathIndex >= path.Length)
                {
                    //print("FINISHED");
                    //print(transform.position);
                    //print(target.position);
                    //print(pathIndex);
                    //print(path.Length);

                    break;
                }
                currentPoint = path[pathIndex];
            }

            transform.position = Vector2.MoveTowards(transform.position, currentPoint, speed);

        }//end of WHILE loop


    }

}
