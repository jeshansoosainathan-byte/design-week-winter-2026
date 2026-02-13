using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

 
public class TeamManager : MonoBehaviour
{
    public enum Team
    {
        NONE,
        PURGATORY,
        CYBERPUNK
    }

    public static TeamManager instance;


    private void Awake()
    {
        if (instance == null) { instance = this; }
    }


    public List<PlayerInput> teamA = new();
    public List<PlayerInput> teamB = new();

    public void HandlePlayerJoined(PlayerInput playerInput)
    {
     
        /*
        var joinAction = playerInput.actions["Join Purgatory"];
        var joinAction2 = playerInput.actions["Join Cyberpunk"];

        */


        var gamepad = playerInput.devices[0] as Gamepad;

        if ( gamepad.buttonWest.isPressed)
        {
           
            AssignTeam(playerInput, Team.PURGATORY);
          
        }
        else if (gamepad.buttonEast.isPressed)
        {
            //Debug.Log("Cyberpunk!");
            AssignTeam(playerInput, Team.CYBERPUNK);
        } else
        {

           

        }

    }

    void AssignTeam(PlayerInput player, Team team)
    {
        
        if (team == Team.PURGATORY)
            teamA.Add(player);
        else if (team == Team.CYBERPUNK)
            teamB.Add(player);

        var handler = player.GetComponent<PlayerController>();
        handler.SetTeam(team);

        
    }
}