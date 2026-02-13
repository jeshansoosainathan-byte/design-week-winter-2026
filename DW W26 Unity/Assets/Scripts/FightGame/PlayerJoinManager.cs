// PlayerJoinManager.cs
// X (buttonWest) on ANY controller → joins as Player 1 (left character)
// B (buttonEast) on ANY controller → joins as Player 2 (right character)
// Order doesn't matter. Same controller is blocked from being both players.
// Once both join, input is unlocked automatically. GameManager can override this later.

using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class PlayerJoinManager : MonoBehaviour
{
    [Header("Player Prefabs (separate rigs)")]
    public GameObject player1Prefab;  // Left character
    public GameObject player2Prefab;  // Right character

    [Header("Spawn Points")]
    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;

    [Header("Join UI (optional — hides when both joined)")]
    public GameObject joinPromptUI;

    [Header("Ready Text (changes to 'Ready' when controller assigned)")]
    public TMPro.TextMeshProUGUI player1ReadyText;  // Shows "Press X to Ready" → "Ready"
    public TMPro.TextMeshProUGUI player2ReadyText;  // Shows "Press B to Ready" → "Ready"

    // Event that GameManager subscribes to
    public static event Action OnBothPlayersJoined;

    // References to the joined players
    [HideInInspector] public PlayerController player1;
    [HideInInspector] public PlayerController player2;

    private bool player1Joined = false;
    private bool player2Joined = false;
    private bool joinPhaseActive = true;

    // Track which gamepad is claimed by each side
    private Gamepad player1Gamepad = null;
    private Gamepad player2Gamepad = null;

    void Update()
    {
        if (!joinPhaseActive) return;

        foreach (var gamepad in Gamepad.all)
        {
            // --- X (buttonWest) pressed → Player 1 (left) ---
            if (!player1Joined && gamepad.buttonWest.wasPressedThisFrame)
            {
                // Block if this gamepad is already taken by Player 2
                if (player2Gamepad != null && gamepad.deviceId == player2Gamepad.deviceId)
                {
                    Debug.LogWarning("That controller is already used by Player 2!");
                    continue;
                }

                SpawnPlayer1(gamepad);
                return; // One join per frame
            }

            // --- B (buttonEast) pressed → Player 2 (right) ---
            if (!player2Joined && gamepad.buttonEast.wasPressedThisFrame)
            {
                // Block if this gamepad is already taken by Player 1
                if (player1Gamepad != null && gamepad.deviceId == player1Gamepad.deviceId)
                {
                    Debug.LogWarning("That controller is already used by Player 1!");
                    continue;
                }

                SpawnPlayer2(gamepad);
                return; // One join per frame
            }
        }
    }

    void SpawnPlayer1(Gamepad gamepad)
    {
        player1Gamepad = gamepad;
        player1Joined = true;
        if (player1ReadyText != null) player1ReadyText.text = "Ready";
        Debug.Log($"Player 1 (LEFT) registered! Gamepad: {gamepad.displayName}, ID: {gamepad.deviceId}");
        CheckBothJoined();
    }

    void SpawnPlayer2(Gamepad gamepad)
    {
        player2Gamepad = gamepad;
        player2Joined = true;
        if (player2ReadyText != null) player2ReadyText.text = "Ready";
        Debug.Log($"Player 2 (RIGHT) registered! Gamepad: {gamepad.displayName}, ID: {gamepad.deviceId}");
        CheckBothJoined();
    }

    void CheckBothJoined()
    {
        if (player1Joined && player2Joined)
        {
            joinPhaseActive = false;

            if (joinPromptUI != null)
                joinPromptUI.SetActive(false);

            // Do NOT unlock players here — GameManager handles that after the start animation
            OnBothPlayersJoined?.Invoke();
            Debug.Log("Both players joined! Waiting for GameManager to start the game.");
        }
    }

    /// <summary>
    /// Actually instantiate the player prefabs. Called by GameManager after the first level loads.
    /// </summary>
    public void SpawnPlayers(Vector3 p1SpawnPos, Vector3 p2SpawnPos)
    {
        if (player1 == null && player1Gamepad != null)
        {
            GameObject p1Obj = Instantiate(player1Prefab, p1SpawnPos, Quaternion.identity);
            p1Obj.name = "Player 1";

            player1 = p1Obj.GetComponent<PlayerController>();
            player1.assignedGamepad = player1Gamepad;
            player1.playerIndex = 0;
            player1.inputLocked = true;
        }

        if (player2 == null && player2Gamepad != null)
        {
            GameObject p2Obj = Instantiate(player2Prefab, p2SpawnPos, Quaternion.identity);
            p2Obj.name = "Player 2";

            player2 = p2Obj.GetComponent<PlayerController>();
            player2.assignedGamepad = player2Gamepad;
            player2.playerIndex = 1;
            player2.inputLocked = true;
        }
    }

    /// <summary>
    /// Unlocks input for both players.
    /// </summary>
    public void UnlockAllPlayers()
    {
        if (player1 != null) player1.inputLocked = false;
        if (player2 != null) player2.inputLocked = false;
    }

    /// <summary>
    /// Locks input for both players.
    /// </summary>
    public void LockAllPlayers()
    {
        if (player1 != null) player1.inputLocked = true;
        if (player2 != null) player2.inputLocked = true;
    }
}
