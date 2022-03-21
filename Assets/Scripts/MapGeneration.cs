using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class MapGeneration : MonoBehaviour
{
    //this multi dimentional array is used to check if a tile is already occupied (true = occupied)
    private bool[,] tileMap;
    //variable used to dictate spawn rate of terrain
    private float tileSpawnChance = 0.05f;
    private int verticalTiles;
    private int horizontalTiles;
    public GameObject[] singleTileObjects;
    public GameObject[] doubleHorizontalObjects;
    public GameObject[] doubleVerticalObjects;
    public GameObject[] quadTileObjects;
    private MapScript map;
    private GameObject[] currObstacles;


    // Start is called before the first frame update
    void Awake()
    {
        map = GetComponent<MapScript>();

        verticalTiles = (int)math.floor(map.getNorthEdge() * 2) / 10;
        horizontalTiles = (int)math.floor(map.getEastEdge() * 2) / 10;

        verticalTiles -= 1;
        horizontalTiles -= 1;

        tileMap = new bool[verticalTiles,horizontalTiles];
        //setting corners to true to make space for portals
        tileMap[0, 0] = true;
        tileMap[0, 1] = true;
        tileMap[1, 0] = true;
        tileMap[1, 1] = true;

        tileMap[verticalTiles-1, 0] = true;
        tileMap[verticalTiles-1, 1] = true;
        tileMap[verticalTiles-2, 0] = true;
        tileMap[verticalTiles-2, 1] = true;


        tileMap[verticalTiles - 1, horizontalTiles-1] = true;
        tileMap[verticalTiles - 1, horizontalTiles - 2] = true;
        tileMap[verticalTiles - 2, horizontalTiles - 1] = true;
        tileMap[verticalTiles - 2, horizontalTiles -2] = true;

        tileMap[0, horizontalTiles - 1] = true;
        tileMap[0, horizontalTiles - 2] = true;
        tileMap[1, horizontalTiles - 1] = true;
        tileMap[1, horizontalTiles - 2] = true;

        tileMap[verticalTiles / 2, horizontalTiles / 2] = true;
        tileMap[verticalTiles / 2, horizontalTiles / 2 -1] = true;
        tileMap[verticalTiles / 2 - 1, horizontalTiles / 2 - 1] = true;
        tileMap[verticalTiles / 2 - 1, horizontalTiles / 2] = true;

        CreateMap();
        currObstacles = CreateCurrObstacleList();
        //Scanning the grid for obstacles.

        DoDelayAction(0.5f);

    }

    private void CreateMap()
    {
        for (int i= 0; i < verticalTiles; i++)
        {
            for (int j = 0; j < horizontalTiles; j++)
            {
                //check if space is free
                if (!tileMap[i,j])
                {
                    //decide if placing a tile
                    if (UnityEngine.Random.Range(0, 1f) < tileSpawnChance)
                    {
                        bool tileFound = false;
                        while (!tileFound)
                        {
                            //decides what shape tile
                            int tileShape = UnityEngine.Random.Range(1, 5);
                            GameObject tempObject;
                            
                            switch (tileShape)
                            {
                                //single tile
                                case 1:
                                    tempObject = Instantiate(singleTileObjects[UnityEngine.Random.Range(0, singleTileObjects.Length)]);
                                    tempObject.transform.SetPositionAndRotation(new Vector3((j + 1) * 10 - (horizontalTiles / 2) * 10 - 5, (verticalTiles / 2) * 10 - 5 - (i) * 10), Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 4) * 90));

                                    tileMap[i, j] = true;

                                    tileFound = true;

                                    tempObject.layer = LayerMask.NameToLayer("Obstacle");
                                    updateChildrenLayer(tempObject);
                                    break;
                                //double horizontal
                                case 2:
                                    if (j + 1 < horizontalTiles && !tileMap[i, j + 1])
                                    {
                                        tempObject = Instantiate(doubleHorizontalObjects[UnityEngine.Random.Range(0, doubleHorizontalObjects.Length)]);
                                        tempObject.transform.SetPositionAndRotation(new Vector3((j + 1) * 10 - (horizontalTiles / 2) * 10 - 5, (verticalTiles / 2) * 10 - 5 - (i) * 10), Quaternion.Euler(0, 0, 0));

                                        tileMap[i, j] = true;
                                        tileMap[i, j + 1] = true;

                                        tileFound = true;
                                        tempObject.layer = LayerMask.NameToLayer("Obstacle");
                                        updateChildrenLayer(tempObject);
                                    }
                                    break;
                                //double vertical
                                case 3:
                                    if (i + 1 < verticalTiles && !tileMap[i + 1, j])
                                    {
                                        tempObject = Instantiate(doubleVerticalObjects[UnityEngine.Random.Range(0, doubleVerticalObjects.Length)]);
                                        tempObject.transform.SetPositionAndRotation(new Vector3((j + 1) * 10 - (horizontalTiles / 2) * 10 - 5, (verticalTiles / 2) * 10 - 5 - (i) * 10), Quaternion.Euler(0, 0, 0));

                                        tileMap[i, j] = true;
                                        tileMap[i + 1, j] = true;

                                        tileFound = true;
                                        tempObject.layer = LayerMask.NameToLayer("Obstacle");
                                        updateChildrenLayer(tempObject);
                                    }
                                    
                                    break;
                                //quad tile
                                case 4:
                                    if (j + 1 < horizontalTiles && i + 1 < verticalTiles && !tileMap[i + 1, j] && !tileMap[i, j + 1] && !tileMap[i + 1, j + 1])
                                    {
                                        tempObject = Instantiate(quadTileObjects[UnityEngine.Random.Range(0, quadTileObjects.Length)]);
                                        tempObject.transform.SetPositionAndRotation(new Vector3((j + 1) * 10 - (horizontalTiles / 2) * 10 - 5, (verticalTiles / 2) * 10 - 5 - (i) * 10), Quaternion.Euler(0, 0, 0));

                                        tileMap[i, j] = true;
                                        tileMap[i + 1, j] = true;
                                        tileMap[i, j + 1] = true;
                                        tileMap[i + 1, j + 1] = true;

                                        tileFound = true;
                                        tempObject.layer = LayerMask.NameToLayer("Obstacle");
                                        updateChildrenLayer(tempObject);
                                    }
                                    break;
                            }
                        }
                        
                    }
                }
            }
        }

    }

    private void updateChildrenLayer(GameObject go)
    {
        for(int i = 0; i < go.transform.childCount; i++)
        {
            go.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Obstacle");
        }
    }

    void DoDelayAction(float delayTime)
    {
        StartCoroutine(DelayAction(delayTime));
    }

    IEnumerator DelayAction(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);

        //Do the action after the delay time has finished.
        AstarPath.active.Scan();
    }

    private GameObject[] CreateCurrObstacleList()
    {
        int targetLayer = LayerMask.NameToLayer("Obstacle");
        List<GameObject> goList = new System.Collections.Generic.List<GameObject>();
        GameObject[] goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        for(int i = 0; i < goArray.Length; i++)
        {
            if (goArray[i].GetComponent<PolygonCollider2D>() != null && goArray[i].layer == targetLayer)
            {
                goList.Add(goArray[i]);
            }
        }
        if (goList.Count == 0)
        {
            return null;
        }
        return goList.ToArray();
    }

    public GameObject[] getObstacleList()
    {
        return currObstacles;
    }
}
