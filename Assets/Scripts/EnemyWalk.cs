using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float lives;
    
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float damage;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    public GameObject canvasPrefab;
    private GameObject canvasInstance;

    private Vector3 direction;
    private Rigidbody2D rb;
    private bool facingRight = true; 

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (canvasPrefab != null)
        {
            canvasInstance = Instantiate(canvasPrefab, transform);
            canvasInstance.transform.localPosition = new Vector3(0, 2f, 0);
            hearts = canvasInstance.GetComponentsInChildren<Image>();
            UpdateHealthUI();
        }
        else
        {
            Debug.LogWarning("Canvas Prefab не назначен!");
        }
    }

    private void Update()
    {
        if (Hero.Instance != null)
        {
            MoveTowardsHero();
        }
    }

    private void MoveTowardsHero()
    {
        Vector3 heroPosition = Hero.Instance.transform.position;
        direction = (heroPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        CheckFlip(heroPosition.x);
    }

    private void CheckFlip(float heroX)
    {
        bool shouldFaceRight = heroX > transform.position.x;

        if (shouldFaceRight == facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(facingRight ? 1 : -1, transform.localScale.y, transform.localScale.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage(damage);
            TakeDamage(Hero.Instance.GetHeroDamage());
        }
        else if (collision.contacts[0].normal.y < -0.7f)
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void TakeDamage(float damage)
    {
        lives -= damage;

        UpdateHealthUI();

        if (lives < 0)
        {
            Debug.Log("Enemy is down");
            Destroy(gameObject);

            if (canvasInstance != null)
            {
                Destroy(canvasInstance);
            }
        }
    }

    private void UpdateHealthUI()
    {
        if (hearts == null || hearts.Length == 0)
        {
            return;
        }

        int heartsToDisplay = Mathf.CeilToInt(lives / hearts.Length);
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i < heartsToDisplay ? fullHeart : emptyHeart;
            hearts[i].enabled = i < hearts.Length;
        }
    }
}
