using UnityEngine;

public class DestroyTerrain : MonoBehaviour
{
    [Header("Hits Settings")]
    [SerializeField] private int maxHits = 2; //How many hits a platform can survive

    private int currentHits = 0;

    [Header("Explosive Settings")]
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private float playerDamage = 50f;
    [SerializeField] private int platformHits = 1;
    [SerializeField] private LayerMask explosionMask = -1; 
    [SerializeField] private GameObject explosionVFX;
    [SerializeField] private float vfxLifetime = 0.5f;

    void Start()
    {
        currentHits = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            TakeHit();
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Break glass on player exit
        if (CompareTag("Glass") && collision.gameObject.CompareTag("Player"))
        {
            TakeHit();
        }
    }

    public void TakeHit()
    {
        currentHits++;
        if (currentHits >= maxHits)
        {
            //Explode if Explosive platform
            if (CompareTag("Explosive"))
            {
                Explode();
            }
            gameObject.SetActive(false);
        }
    }

    private void Explode()
    {
        //Spawn explosion
        if (explosionVFX != null)
        {
            GameObject vfx = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            vfx.transform.localScale = Vector3.one * (explosionRadius * 0.33f);
            Destroy(vfx, vfxLifetime);
        }

        //AOE Damage
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, explosionMask);
        foreach (Collider2D hit in hits)
        {
            //Damage Players
            if (hit.CompareTag("Player"))
            {
                PlayerController health = hit.GetComponent<PlayerController>();
                if (health != null)
                {
                    health.TakeDamage(playerDamage);
                }
            }
            //Damage Platforms (stone/glass/explosive)
            else if (hit.CompareTag("Stone") || hit.CompareTag("Glass"))
            {
                DestroyTerrain platform = hit.GetComponent<DestroyTerrain>();
                if (platform != null)
                {
                    for (int i = 0; i < platformHits; i++)
                    {
                        platform.TakeHit();
                    }
                }
            }
        }

        //Self-destruct
        Destroy(gameObject);
    }
}
