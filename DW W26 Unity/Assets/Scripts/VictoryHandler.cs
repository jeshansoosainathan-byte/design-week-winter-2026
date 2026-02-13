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
        controls.Player.Respawn.performed += ctx => LoadNewGame();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void LoadNewGame()
    {
        SceneManager.LoadScene("FighterGame");
    }
}
