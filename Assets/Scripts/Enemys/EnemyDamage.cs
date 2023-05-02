using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField, Range(1, 100)] private int damage = 10;
    [SerializeField, Range(1, 5)] private float playerInvencibleTime = 3f;
    private bool isDoingDamage = false;
    private float playerCounterTime;

    private void Update()
    {
        if (isDoingDamage && playerCounterTime <= playerInvencibleTime)
        {
            playerCounterTime += Time.deltaTime;
            Physics2D.IgnoreLayerCollision(9, 7, true);
        }
        else if(playerCounterTime >= playerInvencibleTime)
        {
            isDoingDamage = false;
            Physics2D.IgnoreLayerCollision(9, 7, false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !isDoingDamage)
        {
            other.GetComponent<PlayerController>().TakingDamage(damage);
            isDoingDamage = true;
            playerCounterTime = 0;
        }
    }
}
