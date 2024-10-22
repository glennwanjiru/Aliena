using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip musicClip; // The AudioClip to play
    private AudioSource audioSource; // Reference to the AudioSource component

    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();

        // Check if an AudioClip is assigned
        if (musicClip != null)
        {
            // Assign the AudioClip to the AudioSource
            audioSource.clip = musicClip;

            // Play the music
            audioSource.Play();
        }
        else
        {
            Debug.LogError("No AudioClip assigned to play.");
        }
    }
}
