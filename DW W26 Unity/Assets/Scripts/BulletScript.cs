using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class BulletScript : MonoBehaviour,ITeamMember
{

    [SerializeField] float damageToPlayers;
    [SerializeField] float damagetoTerrain;
    public GameObject owner;

    public TeamManager.Team CurrentTeam = TeamManager.Team.NONE;
    


    public void Initialize(GameObject ownerIn, TeamManager.Team team)
    {
        Debug.Log("Init!");
        owner = ownerIn;
        CurrentTeam = team;

        Debug.Log($"Owner: {owner} Team: {CurrentTeam}");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        ITeamMember team = collision.gameObject.GetComponent<ITeamMember>();
        GameObject obj = collision.gameObject;

        Debug.Log($"Owner: {owner} Team: {CurrentTeam}");
    
            if (collision.gameObject == owner || team.getTeam() == CurrentTeam)
            {


                Physics2D.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider2D>());

                return;



            }
        else
        {

            
            if (damageable != null)
            {
                Debug.Log("Damageable!");
                if (collision.gameObject.CompareTag("Player"))
                {

                    TeamManager.Team otherteam = team.getTeam();

                    Debug.Log($"Player Detected, Shoot! I am {CurrentTeam} and it is {otherteam}");

                    damageable.TakeDamage(damageToPlayers);
                }
                else if (collision.collider.CompareTag("Tile"))
                {
                    damageable.TakeDamage(damagetoTerrain);



                }







            }
            


        }

        Destroy(gameObject);
    }


    public void SetTeam(TeamManager.Team team)
    {
        CurrentTeam = team;
 
    }

    public TeamManager.Team getTeam()
    {
        return CurrentTeam;
    }
}
