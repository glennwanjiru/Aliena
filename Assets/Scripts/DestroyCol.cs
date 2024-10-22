using UnityEngine;

public class DestroyCol : MonoBehaviour
{
    public string bulletTag = "Bullet"; // Tag of the bullet object
    public string enemyTag = "Enemy";   // Tag of the enemy object

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the bullet
        if (collision.gameObject.CompareTag(bulletTag))
        {
            // Check if the object hit is an enemy
            if (gameObject.CompareTag(enemyTag))
            {
                // Destroy the enemy object
                Destroy(gameObject);
            }
        }
    }
}
