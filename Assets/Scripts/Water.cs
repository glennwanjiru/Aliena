using UnityEngine;
using UnityEngine.SceneManagement;

public class Water : MonoBehaviour
{
    public string sceneToLoad;          // Name of the scene to load
    public string targetTag;            // Tag of the game object you want to trigger scene switch

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the desired object
        if (collision.gameObject.CompareTag(targetTag))
        {
            // Load the specified scene
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
