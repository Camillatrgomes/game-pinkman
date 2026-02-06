using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 50f;

    [Header("Componentes")]
    private Rigidbody2D rig;
    private Animator animator;
    private SpriteRenderer sprite;

    private float alturaInicialQueda;
    private bool estaCaindo;
    private bool estaMorto;

    private bool isGrounded;
    private bool doubleJump;
    private float direction;

    [Header("Moedas")]
    private int coinCounter = 0;
    public TMP_Text counterText;

    private InputAction moveAction;
    private InputAction jumpAction;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rig = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        moveAction = new InputAction("Move");
        moveAction.AddCompositeBinding("1DAxis")
            .With("Negative", "<Keyboard>/leftArrow")
            .With("Positive", "<Keyboard>/rightArrow");

        jumpAction = new InputAction("Jump", binding: "<Keyboard>/space");

        moveAction.Enable();
        jumpAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }

    void Update()
    {

    if (transform.position.y <= -36.46f && !estaMorto)
    {
        Morrer();
    }

        direction = moveAction.ReadValue<float>();
        animator.SetBool("andando", direction != 0);

        // Detecta início da queda
        if (rig.linearVelocity.y < 0 && !isGrounded && !estaCaindo)
        {
            alturaInicialQueda = transform.position.y;
            estaCaindo = true;
        }

        if (jumpAction.WasPressedThisFrame())
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        if (estaMorto) return;

        rig.linearVelocity = new Vector2(direction * speed, rig.linearVelocity.y);

        if (direction < 0) sprite.flipX = true;
        else if (direction > 0) sprite.flipX = false;
    }

    void Jump()
    {
        if (isGrounded)
        {
            rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("pulando", true);

        }
        else if (doubleJump)
        {
            rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("pulando", true);
            doubleJump = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            doubleJump = true;
            animator.SetBool("pulando", false);
        }
        else if (collision.gameObject.CompareTag("morte"))
        {
            Morrer();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin") && collision.gameObject.activeSelf)
        {
            collision.gameObject.SetActive(false);
            coinCounter++;
            counterText.text = "Coins: " + coinCounter;
        }
        else if (collision.CompareTag("morte"))
        {
            Morrer();
        }
    }

    void Morrer()
    {
        if (estaMorto) return;

        estaMorto = true;

        rig.linearVelocity = Vector2.zero;
        rig.simulated = false;

        animator.SetTrigger("morrer");

        moveAction.Disable();
        jumpAction.Disable();

        Invoke(nameof(ReiniciarJogo), 2f);
    }


    void ReiniciarJogo()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
