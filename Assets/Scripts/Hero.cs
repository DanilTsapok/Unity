using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class UnitRoot : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] public float lives;
    [SerializeField] private float jumpForce;
    [SerializeField] public float damage;
    [SerializeField] private LayerMask groundLayer;

    [Header("Health UI")]
    [SerializeField] public Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private TextMeshProUGUI damageText;

    VolumeSettings volumeSettings;

    AudioManager audioManager;


    public Animator animator;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isDead = false;
    private bool facingRight = true; 

    public static UnitRoot Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public float GetHeroDamage()
    {
        return damage;
    }

    public void GetDamage(float enemyDamage)
    {
        if (isDead) return;
       
        lives -= enemyDamage;
        UpdateHealthUI();
        damageText.text = "-" + enemyDamage.ToString();

        StartCoroutine(FadeOutDamageText());

        if (lives <= 0)
        {
            Debug.Log("Dead");
            isDead = true;
            animator.SetBool("4_Death", true);
            //StartCoroutine(DefeatGame());
        }
    }

   

    private IEnumerable DefeatGame()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        SceneManager.LoadScene("Defeat");

    }
    private IEnumerator FadeOutDamageText()
    {
        Color color = damageText.color;
        float duration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, elapsedTime / duration);
            damageText.color = color;
            yield return null;
        }

        damageText.text = "";
        color.a = 1;
        damageText.color = color;
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
        animator.SetBool("1_Move", false);
        //volumeSettings.LoadVolume();
    }


    private void Run()
    {
        if (isDead) return;

        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput != 0)
        {
            Vector3 moveDir = new Vector3(horizontalInput, 0, 0);
            transform.position = Vector3.MoveTowards(transform.position, transform.position + moveDir, speed * Time.deltaTime);
            animator.SetBool("1_Move", true);

            if (horizontalInput < 0 && facingRight)
            {
                Flip();
            }
            else if (horizontalInput > 0 && !facingRight)
            {
                Flip();
            }
        }
        else
        {
            animator.SetBool("1_Move", false);
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(facingRight ? 1 : -1, transform.localScale.y, transform.localScale.z);
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {
        if (!isDead)
        {
            if (Input.GetButton("Horizontal"))
            {
                Run();
                
            }

            if (!Input.GetButton("Horizontal"))
            {
                animator.SetBool("1_Move", false);
            }

            if (isGrounded && Input.GetButtonDown("Jump"))
            {
                Jump();
                
            }

            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                audioManager.PlaySFX(audioManager.attack);
              
            }

            if (Input.GetMouseButtonDown(1))
            {
                Defance();
            }
        }
    }
    private void Defance()
    {
        Enemy.Instance.damage = 0f;

    }
    private void Attack()
    {
        Debug.Log("Attack");
        animator.SetBool("2_Attack", true);
    }

    private void Jump()
    {
        if (isDead) return;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.3f, groundLayer);
    }
}