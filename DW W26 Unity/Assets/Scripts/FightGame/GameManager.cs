// GameManager.cs
// Controls the entire game flow: join → start → rounds → transitions → win → title screen.
// Manages scoring, map selection, spawn points, animator bools, and UI text.

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public PlayerJoinManager joinManager;
    public Animator transitionAnimator;       // Animator with bools: start, game, levelLoad

    [Header("Level Layouts")]
    [Tooltip("Parent GameObjects containing platforms. Only one active at a time.")]
    public GameObject[] levelLayouts;

    [Header("Spawn Points Per Level")]
    [Tooltip("Must match levelLayouts array. Each entry has P1 and P2 spawn points for that level.")]
    public LevelSpawnPoints[] levelSpawnPoints;

    [Header("UI Text")]
    public TextMeshProUGUI leftResultText;    // Shows WINNER/LOSER on left side
    public TextMeshProUGUI rightResultText;   // Shows WINNER/LOSER on right side

    [Header("Score UI")]
    public TextMeshProUGUI player1ScoreText;  // Displays P1 score
    public TextMeshProUGUI player2ScoreText;  // Displays P2 score

    [Header("Settings")]
    public int pointsToWin = 10;
    public float transitionDuration = 4f;
    public float finalWinDisplayTime = 7f;
    public string titleSceneName = "TitleScreen";

    // Scores
    private int player1Score = 0;
    private int player2Score = 0;

    // Level selection — tracks which haven't been used yet
    private List<int> availableLevels = new List<int>();
    private int currentLevelIndex = -1;

    // State
    private bool gameStarted = false;
    private bool gameOver = false;

    // Player references (grabbed from JoinManager after join)
    private PlayerController p1;
    private PlayerController p2;
    private PlayerHealth p1Health;
    private PlayerHealth p2Health;

    void OnEnable()
    {
        PlayerJoinManager.OnBothPlayersJoined += OnBothPlayersJoined;
        PlayerHealth.OnPlayerDied += OnPlayerDied;
    }

    void OnDisable()
    {
        PlayerJoinManager.OnBothPlayersJoined -= OnBothPlayersJoined;
        PlayerHealth.OnPlayerDied -= OnPlayerDied;
    }

    void Start()
    {
        // Disable all level layouts at start
        foreach (var layout in levelLayouts)
        {
            if (layout != null) layout.SetActive(false);
        }

        // Initialize scores
        UpdateScoreUI();

        // Clear result text
        if (leftResultText != null) leftResultText.text = "";
        if (rightResultText != null) rightResultText.text = "";

        // Set start bool — triggers the join/ready UI animation
        if (transitionAnimator != null)
        {
            transitionAnimator.SetBool("start", true);
        }
    }

    // =====================
    // JOIN FLOW
    // =====================

    void OnBothPlayersJoined()
    {
        // Players are registered but NOT spawned yet
        // Disable start bool — animation brings UI down
        if (transitionAnimator != null)
        {
            transitionAnimator.SetBool("start", false);
        }

        StartCoroutine(StartFirstRound());
    }

    IEnumerator StartFirstRound()
    {
        // Wait for the start animation to finish
        yield return new WaitForSeconds(2f);

        // Begin the first round — this will load the level and THEN spawn players
        StartCoroutine(BeginRound());
    }

    // =====================
    // ROUND FLOW
    // =====================

    IEnumerator BeginRound()
    {
        gameStarted = true;

        // Lock players (if they exist yet)
        joinManager.LockAllPlayers();

        // Select and activate the first map
        SelectRandomLevel();

        // Wait for the level to fully initialize
        yield return new WaitForSeconds(0.5f);

        // Spawn the player prefabs now that the level is loaded
        LevelSpawnPoints spawns = levelSpawnPoints[currentLevelIndex];
        joinManager.SpawnPlayers(spawns.player1Spawn.position, spawns.player2Spawn.position);

        // Grab references now that players exist
        p1 = joinManager.player1;
        p2 = joinManager.player2;
        p1Health = p1.GetComponent<PlayerHealth>();
        p2Health = p2.GetComponent<PlayerHealth>();

        // Reset health
        p1Health.ResetHealth();
        p2Health.ResetHealth();
        PlayerHealth.ResetRoundDeathLock();

        // Wait remaining time to hit 3 seconds total (0.5 already passed)
        yield return new WaitForSeconds(2.5f);

        // Enable levelLoad
        if (transitionAnimator != null)
        {
            transitionAnimator.SetBool("levelLoad", true);
        }

        // Wait 3 seconds after levelLoad
        yield return new WaitForSeconds(3f);

        if (transitionAnimator != null)
        {
            transitionAnimator.SetBool("game", false);
            transitionAnimator.SetBool("levelLoad", false);
        }

        // Clear result text
        if (leftResultText != null) leftResultText.text = "";
        if (rightResultText != null) rightResultText.text = "";

        // Unlock players — round is live!
        joinManager.UnlockAllPlayers();

        Debug.Log("Round started! FIGHT!");
    }

    // =====================
    // DEATH / SCORING
    // =====================

    void OnPlayerDied(int deadPlayerIndex)
    {
        if (gameOver) return;

        // Award point to the opposite player
        if (deadPlayerIndex == 0)
        {
            player2Score++;
            if (leftResultText != null) leftResultText.text = "LOSER";
            if (rightResultText != null) rightResultText.text = "WINNER";
            Debug.Log($"Player 1 died! Player 2 scores. ({player1Score} - {player2Score})");
        }
        else
        {
            player1Score++;
            if (leftResultText != null) leftResultText.text = "WINNER";
            if (rightResultText != null) rightResultText.text = "LOSER";
            Debug.Log($"Player 2 died! Player 1 scores. ({player1Score} - {player2Score})");
        }

        UpdateScoreUI();

        // Lock players immediately
        joinManager.LockAllPlayers();

        // Check for game winner
        if (player1Score >= pointsToWin || player2Score >= pointsToWin)
        {
            StartCoroutine(GameWon());
        }
        else
        {
            StartCoroutine(RoundTransition());
        }
    }

    // =====================
    // ROUND TRANSITION
    // =====================

    IEnumerator RoundTransition()
    {
        // Enable game bool — triggers transition animation
        if (transitionAnimator != null)
        {
            transitionAnimator.SetBool("game", true);
        }

        // Hide players immediately so they don't fall into the abyss during transition
        HidePlayers();

        // Wait 4 seconds of transition
        yield return new WaitForSeconds(4f);

        // Disable current level (LevelLayoutResetter resets platforms via OnDisable)
        if (currentLevelIndex >= 0 && currentLevelIndex < levelLayouts.Length)
        {
            levelLayouts[currentLevelIndex].SetActive(false);
        }

        // Load new level
        SelectRandomLevel();

        yield return new WaitForSeconds(0.5f);

        // Respawn and reset
        RespawnPlayers();
        p1Health.ResetHealth();
        p2Health.ResetHealth();
        PlayerHealth.ResetRoundDeathLock();

        // Show players now that they're safely on the new level
        ShowPlayers();

        // Wait the remaining 1.5 seconds to hit 6 seconds total (4 + 0.5 + 1.5 = 6)
        yield return new WaitForSeconds(1.5f);

        // NOW enable levelLoad
        if (transitionAnimator != null)
        {
            transitionAnimator.SetBool("levelLoad", true);
        }

        // Wait 3 seconds after levelLoad
        yield return new WaitForSeconds(3f);

        if (transitionAnimator != null)
        {
            transitionAnimator.SetBool("game", false);
            transitionAnimator.SetBool("levelLoad", false);
        }

        // Clear result text
        if (leftResultText != null) leftResultText.text = "";
        if (rightResultText != null) rightResultText.text = "";

        // Unlock players — round is live!
        joinManager.UnlockAllPlayers();

        Debug.Log("Round started! FIGHT!");
    }

    // =====================
    // GAME WON
    // =====================

    IEnumerator GameWon()
    {
        gameOver = true;

        // Enable game bool for the final transition
        if (transitionAnimator != null)
        {
            transitionAnimator.SetBool("game", true);
        }

        // Show final winner text — both sides show the same winner
        if (player1Score >= pointsToWin)
        {
            if (leftResultText != null) leftResultText.text = "PURGATORY WINS";
            if (rightResultText != null) rightResultText.text = "PURGATORY WINS";
            Debug.Log("PURGATORY (Player 1) WINS THE GAME!");
        }
        else
        {
            if (leftResultText != null) leftResultText.text = "CYBERPUNK WINS";
            if (rightResultText != null) rightResultText.text = "CYBERPUNK WINS";
            Debug.Log("CYBERPUNK (Player 2) WINS THE GAME!");
        }

        // Enable end bool — triggers the final win animation
        if (transitionAnimator != null)
        {
            transitionAnimator.SetBool("end", true);
        }

        // Wait for display time
        yield return new WaitForSeconds(finalWinDisplayTime);

        // Load title screen
        SceneManager.LoadScene(titleSceneName);
    }

    // =====================
    // LEVEL SELECTION
    // =====================

    void SelectRandomLevel()
    {
        // Refill the pool if exhausted
        if (availableLevels.Count == 0)
        {
            for (int i = 0; i < levelLayouts.Length; i++)
            {
                availableLevels.Add(i);
            }

            // Remove the current level so we don't repeat immediately
            if (currentLevelIndex >= 0)
            {
                availableLevels.Remove(currentLevelIndex);
            }
        }

        // Pick a random level from available pool
        int randomIndex = Random.Range(0, availableLevels.Count);
        int selectedLevel = availableLevels[randomIndex];
        availableLevels.RemoveAt(randomIndex);

        // Deactivate current level
        if (currentLevelIndex >= 0 && currentLevelIndex < levelLayouts.Length)
        {
            levelLayouts[currentLevelIndex].SetActive(false);
        }

        // Activate new level
        currentLevelIndex = selectedLevel;
        levelLayouts[currentLevelIndex].SetActive(true);

        Debug.Log($"Level selected: {levelLayouts[currentLevelIndex].name} (index {currentLevelIndex})");
    }

    // =====================
    // SPAWN
    // =====================

    void RespawnPlayers()
    {
        if (currentLevelIndex < 0 || currentLevelIndex >= levelSpawnPoints.Length)
        {
            Debug.LogError("No spawn points configured for current level index!");
            return;
        }

        LevelSpawnPoints spawns = levelSpawnPoints[currentLevelIndex];

        if (p1 != null && spawns.player1Spawn != null)
        {
            p1.SetPosition(spawns.player1Spawn.position);
        }

        if (p2 != null && spawns.player2Spawn != null)
        {
            p2.SetPosition(spawns.player2Spawn.position);
        }
    }

    /// <summary>
    /// Disables player GameObjects so they can't interact with anything during transitions.
    /// </summary>
    void HidePlayers()
    {
        if (p1 != null) p1.gameObject.SetActive(false);
        if (p2 != null) p2.gameObject.SetActive(false);
    }

    /// <summary>
    /// Re-enables player GameObjects after they've been repositioned.
    /// </summary>
    void ShowPlayers()
    {
        if (p1 != null) p1.gameObject.SetActive(true);
        if (p2 != null) p2.gameObject.SetActive(true);
    }

    // =====================
    // UI
    // =====================

    void UpdateScoreUI()
    {
        if (player1ScoreText != null) player1ScoreText.text = player1Score.ToString();
        if (player2ScoreText != null) player2ScoreText.text = player2Score.ToString();
    }
}

// =====================
// HELPER CLASS
// =====================

[System.Serializable]
public class LevelSpawnPoints
{
    public Transform player1Spawn;
    public Transform player2Spawn;
}
