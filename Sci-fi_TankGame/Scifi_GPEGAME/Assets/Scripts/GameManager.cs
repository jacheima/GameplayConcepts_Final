﻿using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Transform> PlayerSpawnPositions;
    public List<Transform> EnemySpawnPositions;
    public List<Transform> PowerUpSpawnPositions;
    public List<Transform> waypoints;

    public List<GameObject> Enemies;
    public List<GameObject> Players;
    public List<GameObject> PowerUps;

    public GameObject enemyPrefab;
    public GameObject player1Prefab;
    //public GameObject player2Prefab;
    public GameObject speedBoostPrefab;

    public GameObject enemyParent;

    public bool singlePlayer;
    //public bool multiplayer;

    //Static instance of GameManager which allows it to be accessed by any other script.
    public static GameManager instance = null;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {

            //if not, set instance to this
            instance = this;
        }
        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    public void SpawnEnemies()
    {

        //for all the enemy spawn positions
        for (int i = 0; i < EnemySpawnPositions.Count; i++)
        {
            //get the tile script attached to the parent of the enemy spawn position
            Tile tileScript = EnemySpawnPositions[i].GetComponentInParent<Tile>();

            //Spawn Enmey
            GameObject enemy = Instantiate(enemyPrefab, EnemySpawnPositions[i].position,
                EnemySpawnPositions[i].rotation);

            //set the enemies name
            enemy.name = "DanTheCroc" + "_" + (i + 1);

            //parent the enemies to the enemy gameObject
            enemy.GetComponent<Transform>().parent = enemyParent.GetComponent<Transform>();

            //Add the enemy to the list of enemies currently in the world
            Enemies.Add(enemy);

            //call the get waypoints function in the Tile script to assign each enemy all the waypoints of the tile they spawned on
            tileScript.GetWaypoints(enemy);
        }
    }

    public void SpawnPlayers()
    {

        //choose a random spawn position from the player spawn point list
        int randomPlayer1Spawn = Random.Range(0, PlayerSpawnPositions.Count);

        //Instantiate player 1 at that location
        GameObject player1 = Instantiate(player1Prefab, PlayerSpawnPositions[randomPlayer1Spawn].position,
            PlayerSpawnPositions[randomPlayer1Spawn].rotation);

        player1.name = "Player One";

        Players.Add(player1);

        ////if the game is a multi-player player game
        //if (singlePlayer == false && multiplayer == true)
        //{
        //    //choose a random spawn position from the player spawn point list
        //    int randomPlayer2Spawn = Random.Range(0, PlayerSpawnPositions.Count);

        //    //Instantiate player 1 at that location
        //    GameObject player2 = Instantiate(player1Prefab, PlayerSpawnPositions[randomPlayer2Spawn].position,
        //        PlayerSpawnPositions[randomPlayer2Spawn].rotation);
        //}
        
    }

    //TODO: The game isn't spawning power-ups in the level, there are no errors in the console. I need to figure out what is happening and get it to spawn the power-ups
    public void SpawnPowerUps()
    {
        if (PowerUps.Count > 5)
        {
            int randomPowerUpSpawn = Random.Range(0, PowerUpSpawnPositions.Count);

            GameObject powerUp = Instantiate(speedBoostPrefab,
                PowerUpSpawnPositions[randomPowerUpSpawn].transform.position,
                PowerUpSpawnPositions[randomPowerUpSpawn].transform.rotation);

            PowerUps.Add(powerUp);
        }
    }
}
