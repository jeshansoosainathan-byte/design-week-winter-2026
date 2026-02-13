using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;
<<<<<<< HEAD:DW W26 Unity/Assets/Scripts/Player Scripts/PlayerController.cs
public class PlayerController : MonoBehaviour, IDamageable, ITeamMember
=======
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour, IDamageable
>>>>>>> 49e718f01333344692324fea0b8160d0faaf6e19:DW W26 Unity/Assets/Scripts/PlayerController.cs
{

    

    [Header("Player Component References")]
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private Image healthbarSprite;
    

    public TeamManager.Team CurrentTeam { get; private set; } = TeamManager.Team.NONE;


    [Header("Player Stats")]


    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;
    [SerializeField] float speed;
    [SerializeField] float jumpHeight;
    [SerializeField] float lowJumpMultiplier = 1.5f;
    [SerializeField] float fallMultiplier = 2.5f;


    [Header("Grounding")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;

    [SerializeField] float coyoteTime;
    [SerializeField] float coyoteTimeMax = 0.2f;

    [Header("Audio")]
    [SerializeField] AudioClip spawnSound; // <-- drag your sound here in the inspector
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioSource source;






    private float horizontal;
    private float up;

    InputAction.CallbackContext jumpContext;


    public void updateHealthBar()
    {

        healthbarSprite.fillAmount = currentHealth / maxHealth;

    }

 

    

    public void Start()
    {

        source = GetComponent<AudioSource>();
        source.clip= spawnSound; 
        source.Play();


        currentHealth = maxHealth;

        updateHealthBar();



    }


    public void Move(InputAction.CallbackContext context)
    {

        horizontal = context.ReadValue<Vector2>().x;

    }

    public void jump(InputAction.CallbackContext context)
    {


     

        jumpContext = context;
        if (context.performed && coyoteTime >0)
        { 

            
            rigidbody2D.linearVelocity = new Vector2(rigidbody2D.linearVelocityX, jumpHeight);

        }

        if (context.canceled)
        {
            
        }


    }




    private void Update()
    {


        if (transform.position.y < -30)
        {

            TakeDamage(1f);

        }

        updateHealthBar();
        if (isGrounded())
        {
            coyoteTime = coyoteTimeMax;

        }
        else
        {
            coyoteTime -= Time.deltaTime;
        }

rigidbody2D.linearVelocity = new Vector2(horizontal * speed, rigidbody2D.linearVelocity.y);

        if (rigidbody2D.linearVelocity.y < 0)    //If the player is falling
        {
            //Add to the velocity.
            //Vector2.up (0, 1) * gravity (default is -9.8) * fallMultiplier scalar
            //and then scaled by deltaTime to fix potential frame jitter
            rigidbody2D.linearVelocity += (Vector2.up * Physics2D.gravity.y *
                (fallMultiplier * Time.deltaTime));
        }

        else if (rigidbody2D.linearVelocity.y > 0 &&  !jumpContext.performed)
        {
            //If they are in the air and not pressing the key, apply the
            //low jump multiplier.
            rigidbody2D.linearVelocity += (Vector2.up * Physics2D.gravity.y *
                (lowJumpMultiplier * Time.deltaTime));
        }

    }

    private bool isGrounded()
    {

        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1f, 0.1f), CapsuleDirection2D.Horizontal, 0, groundLayer);

    }
 
  
    public void SetTeam(TeamManager.Team team)
    {
        CurrentTeam = team;

        
        Debug.Log($"Player {GetComponent<PlayerInput>().playerIndex} joined {team}");

        if (team == TeamManager.Team.PURGATORY)
        {

           


        } else
        {

          
        }



            var sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            sprite.color = team == TeamManager.Team.PURGATORY ? Color.red : Color.blue;
        }
    }
    public void die()
    {
        //Determine winner
        GameManager.Team winnerTeam = (CurrentTeam == TeamSelectManager.Team.PURGATORY)
        ? GameManager.Team.Cyberpunk
        : GameManager.Team.Purgatory;

        GameManager.winner = winnerTeam;
        //Death sound   
        SceneManager.LoadScene("Victory");
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        SFXManager.instance.playSFX(hurtSound, transform, 1f);
        Debug.Log($"Current Health: {currentHealth}");
        if (currentHealth <= 0) {
            currentHealth = 0;
            die();
        }


    }

    public TeamManager.Team getTeam()
    {
        return CurrentTeam;
    }
}
