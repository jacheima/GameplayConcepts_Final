using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PawnData : MonoBehaviour
{
    public PawnMover mover;
    public bulletMover bulletMover;
    public float moveSpeed;
    public float oldMoveSpeed;
    public float rotateSpeed;

    public float health = 100;
    public float oldHealth;

    public int lives = 4;
    public int score = 0;

    public float damage = 25;
    public float oldDamage;

    public bool speedBoosted = false;
    public bool damagedIncreased = false;
    public bool healthBoosted = false;

    public float shootWait = 3;

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

    //HealthBoost
    public float HealthBoostStartTime;
    public float healthModifier;
    public float healthDuration;
    public bool increasedHealthForever;


    //UI Variables
    public HealthBar healthBar;
    public Lives livesKeeper;
    public Score scoreKeeper;

    //Audio Variables
    public AudioSource PowerUp;

    public AudioClip powerup;

    //GameOver variables
    public GameObject GameOverScreenP1;
    public GameObject GameOverScreenP2;
    public GameObject Lives;
    public GameObject Score;
    public GameObject Timer;
    public GameObject HealthUI;
    public Button ContinueButton;

    void Start()
    {
        PowerUp = GameObject.Find("PowerUpAudio").GetComponent<AudioSource>();
    }

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

        if (healthBoosted)
        {
            if (!increasedHealthForever)
            {
                if (Time.time >= HealthBoostStartTime + healthDuration)
                {
                    healthBoosted = false;
                    health = oldHealth;
                }
            }
        }

        healthBar.SetHealth(health);

       
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
            PowerUp.Play();
            speedBoosted = true;
        }

    }

    public void IncreasedDamage(float modifier, float duration, bool isPermanent)
    {
        IncreasedDamageStartTime = Time.time;

        oldDamage = damage;

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
            PowerUp.Play();
            damagedIncreased = true;
        }
    }

    public void IncreasedHealth(float modifier, float duration, bool isPermanent)
    {
        HealthBoostStartTime = Time.time;

        oldHealth = health;

        healthModifier = modifier;
        healthDuration = duration;

        if (isPermanent)
        {
            increasedHealthForever = true;
            health = health * healthModifier;
        }

        if (healthBoosted)
        {
            health = health * 1;
        }

        if (!healthBoosted)
        {
            health = health * healthModifier;
            PowerUp.Play();
            healthBoosted = true;
        }


    }

    public void GameOver(GameObject player)
    {
        
       

        if (lives <= 0)
        {
            Score = GameObject.Find("ScoreKeeper");
            Lives = GameObject.Find("LivesKeeper");
            Timer = GameObject.Find("GameTimer");
            

            if (player.tag == "Player1")
            {
                GameOverScreenP1 = GameObject.Find("GameOver1");
                HealthUI = GameObject.Find("P1HealthBar");
                

                GameOverScreenP1.SetActive(true);

                if (GameManager.instance.multiplayer)
                {
                    if (GameManager.instance.Players[1].GetComponent<PawnData>().health < 0)
                    {
                        ContinueButton = GameObject.Find("GameOver1").GetComponent<Button>();
                        ContinueButton.enabled = true;
                    }
                    else
                    {
                        ContinueButton.enabled = false;
                    }
                }

                Text finalScore = GameObject.Find("finalScore1").GetComponent<Text>();
                Text finalTime = GameObject.Find("finalTime1").GetComponent<Text>();

                //TODO: See if the gameover screen works, and set and save hi score

                finalScore.text = (player.GetComponent<PawnData>().score).ToString();
                finalTime.text = (player.GetComponent<GameTimer>().minutes + ":" + player.GetComponent<GameTimer>().seconds).ToString();

                Score.GetComponent<Score>().scoreTextp1.enabled = false;
                Lives.GetComponent<Lives>().livesTextP1.enabled = false;
                Timer.GetComponent<GameTimer>().timerTextp1.enabled = false;
                HealthUI.SetActive(false);
            }

            if (player.tag == "Player2")
            {
                GameOverScreenP2 = GameObject.Find("GameOver2");
                GameOverScreenP2.SetActive(true);

                if (GameManager.instance.multiplayer)
                {
                    if (GameManager.instance.Players[0].GetComponent<PawnData>().health < 0)
                    {
                        ContinueButton = GameObject.Find("GameOver1").GetComponent<Button>();
                        ContinueButton.enabled = true;
                    }
                    else
                    {
                        ContinueButton.enabled = false;
                    }
                }

                Text finalScore = GameObject.Find("finalScore2").GetComponent<Text>();
                Text finalTime = GameObject.Find("finalTime2").GetComponent<Text>();

                finalScore.text = (player.GetComponent<PawnData>().score).ToString();
                finalTime.text = (player.GetComponent<GameTimer>().minutes + ":" + player.GetComponent<GameTimer>().seconds).ToString();

                Score.GetComponent<Score>().scoreTextp1.enabled = false;
                Lives.GetComponent<Lives>().livesTextP1.enabled = false;
                Timer.GetComponent<GameTimer>().timerTextp1.enabled = false;
                HealthUI.SetActive(false);
            }
            
        }
    }
}