using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Import the SceneManager

public class Movement2D : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    private LineRenderer lineRenderer;
    private Animator animator;
    private AudioSource audioSource;

    [Header("Movement Variables")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private int extraJumps = 1;
    [SerializeField] private Transform projectileSpawnPoint;

    private int extraJumpsLeft;
    private bool isFacingRight = true;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 10f;

    // Variables for drawing line
    private bool drawingLine = false;
    private Vector3 lineEndPoint;

    // Variables for handling damage
    private int hitCounter = 0;
    [SerializeField] private int maxHits = 3;

    [Header("UI Elements")]
    [SerializeField] private Slider healthSlider;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip dieSound;

    // Scene names for transitions
    [Header("Scene Names")]
    [SerializeField] private string deathSceneName;
    [SerializeField] private string finishLineSceneName;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        extraJumpsLeft = extraJumps;

        // Initialize LineRenderer
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // Initialize Animator
        animator = GetComponent<Animator>();

        // Initialize AudioSource
        audioSource = GetComponent<AudioSource>();

        // Initialize Health Slider
        healthSlider.maxValue = maxHits;
        healthSlider.value = maxHits;
    }

    private void Update()
    {
        // Ground check
        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.5f, 0.1f), 0f, groundLayer);

        // Handle jump input
        if (isGrounded)
        {
            extraJumpsLeft = extraJumps;
        }

        // Handle mouse input for drawing line and shooting projectile
        if (Input.GetMouseButtonDown(1)) // Right mouse button pressed
        {
            StartDrawingLine();
        }
        else if (Input.GetMouseButtonUp(1)) // Right mouse button released
        {
            StopDrawingLine();
            ShootProjectile();
            TriggerAttackAnimation();
            PlaySound(shootSound); // Play shoot sound
        }

        if (Input.GetButtonDown("Jump") && (isGrounded || extraJumpsLeft > 0))
        {
            Jump();
        }

        if (drawingLine)
        {
            UpdateLineEndPoint();
            DrawLineToMouse();
        }

        // Update animations
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        // Handle movement input
        float moveInput = Input.GetAxis("Horizontal");

        // Apply acceleration
        float newVelocityX = rb.velocity.x + moveInput * acceleration * Time.fixedDeltaTime;

        // Clamp velocity to maximum speed
        newVelocityX = Mathf.Clamp(newVelocityX, -maxSpeed, maxSpeed);

        // Set the new velocity
        rb.velocity = new Vector2(newVelocityX, rb.velocity.y);

        // Flip character based on movement direction
        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        if (!isGrounded)
        {
            extraJumpsLeft--;
        }

        // Set jump animation
        animator.SetBool("isJump", true);
        PlaySound(jumpSound); // Play jump sound
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void DrawLineToMouse()
    {
        // Set the line points
        lineRenderer.SetPosition(0, projectileSpawnPoint.position);
        lineRenderer.SetPosition(1, lineEndPoint);
    }

    private void StartDrawingLine()
    {
        // Set flag to start drawing line
        drawingLine = true;

        // Update the line end point initially
        UpdateLineEndPoint();
    }

    private void StopDrawingLine()
    {
        // Clear the drawn line
        drawingLine = false;
        lineRenderer.SetPosition(0, projectileSpawnPoint.position);
        lineRenderer.SetPosition(1, projectileSpawnPoint.position);
    }

    private void UpdateLineEndPoint()
    {
        // Update the line end point to match the current position of the mouse cursor
        lineEndPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lineEndPoint.z = 0;
    }

    private void ShootProjectile()
    {
        // Instantiate the projectile prefab at the position of the spawn point
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

        // Calculate the direction towards the mouse cursor
        Vector2 direction = (lineEndPoint - projectileSpawnPoint.position).normalized;

        // Apply velocity to the projectile in the calculated direction
        projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
    }

    private void UpdateAnimations()
    {
        // Set the isRun parameter based on horizontal movement
        animator.SetBool("isRun", Mathf.Abs(rb.velocity.x) > 0.1f);

        // Set the isJump parameter based on grounded state
        animator.SetBool("isJump", !isGrounded);
    }

    private void TriggerAttackAnimation()
    {
        // Trigger the attack animation
        animator.SetTrigger("attack");
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            hitCounter++;
            animator.SetTrigger("hurt");

            // Update the health slider
            healthSlider.value = maxHits - hitCounter;

            PlaySound(hitSound); // Play hit sound

            if (hitCounter >= maxHits)
            {
                animator.SetTrigger("die");
                PlaySound(dieSound); // Play die sound
                // Load death scene
                SceneManager.LoadScene(deathSceneName);
            }
        }
        else if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
        }
        else if (collision.gameObject.CompareTag("FinishLine"))
        {
            // Load finish line scene
            SceneManager.LoadScene(finishLineSceneName);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = false;
        }
    }
}
