using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image hpImage;
    public Image hpEffectImage;
    public float hurtSpeed = 0.005f;

    private int playerHealth;
    private int maxPlayerHealth;

    //public void SetMaxHealth(int maxHealth)
    //{
    //    slider.maxValue = maxHealth;
    //    slider.value = maxHealth;
    //}

    private void Update()
    {
        hpImage.fillAmount = playerHealth / maxPlayerHealth;
        if (hpEffectImage.fillAmount > hpImage.fillAmount)
        {
            hpEffectImage.fillAmount -= hurtSpeed;
        }
        else
        {
            hpEffectImage.fillAmount = hpImage.fillAmount;
        }
    }

    public void SetHealth(int health, int maxHealth)
    {
        maxPlayerHealth = maxHealth;
        playerHealth = health;
    }
}
