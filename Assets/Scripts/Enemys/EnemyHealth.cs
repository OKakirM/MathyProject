using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Max Health")]
    [SerializeField, Range(1, 100)] private int MaxHealth = 20;
    [SerializeField] private PlayerController player;

    [Header("Knockback")]
    [SerializeField, Range(1f, 10f)] private float knockback = 3f;
    [SerializeField, Range(0f, 1f)] private float knockbackDuration = .5f;

    private int currentHealth;
    private float knockbackTimer;
    private Rigidbody2D body;
    private Vector2 velocity;
    [HideInInspector] public bool isDead = false;
    [HideInInspector] public bool isTakedDamage = false;
    

    private void Start()
    {
        currentHealth = MaxHealth;
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        velocity = body.velocity;
        knockbackEnemy();
        body.velocity = velocity;
    }

    public void Damage(int damage)
    {
        currentHealth -= damage;
        isTakedDamage = true;   
        if(currentHealth <= 0)
        {
            isDead = true;
        }
    }

    private void knockbackEnemy()
    {
        if(isTakedDamage && knockbackTimer > 0f)
        {
            knockbackTimer -= Time.deltaTime;
            Vector2 desiredKnockback = new Vector2(player.PlayerFacingDirection() ? 1 : -1, 0f) * Mathf.Max(knockback, 0f);
            velocity.x = Mathf.MoveTowards(velocity.x, desiredKnockback.x, knockback);
        }
        else if(knockbackTimer <= 0f)
        {
            knockbackTimer = knockbackDuration;
            isTakedDamage = false;
        }

    }
}
