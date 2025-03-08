using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Animator animator;
    public static Weapon Instance { get; private set; }
    void Start()
    {
       
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject == Enemy.Instance.gameObject)
    //    {
    //        Enemy.Instance.TakeDamage(UnitRoot.Instance.GetHeroDamage(), collision.transform.position);
    //    }

    //}
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
       
      
    }

    void Update()
    {
        
    }
}
