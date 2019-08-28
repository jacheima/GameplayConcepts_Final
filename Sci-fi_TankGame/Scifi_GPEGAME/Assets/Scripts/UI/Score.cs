using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreTextp1;
    public Text scoreTextp2;

    void Start()
    {
        scoreTextp1 = GameObject.FindWithTag("Player1ScoreText").GetComponent<Text>();
        scoreTextp2 = GameObject.FindWithTag("Player2ScoreText").GetComponent<Text>();
    }

    public void AddScore(PawnData player, int score)
    {

        if (player.gameObject.tag.Equals("Player1"))
        {
            scoreTextp1.text = ((int) score).ToString();
        }

        if (player.gameObject.tag.Equals("Player2"))
        {
            scoreTextp2.text = ((int)score).ToString();
        }

    }
}
