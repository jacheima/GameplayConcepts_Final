using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthIndicator;

    public int minHealth;
    public int maxHealth;

    public void SetHealth(float health)
    { 
        //update health UI
        healthIndicator.fillAmount = (float) (health * .01);
    }
}
