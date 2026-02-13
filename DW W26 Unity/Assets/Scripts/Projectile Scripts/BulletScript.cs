using System;

using UnityEngine;
using UnityEngine.InputSystem;

public class BulletScript : MonoBehaviour,ITeamMember
{

    [SerializeField] float damageToPlayers;
    [SerializeField] float damageToTerrain;
    public GameObject owner;

    public TeamManager.Team CurrentTeam = TeamManager.Team.NONE;
    bool unique = false;



    public void Initialize(GameObject ownerIn, TeamManager.Team team, bool uniqueIn)
    {
       
        owner = ownerIn;
        CurrentTeam = team;
        unique = uniqueIn;


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
        if (unique) { return; }
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        ITeamMember team = collision.gameObject.GetComponent<ITeamMember>();

        GameObject obj = collision.gameObject;
        
    
            if (obj == owner || (team !=null && team.getTeam() == CurrentTeam))
            {


                Physics2D.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider2D>());

                return;



            }
        else
        {
         

            if (damageable != null)
            {
            
                if (collision.gameObject.CompareTag("Player"))
                {

                    TeamManager.Team otherteam = team.getTeam();

               

                    damageable.TakeDamage(damageToPlayers);
                }


                else if (collision.collider.CompareTag("Tile"))
                {
                    damageable.TakeDamage(damageToTerrain);



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
