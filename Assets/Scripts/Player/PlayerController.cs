using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EZCameraShake;

public class PlayerController : MonoBehaviour
{
    #region Variables

    #region Health Variables
    [Header("Health")]
    [SerializeField, Range(1, 100)] private float playerHealth = 20;
    [SerializeField] private HealthBar healthBar;
    [HideInInspector] public float currentHealth;
    #endregion

    #region Taking Damage Variables
    [Header("Taking Damage")]
    [SerializeField, Range(1f, 200f)] private float takeDamageImpulse = 10f;
    [SerializeField, Range(1f, 10f)] private float invencibleTime = 2f;
    [SerializeField] private GameObject ui;
    [SerializeField] private GameObject deathBG;
    private bool isTakedDamage = false;
    private float invencibleCounter;
    [HideInInspector] public bool isDead = false;
    #endregion

    #region Dealing Damage Variables
    [Header("Dealing Damage")]
    [SerializeField, Range(1f, 50f)] public int damage = 5;
    [SerializeField, Range(1f, 10f)] private float attackImpulse = 3f;
    [SerializeField, Range(0f, 1f)] private float attackDelay = .5f;
    [SerializeField] public Transform attackHitBoxPos;
    [SerializeField, Range(0f, 1f)] public float attackRadius;
    [SerializeField] public LayerMask whatIsDamageble;
    private bool inputAttack;
    [HideInInspector] public bool isAttacking = false;
    private float attackCounter;
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

    #region Crouch Variables
    [HideInInspector] public bool isCrouching;
    #endregion

    #region Dodge Variables
    [Header("Dodge")]
    [SerializeField, Range(0f, 100f)] private float dodgeImpulse = 8f;
    [SerializeField, Range(0f, 1f)] private float dodgeTime = 0.12f;
    [SerializeField, Range(0f, 1f)] private float dodgeCooldown = .1f;
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
    private bool prevGround;
    #endregion

    #region Direction & Velocity Variables
    [HideInInspector] public Vector2 direction;
    private Vector2 desiredVelocity;
    private Vector2 velocity;
    #endregion

    #region Componets Variables
    private Rigidbody2D body;
    [Header("Animator Controller")]
    [InspectorName("Sprite Animator")]public Animator anim;
    [InspectorName("Squash & Stretch Animator")] public Animator anchor;
    public static PlayerController instance;
    public GameOverScript gameover;
    public PauseMenu pauseMenu;
    public QuestionScripts doQuestion;
    #endregion

    #region Audio Variables
    [Header("Audio Controller")]
    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource walkSoundEffect;
    [SerializeField] private AudioSource hitSoundEffect;
    [SerializeField] private AudioSource dashSoundEffect;
    private float timerCount;

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

    #region Effects Variables
    //[Header("Effects")]
    #endregion

    #endregion

    #region Unity Functions
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();

        defaultGravityScale = 2f;
        dodgeCooldownCounter = dodgeCooldown;
        currentHealth = playerHealth;
        healthBar.SetMaxHealth(playerHealth);

