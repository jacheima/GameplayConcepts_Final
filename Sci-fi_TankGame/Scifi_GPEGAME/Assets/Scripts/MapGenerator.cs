using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    //Declaring the variables I will be using in the script
    public List<GameObject> tilePrefabs;
    public List<GameObject> mapList;

    public GameObject[,] grid;

    public int columns;
    public int rows;

    public float tileWidth;
    public float tileHeight;

    public bool randomMap;
    public bool levelOfTheDay;

    public int numberOfEnemies;

    public int seed = 0;

    public int convertDatetoInt(DateTime date)
    {
        return date.Day + date.Month + date.Year;
    }

    public int RandomSeed(DateTime time)
    {
        return time.Hour + time.Minute + time.Second + time.Millisecond;
    }


   public GameManager gm = GameManager.instance;

    void Start()
    {
        if (seed == 0)
        {
            Random.InitState(RandomSeed(DateTime.Now));
        }
        else
        {
            Random.InitState(seed);
        }

        if (levelOfTheDay)
        {
            Random.InitState(convertDatetoInt(DateTime.Today));
        }

        if (randomMap)
        {
            Random.InitState(RandomSeed(DateTime.Now));
        }


        CreateMap();
    }

    void CreateMap()
    {
        //create the 2D array
        grid = new GameObject[columns, rows];

        for (int currentCol = 0; currentCol < columns; currentCol++)
        {
            for (int currentRow = 0; currentRow < rows; currentRow++)
            {
                //Instantiate a room
                GameObject room = Instantiate(RandomRoom());

                //Add the room to the grid array
                grid[currentCol, currentRow] = room;

                //Move the room into position
                grid[currentCol, currentRow].transform.position =  new Vector3(currentCol * tileWidth, 0, -currentRow * tileHeight);

                //name the tile
                grid[currentCol, currentRow].name = "Tile_" + currentCol + "_" + currentRow;

                //make the tile a child of this object
                grid[currentCol, currentRow].GetComponent<Transform>().parent = this.gameObject.GetComponent<Transform>();

                //Remove Doors
                Tile tileScript = grid[currentCol, currentRow].GetComponent<Tile>();

               //if the tile is not in the first row
                if (currentRow > 0)
                {
                    tileScript.southDoor.SetActive(false);
                }

                //if the tile is not in the last row
                if (currentRow < rows - 1)
                {
                    tileScript.northDoor.SetActive(false);
                }

                //if the tile is not in the first column
                if (currentCol > 0)
                {
                    tileScript.eastDoor.SetActive(false);
                }

                //if the tile is not in the last column
                if (currentCol < columns - 1)
                {
                    tileScript.westDoor.SetActive(false);
                }

                GameManager.instance.EnemySpawnPositions.Add(tileScript.enemySpawns);
                GameManager.instance.PlayerSpawnPositions.Add(tileScript.playerSpawn);
                GameManager.instance.PowerUpSpawnPositions.Add(tileScript.powerUpSpawns);

                mapList.Add(room);
            }
        }

        GameManager.instance.SpawnEnemies();
        GameManager.instance.SpawnPlayers();
        GameManager.instance.SpawnPowerUps();
    }

    GameObject RandomRoom()
    {
        //Pick a random number between 0 and the number of rooms in the prefab list
        int roomIndex = Random.Range(0, tilePrefabs.Count);
        
        //Return that prefab tile
        return tilePrefabs[roomIndex];
    }


}
