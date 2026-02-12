using UnityEngine;
using UnityEngine.InputSystem;

public class BulletScript : MonoBehaviour
{


    public TeamSelectManager.Team CurrentTeam { get; private set; } = TeamSelectManager.Team.NONE;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

        if (damageable != null)
        {
            damageable.TakeDamage(1f);

        }

        Destroy(gameObject);
    }


    public void SetTeam(TeamSelectManager.Team team)
    {
        CurrentTeam = team;
 
    }
}
