using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

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

    private void Shake()
    {
        CameraShaker.Instance.ShakeOnce(2f, 3f, .1f, .6f);
    }
}
