using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealingAttack : MonoBehaviour
{
    public PlayerController attack;

    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack.attackHitBoxPos.position, attack.attackRadius, attack.whatIsDamageble);

        foreach (Collider2D collider in detectedObjects)
        {
            collider.GetComponent<EnemyHealth>().Damage(attack.damage);
        }
    }
}
