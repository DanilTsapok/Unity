using UnityEngine;

public class Hero  : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    private float lives = 100f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private LayerMask groundLayer; 

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private bool isGrounded;


    public static Hero Instance { get; set; }

    public void GetDamage()
    {
        lives-= 20;
        Debug.Log("Получен урон" + lives);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; 
        }
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Run()
    {
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
        sprite.flipX = dir.x < 0;
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
