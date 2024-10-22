using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    public string ignoredTag = "Player"; // Tag to ignore collisions with
    public GameObject explosionPrefab; // Prefab of the explosion
    public float explosionDuration = 1.0f; // Duration before destroying the explosion

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(ignoredTag))
        {
            // Ignore collision with objects having the specified tag
            return;
        }

        // Instantiate the explosion at the same position as the bullet
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Debug.Log("Explosion instantiated");

        // Destroy the explosion object after the specified duration
        Destroy(explosion, explosionDuration);
        Debug.Log("Explosion destroyed after " + explosionDuration + " seconds");

        // Destroy the bullet object
        Destroy(gameObject);
        Debug.Log("Bullet destroyed");
    }
}
