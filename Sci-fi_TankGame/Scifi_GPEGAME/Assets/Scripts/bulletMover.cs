using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bulletMover : MonoBehaviour
{
    private Rigidbody rb;

    public float gunSpeed = 3.0f;
    public float bulletDestroy = 5.0f;

    public PawnData playerData;

    public Score scoreKeeper;
    public Lives livesKeeper;

    private AudioSource playerDeathAudio;
    private AudioSource enemyDeathAudio;
    private AudioSource bulletAudio;

    public AudioClip bullet;
    public AudioClip playerDeath;
    public AudioClip enemyDeath;

    

    // Start is called before the first frame update
    void Start()
    {
        playerDeathAudio = GameObject.Find("PlayerDeathAudio").GetComponent<AudioSource>();
        enemyDeathAudio = GameObject.Find("EnemyDeathAudio").GetComponent<AudioSource>();
        bulletAudio = GameObject.Find("BulletAudio").GetComponent<AudioSource>();
        livesKeeper = GameObject.Find("LivesKeeper").GetComponent<Lives>();
        scoreKeeper = GameObject.Find("ScoreKeeper").GetComponent<Score>();
        
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * gunSpeed;
        Destroy(gameObject, bulletDestroy);
    }

    public void SetPlayer(PawnData data)
    {
        playerData = data;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2"|| other.gameObject.tag == "Enemy")
        {
            PawnData pawn = other.gameObject.GetComponent<PawnData>();

            if ((pawn.gameObject.tag == "Player1" && playerData.gameObject.tag == "Enemy") || (pawn.gameObject.tag == "Player2" && playerData.gameObject.tag == "Enemy"))
            {
                pawn.health -= playerData.damage;

                if (pawn.health <= 0)
                {
                    playerDeathAudio.Play();
                    pawn.lives--;
                    livesKeeper.AddLives(pawn.lives, pawn);
                    other.gameObject.SetActive(false);
                    GameManager.instance.RespawnPlayer(other.gameObject);

                    if (pawn.lives <= 0)
                    {
                        pawn.GameOver(pawn.gameObject);
                    }
                }
            }

            if ((pawn.gameObject.tag == "Enemy" && playerData.gameObject.tag == "Player1") || (pawn.gameObject.tag == "Enemy" && playerData.gameObject.tag == "Player2"))
            {
                pawn.health -= playerData.damage;

                if (pawn.health <= 0)
                {
                    enemyDeathAudio.Play();
                    GameManager.instance.Enemies.Remove(pawn.gameObject);
                    Destroy(pawn.gameObject);
                    playerData.score++;
                    scoreKeeper.AddScore(playerData, playerData.score);
                }
            }
        }

        bulletAudio.Play();
        Destroy(gameObject);
    }

       
        
}
