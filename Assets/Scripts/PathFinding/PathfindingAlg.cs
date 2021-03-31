using System;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Collections;

public class PathfindingAlg : MonoBehaviour
{
    PathRequestManager requestManager;


    Grid grid;

    private void Awake()
    {
        grid = GetComponent<Grid>();
        requestManager = GetComponent<PathRequestManager>();
    }

    public void startFindPath(Vector2 startPos, Vector2 targetPos)
    {
        //print("hi1");
        StartCoroutine(findPath(startPos, targetPos));
    }

    //Doing A* here
    IEnumerator findPath(Vector2 startPosition, Vector2 targetPosition)
    {

        Stopwatch sw = new Stopwatch();

        sw.Start();


        Vector2[] waypoints = new Vector2[0];
        bool pathSuccess = false;

        Node startNode = grid.getNodeFromWorldPoint(startPosition);
        Node targetNode = grid.getNodeFromWorldPoint(targetPosition);

        if(startNode.walkable && targetNode.walkable)
        {
            //List<Node> openSet = new List<Node>();
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            //print("hi2");

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                //finding the node with the lowest fCost.


                Node currentNode = openSet.RemoveFirst();


                closedSet.Add(currentNode);

                //reached the goal.
                if (currentNode == targetNode)
                {
                    sw.Stop();
                    print("Path found: " + sw.ElapsedMilliseconds + " ms");
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbour in grid.getNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }


                    int newMovementCostToNeighbour = currentNode.gCost + getDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = getDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbour);
                        }
                    }

                }

            }
        }


        //wait for 1 frame before returning.
        yield return null;

        if (pathSuccess)
        {
            waypoints = getPath(startNode, targetNode);
        }

        requestManager.FinishedProcessingPath(waypoints, pathSuccess);

    }

    //Getting the path from the end to the start (retracing it)
    private Vector2[] getPath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();

        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector2[] waypoints = SimplifyPath(path);

        Array.Reverse(waypoints);


        return waypoints;

    }
    //returns the transform.position of where the player/AI will be heading to after doing A*
    Vector2[] SimplifyPath(List<Node> path)
    {
        List<Vector2> waypoints = new List<Vector2>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            //Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            //if (directionNew != directionOld)
            //{
                waypoints.Add(path[i].worldPosition);
            //}
            //directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    //Heuristic
    private int getDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        else
        {
            return 14 * distX + 10 * (distY - distX);
        }

    }



}
