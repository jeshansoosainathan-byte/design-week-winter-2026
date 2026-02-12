using UnityEngine;

public class DestroyTerrain : MonoBehaviour
{
    [Header("Hits Settings")]
    [SerializeField] private int maxHits = 1; //How many hits a platform can survive 2 for stone, 1 for glass

    private int currentHits = 0;

    void Start()
    {
        currentHits = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            TakeHit();
            Destroy(collision.gameObject); //Destroy projectile on hit
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Break glass on player exit
        if (CompareTag("Glass") && collision.gameObject.CompareTag("Player"))
        {
            TakeHit();
        }
    }

    public void TakeHit()
    {
        currentHits++;
        if (currentHits >= maxHits)
        {
            gameObject.SetActive(false);
        }
    }
}
