using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneSwitcher : MonoBehaviour
{
    public Button switchSceneButton; // Reference to the Button component
    public string sceneToLoad;       // Name of the scene to load

    void Start()
    {
        // Ensure the button has been assigned
        if (switchSceneButton != null)
        {
            // Add a listener to the button to call the LoadScene method when clicked
            switchSceneButton.onClick.AddListener(LoadScene);
        }
        else
        {
            Debug.LogError("SwitchSceneButton is not assigned in the inspector.");
        }
    }

    // Method to load the specified scene
    void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            StartCoroutine(LoadSceneAsync(sceneToLoad));
        }
        else
        {
            Debug.LogError("Scene to load is not specified.");
        }
    }

    // Coroutine for loading the scene asynchronously
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        if (asyncLoad == null)
        {
            Debug.LogError($"Failed to load scene: {sceneName}");
            yield break;
        }

        // Optional: Display a loading screen here

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Optional: Hide the loading screen here
    }
}
