using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lives : MonoBehaviour
{
    public Text livesTextP1;
    public Text livesTextP2;

    void Start()
    {
        livesTextP1 = GameObject.FindWithTag("Player1LivesText").GetComponent<Text>();
        livesTextP2 = GameObject.FindWithTag("Player2LivesText").GetComponent<Text>();
    }

    public void AddLives(int lives, PawnData player)
    {
        if (player.gameObject.tag == "Player1")
        {
            livesTextP1.text = ((int) lives).ToString();
        }

        if (player.gameObject.tag == "Player2")
        {
            livesTextP2.text = ((int)lives).ToString();
        }
    }
}
