using UnityEngine;
using UnityEngine.InputSystem;
/*To use this script just attach it to the root of the player prefab, then create a new empty transform as a child of the player prefab called Fire Point and position it at the tip of the weapon sprite
 From there just assign prefabs for projectiles and drag the transforms for the player arm pivot and fire point into the appropriate spots*/
public class FireProjectile : MonoBehaviour
{
    [Header("Pistol")]
    [SerializeField] private GameObject primaryPrefab;
    [SerializeField] private float primarySpeed = 20f;
    [SerializeField] private float primaryFireRate = 5f;

    [Header("Rifle")]
    [SerializeField] private GameObject secondaryPrefab;
    [SerializeField] private float secondarySpeed = 15f;
    [SerializeField] private float secondaryFireRate = 3f;

    [Header("Grenade")]
    [SerializeField] private GameObject tertiaryPrefab;
    [SerializeField] private float tertiarySpeed = 60f;
    [SerializeField] private float tertiaryFireRate = 1f;

    [SerializeField] private WeaponData[] weapons;








    [Header("References")]
    [SerializeField] private Transform armPivot;
    [SerializeField] private Transform firePoint;

    [Header("Aiming")]
  //  [SerializeField] private float aimDeadzone = 0.05f;

    [Header("Owner")]
    [SerializeField] GameObject owner;


    //Input & State
   
    private Vector2 aimDirection;
    private bool attackPressed;
    private float nextFireTime;
    private int currentWeaponIndex = 0; //0 = primary, 1 = secondary
    private int maxWeaponIndex = 0;


    private Vector2 lookInput;


    [Header("Audio")]
    [SerializeField] AudioClip pistolSound; // <-- drag your sound here in the inspector
    [SerializeField] AudioClip rifleSound;
    [SerializeField] AudioClip grenadeSound;

    private void Awake()
    {

      








    }

    public void cycleWeapons(InputAction.CallbackContext context)
    {

        /*
        if (context.canceled)
        {
            SwapWeapon();

        }
        */

    }

    public void fire(InputAction.CallbackContext context)
    {

        attackPressed = context.performed;

    }
    public void look(InputAction.CallbackContext context)
    {

        lookInput=context.ReadValue<Vector2>();

    }
    /*
    public void cycleLeft(InputAction.CallbackContext context)


    {
         
        if (currentWeaponIndex == 0) currentWeaponIndex = 2;

        else { currentWeaponIndex--; };

        
    }

    public void cycleRight(InputAction.CallbackContext context)
    {
        if (currentWeaponIndex == 2) currentWeaponIndex = 0;

        else
        {
            currentWeaponIndex++;
        }

    }
    */



    private void OnEnable()
    {
      
    }

    private void OnDisable()
    {
       
    }

    private void Update()
    {

     

        //Fire on attack press
        if (attackPressed && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / GetCurrentFireRate();
        }
    }

  

    private void SwapWeapon()
    {


        currentWeaponIndex++;

        if (currentWeaponIndex > 2)
        {
            currentWeaponIndex = 0;

        }

        Debug.Log($"Weapon Log {currentWeaponIndex}");

    }

    private GameObject GetCurrentPrefab()
    {
        switch (currentWeaponIndex)
        {
            case 0:
                return primaryPrefab;
            case 1:
                return secondaryPrefab;
                case 2:
                return tertiaryPrefab;
                default: return null;
        }
    }

    private float GetCurrentSpeed()
    {
        switch (currentWeaponIndex)
        {
            case 0:
                return primarySpeed;
            case 1:
                return secondarySpeed;
            case 2:
                return tertiarySpeed;
            default: return -1;
        }
    }




    private float GetCurrentFireRate()
    {
        switch (currentWeaponIndex)
        {
            case 0:
                return primaryFireRate;
            case 1:
                return secondaryFireRate;
            case 2:
                return tertiaryFireRate;
            default: return -1;
        }
    }

    private void Shoot()
    {

       
        GameObject currentPrefab = GetCurrentPrefab();
        if (currentPrefab == null || firePoint == null) return;

        GameObject proj = Instantiate(currentPrefab, firePoint.position, firePoint.rotation);

        BulletScript script = proj.GetComponent<BulletScript>();

       ITeamMember teammember = owner.GetComponent<ITeamMember>();

        if (currentWeaponIndex == 0)
        {
            SFXManager.instance.playSFX(pistolSound, transform, 1f);

        }
        else if (currentWeaponIndex == 1)
        {
            SFXManager.instance.playSFX(rifleSound, transform, 1f);
        }
        else
        {

         //   SFXManager.instance.playSFX(grenadeSound, transform, 1f);


        }

        
            script.Initialize(owner, teammember.getTeam(), currentWeaponIndex==3);
        

        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.right * GetCurrentSpeed();
        }




        Destroy(proj, 5f);
    }
}