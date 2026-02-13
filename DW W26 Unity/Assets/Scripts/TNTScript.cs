using Unity.VisualScripting;
using UnityEngine;
using static TeamManager;

public class TNTScript : MonoBehaviour
{

    bool touched = false;


    [SerializeField] GameObject grenade;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {

       
    }

    public void boom()
    {
        Debug.Log("Boom");
        GameObject bomb  = Instantiate(grenade, transform.position, transform.rotation);

        Destroy(gameObject);



    }


    public void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Feet"))
        {
            Debug.Log("Feet Trigger!");

            boom();

        }




    }



   





}



