using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Destroy this game object when colliding with any other object
        Destroy(gameObject);
    }
}
