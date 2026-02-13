using UnityEngine;

public class Tile : MonoBehaviour, IDamageable
{

    [SerializeField] float maxHealth;
    float currentHealth;

    [SerializeField] Sprite fullHealthSprite;
    [SerializeField] Sprite lowHealthSprite;
    private SpriteRenderer spriteRenderer;

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);

        }

        if (currentHealth == maxHealth)
        {
            spriteRenderer.sprite = fullHealthSprite;
        }
        else
        {

            spriteRenderer.sprite = lowHealthSprite;


        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {


      

        



    }


}
