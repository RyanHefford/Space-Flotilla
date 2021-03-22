using System;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Pathfinding : MonoBehaviour
{

    Grid grid;

    public Transform seeker, target;



    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            findPath(seeker.position, target.position);
        }
        
    }

    //Doing A* here
    public Vector2[] findPath(Vector2 startPosition, Vector2 targetPosition)
    {

        Stopwatch sw = new Stopwatch();

        sw.Start();


        Vector2[] finalPath = null;

        Node startNode = grid.getNodeFromWorldPoint(startPosition);
        Node targetNode = grid.getNodeFromWorldPoint(targetPosition);


        //List<Node> openSet = new List<Node>();
        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();


        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            //finding the node with the lowest fCost.

            //Node currentNode = openSet[0];
            //for(int i = 1; i < openSet.Count; i++)
            //{
            //    if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
            //    {
            //        currentNode = openSet[i];
            //    }
            //}

            //openSet.Remove(currentNode);
            Node currentNode = openSet.RemoveFirst();


            closedSet.Add(currentNode);

            //reached the goal.
            if(currentNode == targetNode)
            {
                sw.Stop();
                print("Path found: " + sw.ElapsedMilliseconds + " ms");
                finalPath = getPath(startNode, targetNode);
                
                break;
            }

            foreach (Node neighbour in grid.getNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }


                int newMovementCostToNeighbour = currentNode.gCost + getDistance(currentNode, neighbour);
                if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = getDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }

            }

        }


        return finalPath;

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



        Vector2[] pathWaypoints = SimplifyPath(path);

        Array.Reverse(pathWaypoints);

        path.Reverse();
        grid.path = path;

        return pathWaypoints;

    }
    //returns the transform.position of where the player/AI will be heading to after doing A*
    Vector2[] SimplifyPath(List<Node> path)
    {
        List<Vector2> waypoints = new List<Vector2>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }


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
