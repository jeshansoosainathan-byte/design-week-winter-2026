// DestructiblePlatform.cs
// A platform that takes damage from particles and tag-based collisions.
// Swaps sprites per damage state, and spawns a prefab on destruction.
// Resets to default when parent GameObject is disabled.

using UnityEngine;
using System.Collections;

public class DestructiblePlatform : MonoBehaviour
{
    [Header("Damage States")]
    [Tooltip("Sprites for each state. Index 0 = default/full health. Last index = final state before destruction.")]
    public Sprite[] damageSprites;
    [Tooltip("How many hits to destroy. Must be <= damageSprites.length")]
    public int hitsToDestroy = 3;

    [Header("On Destroy")]
    [Tooltip("Prefab spawned when the platform is destroyed (debris, particles, etc.)")]
    public GameObject destroySpawnPrefab;

    [Header("Tag Collision")]
    [Tooltip("Tag that triggers collision-based damage or timed destruction")]
    public string triggerTag = "";

    public enum TagBehavior
    {
        None,                   // Tag collision does nothing
        DamageOnEnterAndExit,   // 1 damage on enter, 1 damage on exit
        TimedDestruction        // Activate, wait, then destroy leaving a prefab
    }

    [Tooltip("What happens when the assigned tag collides with this platform")]
    public TagBehavior tagBehavior = TagBehavior.None;

    [Header("Timed Destruction (only if TagBehavior = TimedDestruction)")]
    [Tooltip("Seconds to wait before destroying after tag collision")]
    public float timedDestructionDelay = 1f;
    [Tooltip("Prefab spawned specifically for timed destruction (uses destroySpawnPrefab if null)")]
    public GameObject timedDestroyPrefab;

    // Internal state
    private int currentHits;
    private SpriteRenderer sr;
    private Collider2D col;
    private bool isDestroyed;
    private bool timedDestructionActive;

    [Header("Audio")]
    [Tooltip("Which speaker the damage sound plays from")]
    public bool rightSpeaker = false;

    // Cached defaults for reset
    private Sprite defaultSprite;
    private bool defaultColliderState;
    private AudioSource audioSource;

    void Awake()
    {
        // SpriteRenderer is on a child named "Txt"
        Transform txtChild = transform.Find("Txt");
        if (txtChild != null)
        {
            sr = txtChild.GetComponent<SpriteRenderer>();
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: No child named 'Txt' found! Falling back to self.");
            sr = GetComponent<SpriteRenderer>();
        }

        col = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();

        // Cache defaults
        defaultSprite = sr.sprite;
        defaultColliderState = col.enabled;
    }

    void OnEnable()
    {
        ResetPlatform();

        // Set stereo pan: -1 = full left, 1 = full right
        if (audioSource != null)
        {
            audioSource.panStereo = rightSpeaker ? 1f : -1f;
        }
    }

    // =====================
    // PARTICLE COLLISION
    // =====================

    void OnParticleCollision(GameObject other)
    {
        if (isDestroyed) return;

        // If tag behavior is DamageOnEnterAndExit, particles instantly destroy
        if (tagBehavior == TagBehavior.DamageOnEnterAndExit)
        {
            DestroyPlatform();
        }
        else
        {
            TakeDamage();
        }
    }

    // =====================
    // KILL & RADIUS TAGS
    // =====================

    // "Kill" — instantly destroys this block (player death handled by PlayerHealth)
    // "Radius" — damages this block by 1 (does NOT hurt the player)

    void HandleSpecialTags(GameObject other)
    {
        if (isDestroyed) return;

        if (other.CompareTag("Kill"))
        {
            DestroyPlatform();
        }
        else if (other.CompareTag("Radius"))
        {
            TakeDamage();
        }
    }

