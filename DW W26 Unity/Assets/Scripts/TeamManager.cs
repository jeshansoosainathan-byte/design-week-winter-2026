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
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); } else
        {
         
        }

        


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

            Debug.Log("Join purg");
            AssignTeam(playerInput, Team.PURGATORY);

            Destroy(gameObject);




          
        }
        else if (gamepad.buttonEast.isPressed)
        {

            Debug.Log("Join cyber");
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

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("FighterScene");
    }


}