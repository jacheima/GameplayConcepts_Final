using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float startTime;
    public float statBoost;
    public float duration;

    public string name;
    


    void OnTriggerEnter(Collider other)
    {
        //Get the pawn data of the person who collided with the powerup
        PawnData pawn = other.gameObject.GetComponent<PawnData>();

        //set the start time to the time of collision
        startTime = Time.time;
        
        //save the current move speed
        float speed = pawn.moveSpeed;
        //set the new move speed to the old move speed times the stat boost
        pawn.moveSpeed = pawn.moveSpeed * statBoost;
        //if the time is greater or equal to the start time plus the duration of the powerup
        if (Time.time >= startTime + duration)
        {
            //set the move speed back to the old move speed
            pawn.moveSpeed = speed;
        }
    }
}
