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

    public void Damage(int damage)
    {
        currentHealth -= damage;
        knockbackEnemy();
        isTakedDamage = true;
    }

    private void knockbackEnemy()
    {
        Vector2 desiredKnockback = new Vector2(player.PlayerFacingDirection() ? 1 : -1, 0f);
        body.AddForce(desiredKnockback * knockback, ForceMode2D.Impulse);
    }
}
