using UnityEngine;
using TMPro;
using System.Collections;
public class ArtifactHeal : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI HealthPlus;
    [SerializeField] private float heal;
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == UnitRoot.Instance?.gameObject)
        {
            UnitRoot.Instance.speed += speed;
            UnitRoot.Instance.damage += damage;
            UnitRoot.Instance.jumpForce += jumpSpeed;
            if (HealthPlus != null)
            {
                UnitRoot.Instance.UpdateHealthUI();

                HealthPlus.text = "+" + heal;
                
            }
            spriteRenderer.enabled = false;
            Destroy(gameObject);
            if (UnitRoot.Instance != null)
            {
                UnitRoot.Instance.lives += heal;
                UnitRoot.Instance.UpdateHealthUI(); 
            }
            
        }
    }
    private IEnumerator FadeOutHealText()
    {
        Color color = HealthPlus.color;
        float duration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, elapsedTime / duration);
            HealthPlus.color = color;
            yield return null;
        }

        HealthPlus.text = "";
        color.a = 1;
        HealthPlus.color = color;
    }

}