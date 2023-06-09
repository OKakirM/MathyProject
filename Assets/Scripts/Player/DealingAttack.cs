using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class DealingAttack : MonoBehaviour
{
    public PlayerController attack;
    public AudioSource attackSound;
    public AudioSource attackStrongSound;

    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack.attackHitBoxPos.position, attack.attackRadius, attack.whatIsDamageble);
        attackSound.Play();

        foreach (Collider2D collider in detectedObjects)
        {
            if(Random.Range(0, 100) >= 80)
            {
                collider.GetComponent<EnemyHealth>().Damage(attack.damage + 10);
                attackSound.Stop();
                attackStrongSound.Play();
            }
            else
            {
                collider.GetComponent<EnemyHealth>().Damage(attack.damage);
            }
        }
    }

    private void Shake()
    {
        CameraShaker.Instance.ShakeOnce(2f, 3f, .1f, .6f);
    }
}
