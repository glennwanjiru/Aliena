using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    public Button quitButton; // Reference to the Button component

    void Start()
    {
        // Ensure the button has been assigned
        if (quitButton != null)
        {
            // Add a listener to the button to call the Quit method when clicked
            quitButton.onClick.AddListener(Quit);
        }
        else
        {
            Debug.LogError("QuitButton is not assigned in the inspector.");
        }
    }

    // Method to quit the game
    void Quit()
    {
#if UNITY_EDITOR
            // Application.Quit() does not work in the editor, so if we are running in the editor, stop playing the scene
            UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quit the application
        Application.Quit();
#endif
    }
}
