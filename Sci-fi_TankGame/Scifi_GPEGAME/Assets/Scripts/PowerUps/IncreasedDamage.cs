using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreasedDamage : MonoBehaviour
{
    public float modifier;
    public float duration;
    public bool isPermanent;


    void OnTriggerEnter(Collider other)
    {
        PawnData pawn = other.gameObject.GetComponent<PawnData>();

        GameManager.instance.PowerUps.Remove(gameObject);
        GameManager.instance.availableforPUSpawn.Add(gameObject.transform);

        Destroy(gameObject);

        pawn.IncreasedDamage(modifier, duration, isPermanent);

       

    }
}
