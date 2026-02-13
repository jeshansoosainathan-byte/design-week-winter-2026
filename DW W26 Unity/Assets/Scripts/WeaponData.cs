using UnityEngine;

public class WeaponData : MonoBehaviour
{


    [SerializeField] GameObject projectilePrefab;

    [Header("Stats")]
    [SerializeField] float attackSpeed;
    [SerializeField] float damage;
    [SerializeField] float flightSpeed;
    [SerializeField] float Gravity;



    [Header("Audio")]
    [SerializeField] AudioClip weaponSound;

   



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
