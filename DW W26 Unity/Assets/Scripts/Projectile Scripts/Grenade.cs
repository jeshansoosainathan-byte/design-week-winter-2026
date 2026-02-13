using UnityEngine;
using System.Collections;

public class Grenade : MonoBehaviour
{
    [Header("Explosion Timing")]
    [SerializeField] private float fuseTime = 3f; //Time before explosion

    [Header("Explosion")]
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private float playerDamage = 2f;
    [SerializeField] private int platformHits = 1; 
    [SerializeField] private LayerMask explosionMask = -1; 

    [Header("Effects")]
    [SerializeField] private GameObject explosionVFX; //Explosion sprite placeholder

    [Header("VFX Settings")]
    [SerializeField] private float vfxLifetime = 0.5f; //How long effect lasts

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
