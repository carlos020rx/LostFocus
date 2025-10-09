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

    private float inputX;



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
    public AudioSource footstepsAudio,jump, audRecolectado;

    // --- Nuevo Input System ---
    private EntradasMovimiento controles;
    private float moveInput;
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
        float input = moveInput;

        //if (inicioMinijuego)
        //{
        // --- Movimiento por giroscopio/acelerómetro ---
        // El valor de Input.acceleration.x suele estar entre -1 y 1
        // --- Entrada del acelerómetro ---
        float x = Input.acceleration.x * sensibilidad;

        // Usamos la inclinación solo si es significativa
        if (Mathf.Abs(x) > 0.1f)
            inputX = x;
        else
            inputX = 0f;

        // Movimiento
        Player.position += new Vector3(inputX * velocidad * Time.deltaTime, 0f, 0f);

            // Movimiento
            Player.position += new Vector3(inputX * velocidad * Time.deltaTime, 0f, 0f);

            // Animación
            //animator.SetFloat("isMoving2", Mathf.Abs(inputX));

            // Voltear sprite
            voltear();
            //}
            inicioMinijuego = true;
        }

        // Voltear sprite
        voltear();
        //}


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
        float movimientoTotal = Mathf.Max(Mathf.Abs(inputX), Mathf.Abs(moveInput));
        animator.SetFloat("isMoving2", movimientoTotal);
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

        if (nutrientes == 5)
        {
            nutrientesFin = true;

        }
    
    }

    public void voltear()
    {
        if (mirandoDerecha && inputX < 0f || !mirandoDerecha && inputX > 0f)
        {
            mirandoDerecha = !mirandoDerecha;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }

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