using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{


    public Transform target;

    float speed = 10f;
    Vector2[] path;
    int targetIndex;


    private void Start()
    {

        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    public void OnPathFound(Vector2[] newPath, bool pathSuccessful)
    { 

        if(pathSuccessful)
        {
            targetIndex = 0;
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }


    }

    IEnumerator FollowPath()
    {
        Vector2 currentWaypoint = path[0];

       

        while (true)
        {
           
            if ((Vector2)transform.position == currentWaypoint)
            {
                targetIndex++;

                if(targetIndex >= path.Length)
                {
                    yield break;
                }

                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);

            float angle = Mathf.Atan2(currentWaypoint.y - transform.position.y, currentWaypoint.x - transform.position.x) * Mathf.Rad2Deg;

            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 50 * Time.deltaTime);



            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if(path != null)
        {
            for(int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(path[i], Vector2.one);

                if(i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }


}