        ui.SetActive(true);
        deathBG.SetActive(false);
    }

    private void Update()
    {
        if (!pauseMenu.isPaused && !doQuestion.isSolving)
        {
            MoveInput();
            JumpInput();
            DodgeInput();
            CrouchInput();
            AttackInput();
            Attack();
            Flip();
            Animation();
            healthBar.SetHealth(currentHealth);
        }
        doQuestion.UpdateQuiz();

        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void FixedUpdate()
    {
        velocity = body.velocity;
        Move();
        Jump();
        Dodge();
        Crouching();
        Invencible();
        body.velocity = velocity;
    }

    #endregion

    #region Movement
    private void MoveInput()
    {
        if (isDodging || isCrouching || attackCounter >= 0f || isDead)
        {
            direction.x = 0f;
        } else
        {
            direction.x = Input.GetAxisRaw("Horizontal");
        }
        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - friction, 0f);
    }

    private void Move()
    {
        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        maxSpeedChange = acceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
    }

    #endregion

    #region Dodge
    private void DodgeInput()
    {
        desiredDodgeVelocity = new Vector2(isFacingRight ? 1 : -1, 0f) * Mathf.Max(dodgeImpulse - friction, 0f);
    }

    private void Dodge()
    {
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.K)) && !isDodging && dodgeCooldownCounter >= dodgeCooldown && !isCrouching && attackCounter < 0f && !isDead)
        {
            dodgeCounter = dodgeTime;
        }

        if (dodgeCounter > 0f)
        {
            dodgeCounter -= Time.deltaTime;
            dodgeCooldownCounter = 0;
            DodgeAction();
        } 
        else if (dodgeCounter <= 0f && dodgeCooldownCounter <= dodgeCooldown)
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
        PlayerShadows.me.ShadowTimer();
        CameraShaker.Instance.ShakeOnce(2f, .5f, .1f, .2f);
        isDodging = true;
        if (!dashSoundEffect.isPlaying)
        {
            dashSoundEffect.Play();
        }
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
        prevGround = onGround;
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

        if (jumpBufferCounter > 0 && !isDodging && !isDead && !isAttacking && !inputAttack)
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
            if (!jumpSoundEffect.isPlaying)
            {
                jumpSoundEffect.Play();
            }
            prevGround = false;
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

    #region Crouching
    private void CrouchInput()
    {
        if(!isDead)
        {
            isCrouching = Input.GetKey(KeyCode.S);
        }

    }

    private void Crouching()
    {
        if (isCrouching)
        {
            direction.x = 0;
            velocity.x = Mathf.MoveTowards(velocity.x, 0, .8f);
        }
    }
    #endregion

    #region Taking Damage
    public void TakingDamage(int damage)
    {
        Vector2 desiredDamageImpulse = new Vector2(isFacingRight ? -1 : 1, 0f) * Mathf.Max(takeDamageImpulse, 0f);
        if (!isDodging && !isTakedDamage)
        {
            CameraShaker.Instance.ShakeOnce(2f, 4f, .1f, 1f);
            velocity = body.velocity;
            velocity.x = Mathf.MoveTowards(velocity.x, desiredDamageImpulse.x, takeDamageImpulse);
            body.velocity = velocity;
            isTakedDamage = true;
            currentHealth -= damage;
            hitSoundEffect.Play();


            if (currentHealth <= 0 && !isDead)
            {
                if(damage >= 100)
                {
                    doQuestion.Wrong();
                }
                else
                {
                    doQuestion.Setup();
                }
            }
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

    #region Dealing Damage
    private void AttackInput()
    {
        inputAttack = Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.J);
    }

    public void Attack()
    {
        if (inputAttack && !isAttacking && onGround && !isDodging && !isCrouching && !isDead)
        {
            Vector2 desiredAttackImpulse = new Vector2(isFacingRight ? 1 : -1, 0f) * Mathf.Max(attackImpulse, 0f);
            isAttacking = true;
            attackCounter = attackDelay;
            if (direction.x != 0f)
            {
                body.AddForce((desiredAttackImpulse * attackImpulse) * .15f, ForceMode2D.Impulse);
            } 
            else
            {
                body.AddForce((desiredAttackImpulse * attackImpulse) * .25f, ForceMode2D.Impulse);
            }
        } 
        else
        {
            attackCounter -= Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackHitBoxPos.position, attackRadius);                                                                                                   
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

    public bool PlayerFacingDirection()
    {
        return isFacingRight;
    }

    private void Animation()
    {
        if (direction.x != 0 && onGround)
        {
            isRunning = true;
            timerCount += Time.deltaTime;

            if(!walkSoundEffect.isPlaying && timerCount >= .3f)
            {
                timerCount = 0f;
                walkSoundEffect.Play();
            }
        }
        else
        {
            isRunning = false;
        }

        if (onGround && !prevGround && !Input.GetButton("Jump"))
        {
            prevGround = true;
            anchor.SetTrigger("Squash");
        }

        if(Input.GetButtonDown("Jump") && onGround)
        {
            anchor.SetTrigger("Stretch");
        }

        anim.SetBool("isRunning", isRunning);
        anim.SetBool("onGround", onGround);
        anim.SetBool("isCrouching", isCrouching);
        anim.SetBool("isDead", isDead);
        anim.SetFloat("invencibleCounter", invencibleCounter);
        anchor.SetBool("isTakedDamage", isTakedDamage);
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
