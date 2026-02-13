using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static GameManager instance;

    /*
    int purgapoints = 0;
    int cyberpoints = 0;
    */

    public static Team winner = Team.None;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
       
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
    }
    public void OnPlayerJoined(PlayerInput player)
    {
     //   Debug.Log($"Player {player.playerIndex} joined using {player.devices[0]}");

        TeamManager.instance.HandlePlayerJoined(player);




    }
    void Update()
    {
        
    }
<<<<<<< HEAD
  
=======
    public enum Team
    {
        Purgatory,
        Cyberpunk,
        None
    }
    public class PlayerData
    {
        public Team team;
        public int playerIndex;
        public GameObject characterPrefab;
    }
>>>>>>> 49e718f01333344692324fea0b8160d0faaf6e19

}
