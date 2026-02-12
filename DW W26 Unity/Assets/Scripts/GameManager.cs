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
  

}
