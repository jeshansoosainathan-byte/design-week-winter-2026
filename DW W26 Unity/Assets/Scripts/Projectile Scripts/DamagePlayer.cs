using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHP = 3f;
    [SerializeField] private float projectileDamage = 1f;

    public float CurrentHP { get; private set; }

    void Start()
    {
        CurrentHP = maxHP;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            TakeDamage(projectileDamage);
            Destroy(collision.gameObject); //Destroy the projectile on hit
        }
    }

    public void TakeDamage(float damage)
    {
        CurrentHP -= damage;
        if (CurrentHP <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        //For now just disables player prefab on death but we can finish this with our actual death logic once we know how we wanna handle respawns and stuff
        gameObject.SetActive(false);
    }
}
