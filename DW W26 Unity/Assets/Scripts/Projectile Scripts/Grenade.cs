using UnityEngine;
using System.Collections;

public class Grenade : MonoBehaviour
{
    [Header("Explosion Timing")]
    [SerializeField] private float fuseTime = 3f; //Time before explosion

    [Header("Explosion")]
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private float playerDamage = 50f;
    [SerializeField] private int platformHits = 1; //Hits to deal to platforms
    [SerializeField] private LayerMask explosionMask = -1; //Filter layers (Default = all)


    private void Start()
    {
        // Start fuse timer
        StartCoroutine(ExplodeAfterDelay());
    }

    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(fuseTime);
        Explode();
    }

    private void Explode()
    {
        //AOE Damage
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, explosionMask);
        foreach (Collider2D hit in hits)
        {
            //Damage Players
            if (hit.CompareTag("Player"))
            {
                DamagePlayer health = hit.GetComponent<DamagePlayer>();
                if (health != null)
                {
                    health.TakeDamage(playerDamage);
                }
            }
            //Damage Platforms (stone/glass)
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
