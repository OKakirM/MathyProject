using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image hpImage;
    public Image hpEffectImage;
    public float hurtSpeed = 0.005f;

    private float playerHealth;
    private float maxPlayerHealth;

    public void SetMaxHealth(float maxHealth)
    {
        playerHealth = maxHealth;
        maxPlayerHealth = maxHealth;
    }

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

    public void SetHealth(float health)
    {
        playerHealth = health;
    }
}
