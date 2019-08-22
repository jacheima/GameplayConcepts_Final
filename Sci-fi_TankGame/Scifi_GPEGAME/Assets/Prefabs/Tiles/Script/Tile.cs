using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject northDoor;
    public GameObject eastDoor;
    public GameObject southDoor;
    public GameObject westDoor;

    public Transform powerUpSpawns;
    public Transform enemySpawns;
    public Transform playerSpawn;

    public List<GameObject> waypoints;

    public void GetWaypoints(GameObject enemy)
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            enemy.GetComponent<DanTheCroc_Controller>().waypoints.Add(waypoints[i]);
        }
    }

}
