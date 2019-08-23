using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnData : MonoBehaviour
{
    public PawnMover mover;

    public float moveSpeed;
    public float oldMoveSpeed;
    public float rotateSpeed;

    public int health = 25;
    public int score = 0;

    public float damage;
    public float oldDamage;

    public bool speedBoosted = false;
    public bool damagedIncreased = false;

    //Shooting Variables
    public GameObject bulletSpawn;
    public GameObject bulletPrefab;

    //Speed Boost Variables
    public float speedBoostStartTime;
    public float speedModifier;
    public float speedDuration;
    public bool speedBoostForever = false;

    //Increased Damage Variables
    public float IncreasedDamageStartTime;
    public float damageModifier;
    public float damageDuration;
    public bool increasedDamgeForever;

    void Update()
    {
        if (speedBoosted)
        {
            if(!speedBoostForever)
            {
                if (Time.time >= speedBoostStartTime + speedDuration)
                {
                    speedBoosted = false;
                    moveSpeed = oldMoveSpeed;
                }
            }
        }

        if (damagedIncreased)
        {
            if (!increasedDamgeForever)
            {
                if (Time.time >= IncreasedDamageStartTime + damageDuration)
                {
                    damagedIncreased = false;
                    damage = oldDamage;
                }
            }
        }
    }
    public void SpeedBoost(float modifier, float duration, bool isPermanent)
    {
        speedBoostStartTime = Time.time;

        speedModifier = modifier;
        speedDuration = duration;

        if (isPermanent)
        {
            speedBoostForever = true;
            moveSpeed = moveSpeed * modifier;
        }

        oldMoveSpeed = moveSpeed;

        if (speedBoosted)
        {
            moveSpeed = moveSpeed * 1;
        }

        if (!speedBoosted)
        {
            moveSpeed = moveSpeed * modifier;
            speedBoosted = true;
        }

    }

    public void IncreasedDamage(float modifier, float duration, bool isPermanent)
    {
        IncreasedDamageStartTime = Time.time;

        damageModifier = modifier;
        damageDuration = duration;

        if (isPermanent)
        {
            increasedDamgeForever = true;
            damage = damage * damageModifier;
        }

        damage = oldDamage;

        if (damagedIncreased)
        {
            damage = damage * 1;
        }

        if (!damagedIncreased)
        {
            damage = damage * damageModifier;
            damagedIncreased = true;
        }
    }
}