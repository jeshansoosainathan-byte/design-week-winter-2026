using Unity.VisualScripting;
using UnityEngine;
using static TeamManager;

public class GlassScript : MonoBehaviour
{

    bool touched = false;

    [SerializeField] Sprite brokenSprite;
    private SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (touched) { spriteRenderer.sprite = brokenSprite; }
    
       
    }

    public void crackGlass()
    {
        GetComponent<SpriteRenderer>().sprite = brokenSprite;

        touched = true;


    }


    public void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Feet"))
        {
            Debug.Log("Feet Trigger!");

            crackGlass();

        }




    }



    public void OnCollisionExit2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;

        if (obj.CompareTag("Player") && touched)
        {


            Destroy(gameObject);
        }

    }





}