    // =====================
    // TAG COLLISION
    // =====================

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDestroyed) return;

        // Check special tags first
        HandleSpecialTags(collision.gameObject);

        if (tagBehavior == TagBehavior.None) return;
        if (string.IsNullOrEmpty(triggerTag)) return;
        if (!collision.gameObject.CompareTag(triggerTag)) return;

        switch (tagBehavior)
        {
            case TagBehavior.DamageOnEnterAndExit:
                TakeDamage();
                break;

            case TagBehavior.TimedDestruction:
                if (!timedDestructionActive)
                {
                    StartCoroutine(TimedDestruction());
                }
                break;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (isDestroyed) return;
        if (tagBehavior == TagBehavior.None) return;
        if (string.IsNullOrEmpty(triggerTag)) return;
        if (!collision.gameObject.CompareTag(triggerTag)) return;

        if (tagBehavior == TagBehavior.DamageOnEnterAndExit)
        {
            TakeDamage();
        }
    }

    // Also support trigger colliders
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDestroyed) return;

        // Check special tags first
        HandleSpecialTags(other.gameObject);

        if (tagBehavior == TagBehavior.None) return;
        if (string.IsNullOrEmpty(triggerTag)) return;
        if (!other.CompareTag(triggerTag)) return;

        switch (tagBehavior)
        {
            case TagBehavior.DamageOnEnterAndExit:
                TakeDamage();
                break;

            case TagBehavior.TimedDestruction:
                if (!timedDestructionActive)
                {
                    StartCoroutine(TimedDestruction());
                }
                break;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (isDestroyed) return;
        if (tagBehavior == TagBehavior.None) return;
        if (string.IsNullOrEmpty(triggerTag)) return;
        if (!other.CompareTag(triggerTag)) return;

        if (tagBehavior == TagBehavior.DamageOnEnterAndExit)
        {
            TakeDamage();
        }
    }

    // =====================
    // DAMAGE
    // =====================

    public void TakeDamage()
    {
        if (isDestroyed) return;

        currentHits++;

        if (currentHits >= hitsToDestroy)
        {
            DestroyPlatform();
        }
        else
        {
            UpdateSprite();
            PlayDamageSound();
        }
    }

    void UpdateSprite()
    {
        if (damageSprites == null || damageSprites.Length == 0) return;

        // Only use as many sprites as hitsToDestroy allows
        int maxIndex = Mathf.Min(hitsToDestroy, damageSprites.Length) - 1;
        int spriteIndex = Mathf.Clamp(currentHits, 0, maxIndex);
        sr.sprite = damageSprites[spriteIndex];
    }

    void PlayDamageSound()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

    void DestroyPlatform()
    {
        isDestroyed = true;

        // Spawn the destruction prefab at this position
        if (destroySpawnPrefab != null)
        {
            Instantiate(destroySpawnPrefab, transform.position, Quaternion.identity);
        }

        // Hide the platform — don't actually Destroy() so it can reset later
        sr.enabled = false;
        col.enabled = false;
    }

    // =====================
    // TIMED DESTRUCTION
    // =====================

    IEnumerator TimedDestruction()
    {
        timedDestructionActive = true;

        // Advance sprite to next damage state
        currentHits++;
        UpdateSprite();
        PlayDamageSound();

        yield return new WaitForSeconds(timedDestructionDelay);

        if (isDestroyed) yield break;

        isDestroyed = true;

        // Spawn the timed prefab (or fall back to the normal destroy prefab)
        GameObject prefabToSpawn = timedDestroyPrefab != null ? timedDestroyPrefab : destroySpawnPrefab;
        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        }

        sr.enabled = false;
        col.enabled = false;
    }

    // =====================
    // RESET
    // =====================

    public void ResetPlatform()
    {
        StopAllCoroutines();

        currentHits = 0;
        isDestroyed = false;
        timedDestructionActive = false;

        // Restore visuals
        sr.enabled = true;
        sr.sprite = defaultSprite;

        // Restore collider
        col.enabled = defaultColliderState;
    }
}
