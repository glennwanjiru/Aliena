using UnityEngine;
using UnityEngine.UI;

public class ControlMusic : MonoBehaviour
{
    public Button toggleButton; // Reference to the button that toggles the music

    private bool isSoundOn = true; // Flag to track the state of the sound

    void Start()
    {
        // Ensure the button has been assigned
        if (toggleButton != null)
        {
            // Add a listener to the button to call the ToggleSound method when clicked
            toggleButton.onClick.AddListener(ToggleSound);
        }
        else
        {
            Debug.LogError("ToggleButton is not assigned in the inspector.");
        }
    }

    // Method to toggle the sound on and off
    void ToggleSound()
    {
        // Toggle the flag for the sound state
        isSoundOn = !isSoundOn;

        // Get all GameObjects in the scene
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        // Iterate through each GameObject
        foreach (GameObject obj in allObjects)
        {
            // Check if the GameObject has an AudioSource component
            AudioSource audioSource = obj.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                // Enable or disable the AudioSource based on the sound state
                audioSource.enabled = isSoundOn;
            }
        }
    }
}
