using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("Componentes")]
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;

    // --- Variables de input ---
    private float moveInput;              // movimiento total (teclado + botones)
    private float moveInputKeyboard;       // solo teclado
    private float moveInputTouch;          // solo botones/canvas
    private bool jumpKeyboard;             // salto por teclado
    private bool jumpTouch;                // salto por botón

    // --- Nuevo Input System ---
    private EntradasMovimiento controles; // tu archivo generado de acciones

    void Awake()
    {
        controles = new EntradasMovimiento();

        // Horizontal del nuevo input
        controles.Juego.Horizontal.performed += ctx =>
        {
            moveInputTouch = ctx.ReadValue<float>();
        };
        controles.Juego.Horizontal.canceled += ctx =>
        {
            moveInputTouch = 0f;
        };

        // Salto del nuevo input
        controles.Juego.Salto.performed += ctx =>
        {
            jumpTouch = true;
        };
        controles.Juego.Salto.canceled += ctx =>
        {
            jumpTouch = false;
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
        // --- Input de teclado (antiguo) ---
        moveInputKeyboard = Input.GetAxisRaw("Horizontal");
        jumpKeyboard = Input.GetButtonDown("Jump");

        // --- Combinar ambos ---
        // Si se pulsa por teclado o por canvas se suman, pero clamp evita que pase de -1 a 1
        moveInput = Mathf.Clamp(moveInputKeyboard + moveInputTouch, -1f, 1f);

        // --- Movimiento horizontal ---
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // --- Flip sprite ---
        if (moveInput > 0) spriteRenderer.flipX = false;
        else if (moveInput < 0) spriteRenderer.flipX = true;

        // --- Ground check ---
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // --- Salto (si viene de teclado o botón) ---
        if ((jumpKeyboard || jumpTouch) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("Jump");
            // Evitar que se quede en true si viene de botón
            jumpTouch = false;
        }

        // --- Parámetros de animación ---
        animator.SetBool("isMoving", moveInput != 0);
        animator.SetBool("isGrounded", isGrounded);
    }
}