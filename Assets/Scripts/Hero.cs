using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UnitRoot : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] public float lives;
    [SerializeField] private float jumpForce;
    [SerializeField] public float damage;
    [SerializeField] private LayerMask groundLayer;
    [Header("Health UI")]
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    public Animator animator;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isDead = false; // Флаг для отслеживания смерти

    public static UnitRoot Instance { get; private set; }

    public float GetHeroDamage()
    {
        return damage;
    }

    public void GetDamage(float EnemyDamage)
    {
        if (isDead) return; // Если игрок мертв, не наносим урон

        lives -= EnemyDamage;

        UpdateHealthUI();

        if (lives <= 0)
        {
            Debug.Log("Dead");
            isDead = true; // Устанавливаем флаг смерти
            animator.SetBool("4_Death", true); // Запускаем анимацию смерти
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void UpdateHealthUI()
    {
        float heartsToDisplay = lives / 20;

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i < heartsToDisplay ? fullHeart : emptyHeart;
            hearts[i].enabled = i < hearts.Length;
        }
    }

    private void Start()
    {
        animator.SetBool("", true); // Это может быть удалено, так как здесь нет имени анимации
    }

    private void Run()
    {
        if (isDead) return; // Если игрок мертв, не обрабатываем движение

        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput != 0)
        {
            Vector3 moveDir = new Vector3(horizontalInput, 0, 0);
            transform.position = Vector3.MoveTowards(transform.position, transform.position + moveDir, speed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {
        if (!isDead) // Если игрок не мертв, выполняем обработку ввода
        {
            if (Input.GetButton("Horizontal"))
            {
                Run();
            }

            if (isGrounded && Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }
    }

    private void Jump()
    {
        if (isDead) return; // Если игрок мертв, не прыгаем
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.3f, groundLayer);
    }
}
