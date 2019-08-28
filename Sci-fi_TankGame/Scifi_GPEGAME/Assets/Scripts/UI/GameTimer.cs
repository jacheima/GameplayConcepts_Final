using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Time = UnityEngine.Time;

public class GameTimer : MonoBehaviour
{
    public Text timerTextp1;
    public Text timerTextp2;

    public float minutes =  5;
    public float seconds = 0;


    public void CountDown()
    {
        seconds -= 1 * Time.deltaTime;
        if (seconds <= 0)
        {
            minutes--;
            seconds = 60;
        }

        timerTextp1.text = ((int) minutes).ToString("00") + ":" + ((int) seconds).ToString("00");

        if (GameManager.instance.multiplayer)
        {
            timerTextp2.text = ((int)minutes).ToString("00") + ":" + ((int)seconds).ToString("00");
        }

        

        if (minutes < 0)
        {

            timerTextp1.text = "00:00"; 

            if (GameManager.instance.multiplayer)
            {
                timerTextp2.text = "00:00"; 
            }

            GameManager.instance.levelOneLoaded = false;
            GameManager.instance.GameOver();
        }

    }
}
