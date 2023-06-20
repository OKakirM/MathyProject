using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField, Range(1, 100)] public int damage = 10;
    private Rigidbody2D body;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            body.velocity = Vector2.zero;
            other.GetComponent<PlayerController>().TakingDamage(damage);
        }
    }
}
