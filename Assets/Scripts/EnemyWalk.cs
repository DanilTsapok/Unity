using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int lives = 30;
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float jumpForce = 5f;

    [SerializeField] private Image[] hearts;  
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    public GameObject canvasPrefab;
    private GameObject canvasInstance;

    private Vector3 direction;
    private Rigidbody2D rb;

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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage();
            TakeDamage(10); 
        }
        else if (collision.contacts[0].normal.y < -0.5f)
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

 
    private void TakeDamage(int damage)
    {
        lives -= damage;
        UpdateHealthUI();

        if (lives <= 0)
        {
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

        int heartsToDisplay = Mathf.CeilToInt(lives / 10f);  
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
}