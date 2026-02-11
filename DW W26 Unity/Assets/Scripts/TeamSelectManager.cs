using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

 
public class TeamSelectManager : MonoBehaviour
{
    public enum Team
    {
        NONE,
        PURGATORY,
        CYBERPUNK
    }

    public static TeamSelectManager instance;


    private void Awake()
    {
        if (instance == null) { instance = this; }
    }


    public List<PlayerInput> teamA = new();
    public List<PlayerInput> teamB = new();

    public void HandlePlayerJoined(PlayerInput playerInput)
    {





        var joinAction = playerInput.actions["Join Purgatory"];
        var joinAction2 = playerInput.actions["Join Cyberpunk"];
        if ( joinAction.IsPressed())
        {
            AssignTeam(playerInput, Team.PURGATORY);
            return;
        }
        else if (joinAction2.IsPressed())
        {

            AssignTeam(playerInput, Team.CYBERPUNK);
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

        Debug.Log($"Player {player.playerIndex} joined {team}");
    }
}