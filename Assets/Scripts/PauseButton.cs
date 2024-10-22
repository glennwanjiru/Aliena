using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    // The GameObject to enable/disable when the game is paused
    public GameObject objectToToggle;


    // Reference to the button component
    private Button pauseButton;

    // Flag to track the paused state
    private bool isPaused = false;

    void Start()
    {
        // Get the Button component attached to this GameObject
        pauseButton = GetComponent<Button>();

        // Add the OnClick listener
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(TogglePause);
        }
        else
        {
            Debug.LogError("PauseButton script must be attached to a GameObject with a Button component.");
        }
    }

    void TogglePause()
    {
        // Toggle the paused state
        isPaused = !isPaused;

        // Pause or unpause the game
        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        // Enable or disable the specified GameObject
        if (objectToToggle != null)
        {
            objectToToggle.SetActive(isPaused);
        }
        else
        {
            Debug.LogError("No GameObject assigned to toggle.");
        }
    }
}
