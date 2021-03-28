using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    //public Transform player;


    //public bool displayGridGizmos;

    //public LayerMask unwalkableMask;
    //public Vector2 gridWorldSize;
    //public float nodeRadius;
    ////representing the nodes
    //Node[,] grid;

    //float nodeDiameter;
    //int gridSizeX, gridSizeY;

    //private void Awake()
    //{
    //    //nodes to fit in the grid based on the node radius
    //    nodeDiameter = nodeRadius * 2;
    //    gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
    //    gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

    //    createGrid();

    //    displayGridGizmos = true;

    //}
    //public int MaxSize
    //{
    //    get
    //    {
    //        return gridSizeX * gridSizeY;
    //    }
    //}

    //private void createGrid()
    //{
    //    grid = new Node[gridSizeX, gridSizeY];
    //    Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * gridWorldSize.x / 2 - Vector2.up * gridWorldSize.y / 2;
    //    for (int x = 0; x < gridSizeX; x++)
    //    {
    //        for (int y = 0; y < gridSizeY; y++)
    //        {
    //            Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);

    //            //collision check

    //            bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
    //            grid[x, y] = new Node(walkable, worldPoint, x, y);


    //        }
    //    }
    //}

    //public Node getNodeFromWorldPoint(Vector2 worldPosition)
    //{
    //    //Converting the player's position into a Node

    //    float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
    //    float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;

    //    percentX = Mathf.Clamp01(percentX);
    //    percentY = Mathf.Clamp01(percentY);

    //    //get the x and y
    //    int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
    //    int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

    //    return grid[x, y];
    //}


    //public List<Node> getNeighbours(Node node)
    //{
    //    List<Node> neighbours = new List<Node>();

    //    //searching for neighbours.

    //    for (int x = -1; x <= 1; x++)
    //    {
    //        for (int y = -1; y <= 1; y++)
    //        {
    //            if (x == 0 && y == 0)
    //            {
    //                continue;
    //            }
    //            else
    //            {
    //                int checkX = node.gridX + x;
    //                int checkY = node.gridY + y;

    //                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
    //                {
    //                    neighbours.Add(grid[checkX, checkY]);
    //                }
    //            }
    //        }
    //    }



    //    return neighbours;

    //}


    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));


    //    if (grid != null && displayGridGizmos)
    //    {
    //        foreach (Node n in grid)
    //        {

    //            Node playerNode = getNodeFromWorldPoint((Vector2)player.position);


    //            Gizmos.color = (n.walkable) ? Color.white : Color.red;
    //            if (playerNode == n)
    //            {
    //                Gizmos.color = Color.cyan;
    //            }


    //            Gizmos.DrawCube(n.worldPosition, Vector2.one * (nodeDiameter - 0.1f));
    //        }
    //    }



    //}



    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        createGrid();
    }

    private void Update()
    {
        //updating the obstacle locations
        checkNewObstacleLocation();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    private void createGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * gridWorldSize.x / 2 - Vector2.up * gridWorldSize.y / 2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);

                //collision check

                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);


            }
        }
    }

    private void checkNewObstacleLocation()
    {
        Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * gridWorldSize.x / 2 - Vector2.up * gridWorldSize.y / 2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);

                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);

            }
        }
        
    }

    public Node getNodeFromWorldPoint(Vector2 worldPosition)
    {
        //Converting the player's position into a Node

        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        //get the x and y
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }


    public List<Node> getNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        //searching for neighbours.

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                else
                {
                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }
        }



        return neighbours;

    }

    public List<Node> path;
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (path != null)
                    if (path.Contains(n))
                        Gizmos.color = Color.black;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }



}
