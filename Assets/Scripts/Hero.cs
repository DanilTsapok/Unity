using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class UnitRoot : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public float lives;
    [SerializeField] public float jumpForce;
    [SerializeField] public float damage;
    [SerializeField] private LayerMask groundLayer;

    [Header("Health UI")]
    [SerializeField] public Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] public GameObject weaponPrint;
    public bool isPaused = false;
    AudioManager audioManager;

    public Animator animator;
    public Rigidbody2D rb;
    private bool isDead = false;
    private bool facingRight = true;
    public SpriteRenderer leftHandWithWeapon;
    public SpriteRenderer rightHandWithShield;
    public float heartsToDisplay;
    private Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>();
    public static UnitRoot Instance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null, true);
            DontDestroyOnLoad(gameObject);
  
        }
        else
        {
            Destroy(gameObject);
        }

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is missing from UnitRoot!");
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator is missing from UnitRoot!");
        }

        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found! Make sure it exists in the scene.");
        }
        LoadKeyBindings();

    }


    public void LoadKeyBindings()
    {
        keyBindings["MoveLeft"] = ParseKeyCode(PlayerPrefs.GetString("MoveLeftKey", "A"));
        keyBindings["MoveRight"] = ParseKeyCode(PlayerPrefs.GetString("MoveRightKey", "D"));
        keyBindings["Jump"] = ParseKeyCode(PlayerPrefs.GetString("JumpKey", "Space"));
        keyBindings["Attack"] = ParseKeyCode(PlayerPrefs.GetString("AttackKey", "Mouse0"));
       
    }

    public void UpdateKeyBinding(string action, KeyCode keyCode)
    {
        PlayerPrefs.SetString(action, keyCode.ToString());
        PlayerPrefs.Save();
        LoadKeyBindings();
    }
    private KeyCode ParseKeyCode(string key)
    {
        key = key.ToUpper();
        if (Enum.TryParse(key, out KeyCode result))
        {
            return result;
        }
        switch (key)
        {
            case "SPACE":
                return KeyCode.Space;
            case "MOUSE0":
                return KeyCode.Mouse0;
            case "MOUSE1":
                return KeyCode.Mouse1;
            case "MOUSE2":
                return KeyCode.Mouse2;
            default:
                return KeyCode.A;
        }
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
            audioManager.PlaySFX(audioManager.dead);
            rb.simulated = false;
            isPaused = true;
            StartCoroutine(DefeatGame());
        }
    }

    private IEnumerator DefeatGame()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<Collider2D>().enabled = false;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }

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
     
        heartsToDisplay = Mathf.Clamp(lives / 20, 0, hearts.Length);

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i < heartsToDisplay ? fullHeart : emptyHeart;
            hearts[i].enabled = i < hearts.Length;
        }
    }

    private void Start()
    {
        weaponPrint.SetActive(false);
        animator.SetBool("1_Move", false);
     
        PlayerPrefs.SetFloat("Hero_PosX_Slot" + 5, transform.position.x);
        PlayerPrefs.SetFloat("Hero_PosY_Slot" + 5, transform.position.y);
        PlayerPrefs.SetFloat("Hero_PosZ_Slot" + 5, transform.position.z);

    }


    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }


    private void Update()
    {
       
        if (isPaused == true)
        {
            rb.simulated = false;
            return;
        }
        if (!isDead)
        {
            if (Input.GetKey(keyBindings["MoveLeft"]))
            {
                Move(-1);
            }
            else if (Input.GetKey(keyBindings["MoveRight"]))
            {
                Move(1);
            }
            else
            {
                animator.SetBool("1_Move", false);
            }

            if (Input.GetKeyDown(keyBindings["Jump"]))
            {
                Jump();
                audioManager.PlaySFX(audioManager.jump);
            }

            if (Input.GetKeyDown(keyBindings["Attack"]))
            {
                Attack();
                audioManager.PlaySFX(audioManager.attack);
            }

        
        }
    }


    private void Move(int direction)
    {
        if (isDead) return;

        Vector3 moveDir = new Vector3(direction, 0, 0);
        transform.position = Vector3.MoveTowards(transform.position, transform.position + moveDir, speed * Time.deltaTime);
        animator.SetBool("1_Move", true);
        weaponPrint.SetActive(false);
        if (direction < 0 && facingRight)
        {
            Flip();
        }
        else if (direction > 0 && !facingRight)
        {
            Flip();
        }
    }

    private void Attack()
    {
        weaponPrint.SetActive(true);
        animator.SetBool("2_Attack", true);
    }

    private void Jump()
    {
        if (isDead) return;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    //private void Defance()
    //{
    //    if (isDead) return;
    //    Enemy.Instance.damage = 0;
    //    animator.SetBool("2_Attack", true);
    //}
   
}
