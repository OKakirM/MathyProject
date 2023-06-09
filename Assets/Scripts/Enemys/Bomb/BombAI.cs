using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class BombAI : MonoBehaviour
{
    #region Variables

    #region Movement & Distance Variables
    [Header("Movement & Distance")]
    [SerializeField, Range(0f, 20f)] private float distanceToReach = 10f;
    [SerializeField, Range(0f, 10f)] private float enemyVelocity = 2f;
    [SerializeField, Range(0f, 20f)] private float enemyMaxVelocity = 5f;
    private float maxSpeedChange;
    #endregion

    #region Destroy Object Variables
    [SerializeField, Range(0f, 1f)] private float destroyTime = 0.2f;
    private float destroyCounter = 0f;

    #endregion

    #region Components Variables
    [Header("Components")]
    [SerializeField] private Transform player;
    [SerializeField] private PlayerController pController;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private EnemyShake enemyShake;
    [SerializeField] private Animator anim;
    private Rigidbody2D body;
    #endregion

    #region Bool Variables Check
    private float playerDistance;
    private bool isFacingRight = true;
    private bool isRunning;
    private bool isExploding;
    #endregion

    #region Direction & Velocity Variables
    private Vector2 direction;
    private Vector2 velocity;
    private Vector2 desiredVelocity;
    #endregion

    #region Collisions Variable
    private bool onGround;
    private float friction;
    PhysicsMaterial2D material;
    #endregion

    #region Audio Variables
    public AudioSource shakingSound;
    public AudioSource deathSound;
    public AudioSource talkSound;
    private bool wasPlayed = false;
    #endregion

    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        isExploding = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();
        Animations();
        Flip();

        if (velocity.x != 0f && !isExploding)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    private void FixedUpdate()
    {
        direction = (player.position - transform.position).normalized;
        velocity = body.velocity;
        EnemyMovement();
        body.velocity = velocity;
    }

    private void CheckDistance()
    {
        playerDistance = Vector2.Distance(transform.position, player.position);
    }

    private void EnemyMovement()
    {
        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(enemyVelocity, 0f);
        maxSpeedChange = enemyMaxVelocity * Time.deltaTime;
        if (playerDistance <= distanceToReach && !pController.isDead && !enemyHealth.isTakedDamage && !enemyHealth.isDead)
        {
            if((playerDistance <= 1.5f && playerDistance >= 1f) || enemyHealth.isTakedDamage || isExploding)
            {
                velocity.x = Mathf.MoveTowards(velocity.x, 0f, maxSpeedChange);
                isExploding = true;
            }
            else if(!isExploding)
            {
                velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
            }
        }
        else
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0f, maxSpeedChange);
        }

        if (isExploding)
        {
            anim.SetBool("isExplosing", isExploding);
            StartCoroutine(enemyShake.Shake(.1f, .015f));
            if (!shakingSound.isPlaying)
            {
                shakingSound.Play();
            }
            if (!talkSound.isPlaying && !wasPlayed)
            {
                talkSound.Play();
                wasPlayed = true;
            }
            destroyCounter += Time.deltaTime;

            if (destroyCounter >= destroyTime)
            {
                anim.SetBool("isDead", true);
                CameraShaker.Instance.ShakeOnce(2f, 4f, .1f, 1f);
                if (!deathSound.isPlaying)
                {
                    deathSound.Play();
                }
                if (destroyCounter >= destroyTime + .6f)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    #region Others

    private void Animations()
    {
        anim.SetBool("isTakedDamage", enemyHealth.isTakedDamage);
        anim.SetBool("isRunning", isRunning);
    }

    private void Flip()
    {
        if (direction.x < 0.01f && isFacingRight && !isExploding)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
        else if (direction.x > 0.01f && !isFacingRight && !isExploding)
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
