using UnityEngine;

public class BasicProjectiles : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float damage = 1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }
}
