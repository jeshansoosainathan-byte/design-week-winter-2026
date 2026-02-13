// PlayerController.cs
// Reads input directly from an assigned Gamepad reference.
// Features: momentum-based movement, configurable ground check point, coyote time,
// aim target (right stick Y), and shooting with cooldown.

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float acceleration = 40f;
    public float deceleration = 30f;
    public float turnDeceleration = 50f;

    [Header("Jump")]
    public float jumpHeight = 12f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 1.5f;

    [Header("Ground Check")]
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    [Header("Coyote Time")]
    public float coyoteTimeMax = 0.15f;

    [Header("Aim Target")]
    public Transform aimTarget;           // Empty child GameObject — the aim constraint looks at this
    public float aimRange = 3f;           // Max distance the target can move up or down (local Y)
    public float aimSmoothing = 15f;      // How fast the target follows the stick
    public float aimDeadzone = 0.15f;     // Stick values below this snap to center
    public bool rightPlayer = false;      // Toggle ON for the right-side player to invert aim

    [Header("Shooting")]
    public ParticleSystem shootParticle;  // Particle system on the arm/gun — plays once per shot
    public float shootCooldown = 1f;      // Seconds between shots

    // Internal state — movement
    private float coyoteTimer;
    private bool isGrounded;
    private bool jumpHeld;
    private bool jumpPressed;
    private float moveInput;
    private float currentSpeed;

    // Internal state — aim and shoot
    private float aimInput;
    private float currentAimY;            // Current local Y of the aim target
    private float shootTimer;             // Counts down to 0 — can shoot when <= 0
    private Vector3 aimTargetDefaultLocal; // Default local position of aim target

    // Input lockout — GameManager toggles this during round transitions
    [HideInInspector] public bool inputLocked = false;

    // Player index (0 = left player, 1 = right player)
    [HideInInspector] public int playerIndex = -1;

    // The specific gamepad this player reads from — assigned by JoinManager
    [HideInInspector] public Gamepad assignedGamepad;

    // Components
    private Rigidbody2D rb;
    private Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        coyoteTimer = coyoteTimeMax;
        shootTimer = 0f;

        // Store the aim target's default local position so we only modify Y
        if (aimTarget != null)
        {
            aimTargetDefaultLocal = aimTarget.localPosition;
            currentAimY = aimTargetDefaultLocal.y;
        }

        // Make sure the particle system doesn't play on awake
        if (shootParticle != null)
        {
            var main = shootParticle.main;
            main.playOnAwake = false;
            shootParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    void Update()
    {
        ReadInput();

        // ---- MOVEMENT ----
        isGrounded = CheckGrounded();

        if (isGrounded)
            coyoteTimer = coyoteTimeMax;
        else
            coyoteTimer -= Time.deltaTime;

        if (coyoteTimer > 0f && jumpPressed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
            coyoteTimer = 0f;
        }

        jumpPressed = false;

        // Gravity modifications
        if (rb.linearVelocity.y < 0f)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0f && !jumpHeld)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * lowJumpMultiplier * Time.deltaTime;
        }

        ApplyMomentum();
        rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);

        // ---- AIM ----
        UpdateAimTarget();

        // ---- SHOOT ----
        shootTimer -= Time.deltaTime;

        // ---- ANIMATOR ----
        if (anim != null && anim.runtimeAnimatorController != null)
        {
            float normalizedSpeed = Mathf.Abs(currentSpeed) / moveSpeed;
            anim.SetFloat("walkSpeed", normalizedSpeed);
            anim.SetBool("isJumping", !isGrounded);
        }
    }

    // =====================
    // MOMENTUM
    // =====================

    void ApplyMomentum()
    {
        float targetSpeed = moveInput * moveSpeed;

        if (Mathf.Abs(moveInput) > 0.01f)
        {
            bool turningAround = (currentSpeed != 0f) && (Mathf.Sign(targetSpeed) != Mathf.Sign(currentSpeed));

            if (turningAround)
                currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, turnDeceleration * Time.deltaTime);
            else
                currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }
    }

    // =====================
    // AIM
    // =====================

    void UpdateAimTarget()
    {
        if (aimTarget == null) return;

        // Target Y based on stick input
        float targetY;

        if (Mathf.Abs(aimInput) < aimDeadzone)
        {
            // Stick is centered — return to default position
            targetY = aimTargetDefaultLocal.y;
        }
        else
        {
            // Map stick input (-1 to 1) to aim range
            targetY = aimTargetDefaultLocal.y + (aimInput * aimRange);
        }

        // Smoothly move towards target
        currentAimY = Mathf.Lerp(currentAimY, targetY, aimSmoothing * Time.deltaTime);

        // Apply — only modify local Y, keep X and Z from the default
        aimTarget.localPosition = new Vector3(
            aimTargetDefaultLocal.x,
            currentAimY,
            aimTargetDefaultLocal.z
        );
    }

    // =====================
    // SHOOT
    // =====================

    void TryShoot()
    {
        if (shootTimer > 0f) return;
        if (shootParticle == null) return;

        // Play the particle system once
        shootParticle.Play();

        // Reset cooldown
        shootTimer = shootCooldown;
    }

    // =====================
    // INPUT
    // =====================

    void ReadInput()
    {
        if (assignedGamepad == null)
        {
            moveInput = 0f;
            aimInput = 0f;
            return;
        }

        if (inputLocked)
        {
            moveInput = 0f;
            aimInput = 0f;
            jumpPressed = false;
            jumpHeld = false;
            return;
        }

        // Left stick — horizontal movement
        Vector2 leftStick = assignedGamepad.leftStick.ReadValue();
        moveInput = leftStick.x;

        // Right stick — vertical aim (only Y matters)
        Vector2 rightStick = assignedGamepad.rightStick.ReadValue();
        aimInput = rightPlayer ? -rightStick.y : rightStick.y;

        // Left bumper — jump
        if (assignedGamepad.leftShoulder.wasPressedThisFrame)
        {
            jumpPressed = true;
            jumpHeld = true;
        }
        if (assignedGamepad.leftShoulder.wasReleasedThisFrame)
        {
            jumpHeld = false;
        }

        // Right trigger — shoot
        if (assignedGamepad.rightTrigger.wasPressedThisFrame)
        {
            TryShoot();
        }
    }

    // =====================
    // GROUND CHECK
    // =====================

    bool CheckGrounded()
    {
        if (groundCheckPoint == null)
        {
            Debug.LogWarning($"{gameObject.name}: No groundCheckPoint assigned! Using transform.position as fallback.");
            return Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayer);
        }

        return Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
    }

    // =====================
    // PUBLIC METHODS
    // =====================

    public void ResetState()
    {
        rb.linearVelocity = Vector2.zero;
        currentSpeed = 0f;
        moveInput = 0f;
        aimInput = 0f;
        jumpPressed = false;
        jumpHeld = false;
        coyoteTimer = coyoteTimeMax;
        shootTimer = 0f;

        // Reset aim target to default position
        if (aimTarget != null)
        {
            currentAimY = aimTargetDefaultLocal.y;
            aimTarget.localPosition = aimTargetDefaultLocal;
        }
    }

    public void SetPosition(Vector3 spawnPosition)
    {
        transform.position = spawnPosition;
        ResetState();
    }

    void OnDrawGizmosSelected()
    {
        // Ground check
        Gizmos.color = Color.green;
        if (groundCheckPoint != null)
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        else
            Gizmos.DrawWireSphere(transform.position, groundCheckRadius);

        // Aim range visualization
        if (aimTarget != null)
        {
            Gizmos.color = Color.yellow;
            Vector3 basePos = transform.position + (aimTarget.localPosition - new Vector3(0, aimTarget.localPosition.y, 0));
            Vector3 topPos = basePos + Vector3.up * aimRange;
            Vector3 bottomPos = basePos + Vector3.down * aimRange;
            Gizmos.DrawLine(topPos, bottomPos);
            Gizmos.DrawWireSphere(topPos, 0.05f);
            Gizmos.DrawWireSphere(bottomPos, 0.05f);
        }
    }
}
