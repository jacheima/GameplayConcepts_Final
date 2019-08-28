using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public float duration = 10f;
    public float modifier = 1.5f;
    public bool isPermanent = false;



    void OnTriggerEnter(Collider other)
    {
        //Get the pawn data of the person who collided with the powerup
        PawnData pawn = other.gameObject.GetComponent<PawnData>();

        GameManager.instance.PowerUps.Remove(gameObject);
        GameManager.instance.availableforPUSpawn.Add(gameObject.transform);

        //destroy the powerup
        Destroy(gameObject);

        //boost the speed
        pawn.SpeedBoost(modifier, duration, isPermanent);
    }
}
