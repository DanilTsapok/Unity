using UnityEngine;

public class ArtifactHeal : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == UnitRoot.Instance.gameObject)
        {
            spriteRenderer.enabled = false;
            Destroy(gameObject);
            UnitRoot.Instance.lives += 60;
            UnitRoot.Instance.UpdateHealthUI();
        }
    }
}