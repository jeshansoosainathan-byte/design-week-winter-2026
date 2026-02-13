// PlayerHealth.cs
// Attach to the player. Detects kill conditions:
// - "Kill" tagged objects (collision or trigger)
// - Any particle collision
// Fires OnPlayerDied with the player index for GameManager to handle.
// Only one player can die per round.

using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    // Event fired when this player dies — passes the player index
    public static event Action<int> OnPlayerDied;

    // Static lock — only one player can die per round
    private static bool someoneAlreadyDiedThisRound = false;

    private PlayerController controller;
    private bool isDead = false;

    void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead || someoneAlreadyDiedThisRound) return;

        if (collision.gameObject.CompareTag("Kill"))
        {
            Die();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead || someoneAlreadyDiedThisRound) return;

        if (other.CompareTag("Kill"))
        {
            Die();
        }
    }

    void OnParticleCollision(GameObject other)
    {
        if (isDead || someoneAlreadyDiedThisRound) return;

        Die();
    }

    /// <summary>
    /// Call this from a child collider if the particle hits a child object.
    /// </summary>
    public void ParticleHit()
    {
        if (isDead || someoneAlreadyDiedThisRound) return;

        Die();
    }

    void Die()
    {
        if (isDead || someoneAlreadyDiedThisRound) return;
        isDead = true;
        someoneAlreadyDiedThisRound = true;
        Debug.Log($"{gameObject.name} (Player {controller.playerIndex}) was killed!");
        OnPlayerDied?.Invoke(controller.playerIndex);
    }

    /// <summary>
    /// Called by GameManager between rounds to revive the player.
    /// </summary>
    public void ResetHealth()
    {
        isDead = false;
    }

    /// <summary>
    /// Called by GameManager at the start of each round to unlock deaths.
    /// </summary>
    public static void ResetRoundDeathLock()
    {
        someoneAlreadyDiedThisRound = false;
    }
}
