using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour, IDamageable
{

    

    [Header("Player Component References")]
    [SerializeField] Rigidbody2D rigidbody2D;
    [SerializeField] private Image healthbarSprite;
    

    public TeamSelectManager.Team CurrentTeam { get; private set; } = TeamSelectManager.Team.NONE;


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
 
  
    public void SetTeam(TeamSelectManager.Team team)
    {
        CurrentTeam = team;

        Debug.Log($"Player {GetComponent<PlayerInput>().playerIndex} joined {team}");

        if (team == TeamSelectManager.Team.PURGATORY)
        {

            transform.position = new Vector2(0, 0);


        } else
        {

            transform.position = new Vector2(30, 0);


        }



            var sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            sprite.color = team == TeamSelectManager.Team.PURGATORY ? Color.red : Color.blue;
        }
    }
    public void die()
    {

        //Death sound   
      
        Destroy(gameObject);

    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        SFXManager.instance.playSFX(hurtSound, transform, 1f);
        if (currentHealth <= 0) {
            currentHealth = 0;
            die();
        }


    }
}
