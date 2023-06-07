using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Max Health")]
    [SerializeField, Range(1, 100)] private int MaxHealth = 20;
    [SerializeField] private PlayerController player;
    [SerializeField, Range(1f, 10f)] private float knockback = 3f;
    [SerializeField, Range(0f, 1f)] private float knockbackDuration = .5f;
    private int currentHealth;
    private float knockbackTimer;
    private Rigidbody2D body;
    private Vector2 velocity;
    private bool isTakedDamage = false;
    

    private void Start()
    {
        currentHealth = MaxHealth;
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        velocity = body.velocity;
        knockbackEnemy();
        Debug.Log(isTakedDamage);
        body.velocity = velocity;
    }

    public void Damage(int damage)
    {
        currentHealth -= damage;
        isTakedDamage = true;
    }

    private void knockbackEnemy()
    {

        if (isTakedDamage && knockbackTimer >= 0f)
        {
            knockbackTimer -= Time.deltaTime;
            Vector2 desiredKnockback = new Vector2(player.PlayerFacingDirection() ? 1 : -1, 0f) * Mathf.Max(knockback, 0f);
            if (player.PlayerFacingDirection() || !player.PlayerFacingDirection())
            {
                velocity.x = Mathf.MoveTowards(velocity.x, desiredKnockback.x, knockback);
            }
        }
        else if (knockbackTimer < 0f && isTakedDamage)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0f, knockback);
            knockbackTimer = knockbackDuration;
            isTakedDamage = false;
        }

    }
}
