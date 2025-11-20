using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;    // Velocidad de movimiento
    public float jumpForce = 12f;   // Fuerza del salto
    public Transform Player;
    private float horizontal;
    private bool mirandoDerecha = true;
    private float x;
    public float velocidad = 5f;
    private float contador = 3;
    public float inputX;



    [Header("Componentes")]
    private Rigidbody2D rb;
    private Animator animator;
    public SpriteRenderer spriteRenderer;


    [Header("Ground Check")]
    public Transform groundCheck;   // Empty en los pies
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("Sonidos")]
    public AudioSource footstepsAudio, jump, audRecolectado;

    // --- Nuevo Input System ---
    private EntradasMovimiento controles;
    public float moveInput;
    private bool jumpPressed;

    private float sensibilidad = 2.0f;

    [Header("Otros")]
    private float nutrientes;
    public TMP_Text textoNutriente;
    public Slider nutrienteSlider;
    private float nutrienteMax = 5f;
    private float nutrienteActual;

    public bool inicioMinijuego = false;

    public bool nutrientesFin = false;


    public PopupTester popupTester;

    public GameManager gameManager;

    public caidaAlimentos caidaAlimentos;

    public DialogueManager dialogueManager;

    public Spawner spawner;
    public bool EnMiniJuego1 = true;




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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        textoNutriente.text = "0";
        nutrienteSlider.maxValue = nutrienteMax;
        nutrienteActual = 0;
        nutrienteSlider.value = nutrienteActual;
    }

    void OnEnable()
    {
        controles.Juego.Enable();
    }

    void OnDisable()
    {
        controles.Juego.Disable();
    }

    void Update()
    {

        // --- BLOQUEO GENERAL DE INPUTS ---
        if (dialogueManager.isDialogueActive || spawner.minijuego2)
        {
            gameManager.desactivarBotones();
            return; // bloquea todo movimiento
        }

        // -----------------------------
        // 🔵 MOVIMIENTO EN MINIJUEGO 1
        // -----------------------------
        if (popupTester.terminoMensaje1 && EnMiniJuego1)
        {
            Debug.Log("En minijuego");
            gameManager.desactivarBotones();

            float x = Input.acceleration.x * sensibilidad;

            if (Mathf.Abs(x) > 0.1f)
                inputX = x;
            else
                inputX = 0f;

            Player.position += new Vector3(inputX * velocidad * Time.deltaTime, 0f, 0f);

            voltear(); // orienta sprite

            inicioMinijuego = true;

            // animaciones
            animator.SetFloat("isMoving2", Mathf.Abs(inputX));
            animator.SetBool("isGrounded", isGrounded);

            return; // muy importante: evita que el movimiento normal se ejecute
        }


        // -----------------------------
        // 🟢 MOVIMIENTO NORMAL
        // -----------------------------
        gameManager.activarBotones();

        // movimiento horizontal normal
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // voltear sprite
        if (moveInput > 0) spriteRenderer.flipX = false;
        else if (moveInput < 0) spriteRenderer.flipX = true;

        // revisar si está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // saltar
        if (jumpPressed && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("Jump");
            jump.Play();
            jumpPressed = false;
        }

        // animaciones
        float movimientoTotal = Mathf.Max(Mathf.Abs(inputX), Mathf.Abs(moveInput));
        animator.SetFloat("isMoving2", movimientoTotal);
        animator.SetBool("isGrounded", isGrounded);

        // sonido de pasos
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

        // lógica final de nutrientes
        if (nutrientes == 5 && contador == 3)
        {
            nutrientesFin = true;
            contador = 2;
        }

    }

    public void voltear()
    {
     if (inputX > 0)
        spriteRenderer.flipX = false;
    else if (inputX < 0)
        spriteRenderer.flipX = true;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Nutriente"))
        {
            Debug.Log("Lo agarraste");
            Destroy(collision.gameObject);
            audRecolectado.Play();
            nutrientes++;
            textoNutriente.text = nutrientes.ToString();
            nutrienteActual = nutrientes;
            nutrienteSlider.value = nutrienteActual;
        }

        if (collision.CompareTag("alimento"))
        {
            Debug.Log("te mato");
            animator.SetTrigger("dano");
            caidaAlimentos.vida--;
        }

        if (collision.CompareTag("coleccionable"))
        {
            Debug.Log("Lo agarraste");
            Destroy(collision.gameObject);
            audRecolectado.Play();
            popupTester.showMessage7 = true;
        }
    }
}