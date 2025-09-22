using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;   // Velocidad de movimiento
    public float jumpForce = 7f;   // Fuerza del salto

    [Header("Componentes")]
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [Header("Ground Check")]
    public Transform groundCheck;   // Empty en los pies
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;

    private float moveInput;

    public AudioSource footstepsAudio;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // --- Movimiento Horizontal ---
        moveInput = Input.GetAxisRaw("Horizontal"); // A/D o ←/→
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // --- Voltear el sprite ---
        if (moveInput > 0)
            spriteRenderer.flipX = false;
        else if (moveInput < 0)
            spriteRenderer.flipX = true;

        // --- Revisar si está en el suelo ---
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // --- Saltar ---
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("Jump"); // Activa animación de salto
        }

        // --- Pasar parámetros al Animator ---
        animator.SetBool("isMoving", moveInput != 0);
        animator.SetBool("isGrounded", isGrounded);

        if (isGrounded && moveInput != 0)
        {
            if (!footstepsAudio.isPlaying)
                footstepsAudio.Play();
        }
        else
        {
            if (footstepsAudio.isPlaying)
                footstepsAudio.Stop();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Nutriente"))
        {
            Debug.Log("Lo agarraste");

        }

    }


}