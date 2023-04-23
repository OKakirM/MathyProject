using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float distanceToReach = 10f;
    [SerializeField] private float enemyVelocity = 2f;
    [SerializeField] private float enemyJumpHeight = 2f;
    [SerializeField] private float enemyAttackDelay = 3f;

    private Rigidbody2D body;
    private Vector2 direction;
    private float playerDistance;
    private float enemyDelayCounter;
    private bool isFacingRight = true;
    private bool preperingAttack;
    private bool isAttacking;

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
        if(playerDistance <= distanceToReach)
        {
            
            if (playerDistance >= 1f && !preperingAttack)
            {
                body.velocity = new Vector2(direction.x * enemyVelocity, body.velocity.y);
            } 
            else
            {
                preperingAttack = true;
                enemyDelayCounter += Time.deltaTime;
                if (enemyDelayCounter >= enemyAttackDelay)
                {
                    preperingAttack = false;
                    isAttacking = true;
                    enemyDelayCounter = 0f;
                    body.AddForce(Vector2.up * enemyJumpHeight * direction, ForceMode2D.Impulse);
                }
                else if(!isAttacking)
                {
                    body.velocity = Vector2.zero;
                }
            }
        } 
        else
        {
            body.velocity = Vector2.zero;
        }
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
