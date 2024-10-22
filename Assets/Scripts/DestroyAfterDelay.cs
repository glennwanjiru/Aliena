using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    public float delay = 1.0f; // Time delay before destruction

    void Start()
    {
        // Call the DestroyObject method after the specified delay
        Invoke("DestroyObject", delay);
    }

    void DestroyObject()
    {
        // Destroy this game object after the delay
        Destroy(gameObject);
    }
}

