using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    //this multi dimentional array is used to check if a tile is already occupied (true = occupied)
    private bool[,] tileMap;
    //variable used to dictate spawn rate of terrain
    private float tileSpawnChance = 0.1f;
    private int verticalTiles;
    private int horizontalTiles;
    public GameObject[] singleTileObjects;
    public GameObject[] doubleHorizontalObjects;
    public GameObject[] doubleVerticalObjects;
    public GameObject[] quadTileObjects;
    private MapScript map;

    // Start is called before the first frame update
    void Start()
    {
        map = GetComponent<MapScript>();

        verticalTiles = (int)math.floor(map.getNorthEdge() * 2) / 10;
        horizontalTiles = (int)math.floor(map.getEastEdge() * 2) / 10;

        tileMap = new bool[verticalTiles,horizontalTiles];
        CreateMap();

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
                                    }
                                    break;
                            }
                        }
                        
                    }
                }
            }
        }
    }
}
