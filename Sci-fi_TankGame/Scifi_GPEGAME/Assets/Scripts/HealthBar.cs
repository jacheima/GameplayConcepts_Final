using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthIndicator;

    public int minHealth;
    public int maxHealth;
    private int currentHealth;

    public void SetHealth(int health)
    {
        if (health != currentHealth)
        {
            if (maxHealth - minHealth == 0)
            {
                currentHealth = 0;
            }
            else
            {
                currentHealth = health;
            }

            healthIndicator.fillAmount = (float) (currentHealth * .01);
        }
    }

    void Start()
    {
        SetHealth(41);
    }
}
