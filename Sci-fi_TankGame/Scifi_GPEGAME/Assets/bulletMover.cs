using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletMover : MonoBehaviour
{
    private Rigidbody rb;

    public float gunSpeed = 3.0f;
    public float bulletDestroy = 5.0f;

    public GameManager gm;

    public PawnData playerData;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
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
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
        {
            PawnData pawn = other.gameObject.GetComponent<PawnData>();

            pawn.health--;

            Destroy(gameObject);

            if (pawn.health <= 0)
            {

                if (other.gameObject.tag == "Player")
                {
                    Destroy(other.gameObject);
                    GameManager.instance.SpawnPlayers();
                }

                if (other.gameObject.tag == "Enemy")
                {
                    playerData.score++;
                    Destroy(other.gameObject);
                    GameManager.instance.SpawnEnemies();
                }
            }
        }
    }

}
