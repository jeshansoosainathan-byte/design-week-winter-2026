using UnityEngine;

public class DestroyTerrain : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Check for collision with gameobject tagged as a projectile
        if (collision.gameObject.CompareTag("projectile"))
        {
            //Disable gameobject on collision
            gameObject.SetActive(false);

            Destroy(collision.gameObject); //Destroy the projectile on hit
        }
    }
}
