using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float distanceToReach = 10f;
    [SerializeField] private float enemyVelocity = 2f;

    private Rigidbody2D body;
    private Vector2 direction;
    private float playerDistance;
    private bool isFacingRight = true;

    private bool onGround;
    private float friction;
    PhysicsMaterial2D material;

    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();
        Flip();
    }

    private void FixedUpdate()
    {
        direction = (player.position - transform.position).normalized;
        EnemyMovement();
    }

    private void CheckDistance()
    {
        playerDistance = Vector2.Distance(transform.position, player.position);
    }

    private void EnemyMovement()
    {
    }

    #region Others
    private void Flip()
    {
        if (direction.x < 0.01f && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
        else if (direction.x > 0.01f && !isFacingRight)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    private void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 normal = collision.GetContact(i).normal;
            if (normal.y >= 0.9f || (normal.y >= 0.9f && normal.x >= 0.9f))
            {
                onGround = true;
            }
        }
    }

    private void RetriveFriction(Collision2D collision)
    {
        material = collision.rigidbody.sharedMaterial;
        friction = 0;

        if (material != null)
        {
            friction = material.friction;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetriveFriction(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetriveFriction(collision);

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        onGround = false;
        friction = 0;
    }

    #endregion
}
