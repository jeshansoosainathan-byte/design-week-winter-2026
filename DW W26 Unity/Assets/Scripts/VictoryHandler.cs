using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryHandler : MonoBehaviour
{
    /*attach to empty game manger in scene. Displays appropriate canvas depending on winning team then 
      restarts the game when players press start, can change to title screen when functional*/
    [Header("Victory Screens")]
    [SerializeField] GameObject PurgatoryWinScreen;
    [SerializeField] GameObject CyberpunkWinScreen;

    //Input
    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Respawn.performed += ctx => LoadFighterGame();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void Start()
    {
        // Show the win screen based on the stored winner
        switch (GameManager.winner)
        {
            case GameManager.Team.Purgatory:
                PurgatoryWinScreen.SetActive(true);
                CyberpunkWinScreen.SetActive(false);
                break;
            case GameManager.Team.Cyberpunk:
                CyberpunkWinScreen.SetActive(true);
                PurgatoryWinScreen.SetActive(false);
                break;
            default:
                Debug.LogWarning("No winner set! Defaulting to no screens.");
                break;
        }
    }

    private void LoadFighterGame()
    {
        //Reset winner for next game
        GameManager.winner = GameManager.Team.None; // Add Team.None to enum if needed
        SceneManager.LoadScene("FighterGame");
    }
}
