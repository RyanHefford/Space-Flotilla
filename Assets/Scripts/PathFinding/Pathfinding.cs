﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        findPath(seeker.position, target.position);
    }

    //Doing A* here
    void findPath(Vector2 startPosition, Vector2 targetPosition)
    {
        Node startNode = grid.getNodeFromWorldPoint(startPosition);
        Node targetNode = grid.getNodeFromWorldPoint(targetPosition);


        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();


        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            //finding the node with the lowest fCost.

            Node currentNode = openSet[0];
            for(int i = 1; i < openSet.Count; i++)
            {
                if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            //reached the goal.
            if(currentNode == targetNode)
            {
                getPath(startNode, targetNode);
                return;
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


    }

    //Getting the path from the end to the start (retracing it)
    private void getPath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();

        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        grid.path = path;

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
