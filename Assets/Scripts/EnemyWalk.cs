using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float damage;
    [SerializeField] private float knockbackForce = 5f; // Сила отталкивания врага
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    public GameObject canvasPrefab;
    private GameObject canvasInstance;

    private Vector3 direction;
    private Rigidbody2D rb;
    private bool facingRight = true;
    private float damageCooldown = 1f;
    private float lastDamageTime;

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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                Hero.Instance.GetDamage(damage);
                TakeDamage(Hero.Instance.GetHeroDamage(), collision.transform.position);
                lastDamageTime = Time.time;
            }
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
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Исправлено: `velocity`, а не `linearVelocity`
        }
    }

    private void TakeDamage(float damage, Vector3 heroPosition)
    {
        if (hearts.Length > 0)
        {
            int lastIndex = hearts.Length - 1;
            hearts[lastIndex].enabled = false;
            Image[] newHearts = new Image[lastIndex];
            for (int i = 0; i < lastIndex; i++)
            {
                newHearts[i] = hearts[i];
            }
            hearts = newHearts;
        }

        ApplyKnockback(heroPosition);

        if (hearts.Length == 0)
        {
            Debug.Log("Enemy is down");
            Destroy(gameObject);

            if (canvasInstance != null)
            {
                Destroy(canvasInstance);
            }
        }
    }

    private void ApplyKnockback(Vector3 heroPosition)
    {
        if (rb == null) return; // Проверка на null

        Vector2 knockbackDirection = (transform.position - heroPosition).normalized;
        knockbackDirection.y = 0; // Оставляем только горизонтальный отталкивающий импульс
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }

    private void UpdateHealthUI()
    {
        if (hearts == null || hearts.Length == 0)
        {
            return;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = fullHeart;
            hearts[i].enabled = true;
        }
    }
}
