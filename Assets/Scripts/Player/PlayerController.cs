using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables

    #region Health Variables
    [Header("Health")]
    [SerializeField, Range(1, 100)] private int playerHealth = 20;
    [SerializeField] private HealthBar healthBar;
    public int currentHealth;
    #endregion

    #region Damage Variables
    [Header("Damage")]
    [SerializeField, Range(1, 200)] private float takeDamageImpulse = 10;
    [SerializeField, Range(1, 10)] private float invencibleTime = 2;
    private Vector2 desiredDamageImpulse;
    private bool isTakedDamage = false;
    private float invencibleCounter;
    #endregion

    #region Movement Variables
    [Header("Movement")]
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;
    private float maxSpeedChange;
    private float acceleration;
    private bool isRunning = false;
    #endregion

    #region Dodge Variables
    [Header("Dodge")]
    [SerializeField, Range(0f, 100f)] private float dodgeImpulse = 8f;
    [SerializeField, Range(0f, 1f)] private float dodgeTime = 0.12f;
    [SerializeField, Range(0f, 10f)] private float dodgeCooldown = .1f;
    private bool isDodging;
    private Vector2 desiredDodgeVelocity;
    private float dodgeCounter;
    private float dodgeCooldownCounter;
    #endregion

    #region Jump Variables
    [Header("Jump Movement")]
    [SerializeField, Range(0f, 10f)] private float jumpHeight = 3f;
    [SerializeField, Range(0, 5)] private int maxAirJumps = 0;
    [SerializeField, Range(0f, 5f)] private float downwardMovementMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] private float upwardMovementMultiplier = 1.7f;
    [SerializeField, Range(0f, .3f)] private float coyoteTime = .12f;
    [SerializeField, Range(0f, .3f)] private float jumpBufferTime = .12f;
    private int jumpPhase;
    private float defaultGravityScale;
    private float coyoteCounter;
    private float jumpBufferCounter;
    private bool desiredJump;
    #endregion

    #region Direction & Velocity Variables
    private Vector2 direction;
    private Vector2 desiredVelocity;
    private Vector2 velocity;
    #endregion

    #region Componets Variables
    private Rigidbody2D body;
    private Animator anim;
    #endregion

    #region Bool Variables Check
    private bool isFacingRight = true;
    private bool isDodgingActive;
    #endregion

    #region Collisions Variables
    private bool onGround;
    private float friction;
    PhysicsMaterial2D material;
    #endregion

    #endregion

    #region Unity Functions
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        defaultGravityScale = 2f;
        dodgeCooldownCounter = dodgeCooldown;
        currentHealth = playerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        MoveInput();
        JumpInput();
        DodgeInput();
        Flip();
        Animation();
    }

    private void FixedUpdate()
    {
        velocity = body.velocity;
        Move();
        Jump();
        Dodge();
        Invencible();
        body.velocity = velocity;
    }
    #endregion

    #region Movement
    private void MoveInput()
    {
        if (!isDodging)
        {
            direction.x = Input.GetAxisRaw("Horizontal");
        }
        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - friction, 0f);
    }

    private void Move()
    {
        if (!isDodging)
        {
            acceleration = onGround ? maxAcceleration : maxAirAcceleration;
            maxSpeedChange = acceleration * Time.deltaTime;
            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        }
    }

    #endregion

    #region Dodge
    private void DodgeInput()
    {
        desiredDodgeVelocity = new Vector2(isFacingRight ? 1 : -1, 0f) * Mathf.Max(dodgeImpulse - friction, 0f);
    }

    private void Dodge()
    {
        if (Input.GetButton("Fire3") && !isDodging && onGround && dodgeCooldownCounter >= dodgeCooldown)
        {
            dodgeCounter = dodgeTime;
        }

        if (dodgeCounter > 0f && onGround || dodgeCounter > 0f && body.velocity.y < 0)
        {
            dodgeCounter -= Time.deltaTime;
            dodgeCooldownCounter = 0;
            DodgeAction();
        } 
        else if (dodgeCounter < 0f && dodgeCooldownCounter <= dodgeCooldown)
        {
            isDodging = false;
            dodgeCooldownCounter += Time.deltaTime;
        }

        if (isDodgingActive)
        {
            Physics2D.IgnoreLayerCollision(9, 7, true);
        }
        
        if(dodgeCooldownCounter >= dodgeTime)
        {
            Physics2D.IgnoreLayerCollision(9, 7, false);
            isDodgingActive = false;
        }
    }

    private void DodgeAction()
    {
        isDodging = true;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredDodgeVelocity.x, dodgeImpulse);
        if(body.velocity.y >= 0f)
        {
            body.gravityScale = 0f;
        }
    }
    #endregion

    #region Jumping
    private void JumpInput()
    {
        desiredJump |= Input.GetButtonDown("Jump");
    }

    private void Jump()
    {
        if (onGround)
        {
            jumpPhase = 0;
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        if (desiredJump)
        {
            desiredJump = false;
            jumpBufferCounter = jumpBufferTime;
        }
        else if (!desiredJump && jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0 && !isDodging)
        {
            JumpAction();
        }

        if (Input.GetButton("Jump") && body.velocity.y > 0)
        {
            body.gravityScale = upwardMovementMultiplier;
        }
        else if(!Input.GetButton("Jump") || body.velocity.y < 0)
        {
            body.gravityScale = downwardMovementMultiplier;
        }
        else if(body.velocity.y == 0)
        {
            body.gravityScale = defaultGravityScale;
        }
    }

    private void JumpAction()
    {
        if (coyoteCounter > 0f || jumpPhase < maxAirJumps)
        {

            jumpPhase += 1;
            jumpBufferCounter = 0f;
            coyoteCounter = 0;
            float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
            if (velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            velocity.y += jumpSpeed;
        }
    }
    #endregion

    #region Damage
    public void TakingDamage(int damage)
    {
        desiredDamageImpulse = new Vector2(isFacingRight ? -1 : 1, 0f) * Mathf.Max(takeDamageImpulse, 0f);
        if (!isDodging && !isTakedDamage)
        {
            velocity = body.velocity;
            velocity.x = Mathf.MoveTowards(velocity.x, desiredDamageImpulse.x, takeDamageImpulse);
            body.velocity = velocity;
            isTakedDamage = true;
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth, playerHealth);
        }
    }

    private void Invencible()
    {
        if(isTakedDamage && invencibleCounter <= invencibleTime)
        {
            Physics2D.IgnoreLayerCollision(9, 7, true);
            invencibleCounter += Time.deltaTime;
        } 
        else if(invencibleCounter >= invencibleTime)
        {
            Physics2D.IgnoreLayerCollision(9, 7, false);
            invencibleCounter = 0;
            isTakedDamage = false;
        }
    }

    #endregion

    #region Animations
    private void Flip()
    {
        if (direction.x < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
        else if (direction.x > 0 && !isFacingRight)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    private void Animation()
    {
        if(direction.x != 0)
        {
            isRunning = true;
        } else
        {
            isRunning = false;
        }

        anim.SetBool("isRunning", isRunning);
        anim.SetBool("onGround", onGround);
        anim.SetBool("isDodging", isDodging);
        anim.SetFloat("isTakedDamage", invencibleCounter);
        anim.SetFloat("yVelocity", body.velocity.y);
    }
    #endregion

    #region Collisons & GroundCheck
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
        if(collision.gameObject.layer == 9 && isDodging)
        {
            isDodgingActive = true;
        }
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
