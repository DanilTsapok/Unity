using UnityEngine;
using UnityEngine.UI; 

public class Hero : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField ]private float lives;
    [SerializeField] private float jumpForce;
    [SerializeField] private float damage;
    [SerializeField] private LayerMask groundLayer;

    [Header("Health UI")]
    [SerializeField] private Image[] hearts; 
    [SerializeField] private Sprite fullHeart; 
    [SerializeField] private Sprite emptyHeart; 

    private Rigidbody2D rb;
    private bool isGrounded;

    public static Hero Instance { get; set; }

    public float GetHeroDamage()
    {
        return damage;
    }
  
    public void GetDamage(float EnemyDamage)
    {
        lives -= EnemyDamage; 

        UpdateHealthUI();

        if (lives <= 0)
        {
            Debug.Log("Dead");
            Destroy(gameObject); 
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        rb = GetComponent<Rigidbody2D>();
    }

    private void UpdateHealthUI()
    {
       
        int heartsToDisplay = Mathf.FloorToInt(lives / 20);

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < heartsToDisplay)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart; 
            }

            hearts[i].enabled = i < hearts.Length;
        }
    }

    private void Run()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput != 0)
        {
        
            Vector3 moveDir = new Vector3(horizontalInput, 0, 0);
            transform.position = Vector3.MoveTowards(transform.position, transform.position + moveDir, speed * Time.deltaTime);

            if (horizontalInput > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (horizontalInput < 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
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

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.3f, groundLayer);
    }

    
}
