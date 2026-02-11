using UnityEngine;
using UnityEngine.InputSystem;
public class GameMaster : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

 

    void Start()
    {
       
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
    }
    public void OnPlayerJoined(PlayerInput player)
    {
        Debug.Log($"Player {player.playerIndex} joined using {player.devices[0]}");
     
        


 
    }
    void Update()
    {
        
    }
}
