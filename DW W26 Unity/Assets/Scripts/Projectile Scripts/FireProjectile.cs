using UnityEngine;
using UnityEngine.InputSystem;
/*To use this script just attach it to the root of the player prefab, then create a new empty transform as a child of the player prefab called Fire Point and position it at the tip of the weapon sprite
 From there just assign prefabs for projectiles and drag the transforms for the player arm pivot and fire point into the appropriate spots*/
public class FireProjectile : MonoBehaviour
{
    [Header("Primary Projectile")]
    [SerializeField] private GameObject primaryPrefab;
    [SerializeField] private float primarySpeed = 20f;
    [SerializeField] private float primaryFireRate = 5f;

    [Header("Secondary Projectile")]
    [SerializeField] private GameObject secondaryPrefab;
    [SerializeField] private float secondarySpeed = 15f;
    [SerializeField] private float secondaryFireRate = 3f;

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

    private Vector2 lookInput;

    private void Awake()
    {
        
    }


    public void fire(InputAction.CallbackContext context)
    {

        attackPressed = context.performed;

    }
    public void look(InputAction.CallbackContext context)
    {

        lookInput=context.ReadValue<Vector2>();

    }
    



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

    private void OnAttack(bool pressed)
    {
        attackPressed = pressed;
    }

    private void SwapWeapon()
    {
        currentWeaponIndex = 1 - currentWeaponIndex;
    }

    private GameObject GetCurrentPrefab()
    {
        return currentWeaponIndex == 0 ? primaryPrefab : secondaryPrefab;
    }

    private float GetCurrentSpeed()
    {
        return currentWeaponIndex == 0 ? primarySpeed : secondarySpeed;
    }

    private float GetCurrentFireRate()
    {
        return currentWeaponIndex == 0 ? primaryFireRate : secondaryFireRate;
    }

    private void Shoot()
    {

       
        GameObject currentPrefab = GetCurrentPrefab();
        if (currentPrefab == null || firePoint == null) return;

        GameObject proj = Instantiate(currentPrefab, firePoint.position, firePoint.rotation);

        BulletScript script = proj.GetComponent<BulletScript>();

       ITeamMember teammember = owner.GetComponent<ITeamMember>();



        script.Initialize(owner, teammember.getTeam());

        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.right * GetCurrentSpeed();
        }




        Destroy(proj, 5f);
    }
}