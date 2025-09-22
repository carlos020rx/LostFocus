using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;    // Velocidad de movimiento
    public float jumpForce = 12f;   // Fuerza del salto

    [Header("Componentes")]
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [Header("Ground Check")]
    public Transform groundCheck;   // Empty en los pies
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("Sonidos")]
    public AudioSource footstepsAudio;
    public AudioSource jump;

    // --- Nuevo Input System ---
    private EntradasMovimiento controles;
    private float moveInput;
    private bool jumpPressed;

    void Awake()
    {
        controles = new EntradasMovimiento();

        // Movimiento horizontal (teclado + botones del canvas)
        controles.Juego.Horizontal.performed += ctx =>
        {
            moveInput = ctx.ReadValue<float>();
        };
        controles.Juego.Horizontal.canceled += ctx =>
        {
            moveInput = 0f;
        };

        // Salto (teclado + botón)
        controles.Juego.Salto.performed += ctx =>
        {
            jumpPressed = true;
        };
    }

    void OnEnable()
    {
        controles.Juego.Enable();
    }

    void OnDisable()
    {
        controles.Juego.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // --- Movimiento horizontal ---
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // --- Voltear el sprite ---
        if (moveInput > 0)
            spriteRenderer.flipX = false;
        else if (moveInput < 0)
            spriteRenderer.flipX = true;

        // --- Revisar si está en el suelo ---
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // --- Saltar ---
        if (jumpPressed && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("Jump");
            jump.Play();
            jumpPressed = false; // evita múltiples saltos en una sola pulsación
        }

        // --- Parámetros del Animator ---
        animator.SetBool("isMoving", Mathf.Abs(moveInput) > 0.01f);
        animator.SetBool("isGrounded", isGrounded);

        // --- Sonido de pasos ---
        if (isGrounded && Mathf.Abs(moveInput) > 0.01f)
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