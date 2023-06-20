using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class DealingExplosionAttack : MonoBehaviour
{
    public EnemyDamage damage;
    public float attackRadius;
    public Transform hitbox;
    public LayerMask mask;

    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(hitbox.position, attackRadius, mask);

        foreach (Collider2D collider in detectedObjects)
        {
            if (Random.Range(0, 100) >= 80)
            {
                collider.GetComponent<PlayerController>().TakingDamage(damage.damage + 10);
            }
            else
            {
                collider.GetComponent<PlayerController>().TakingDamage(damage.damage);
            }
        }
    }

    private void Shake()
    {
        CameraShaker.Instance.ShakeOnce(2f, 3f, .1f, .6f);
    }
}
