using UnityEngine;

public class FollowCharacter : MonoBehaviour
{
    public Transform target; // The target to follow
    public Vector3 offset;   // The offset from the target

    void LateUpdate()
    {
        if (target != null)

        {
            // Calculate the desired position for the camera
            Vector3 desiredPosition = target.position + offset;

            // Ensure the camera maintains its original Z position
            desiredPosition.z = transform.position.z;

            // Move the camera towards the desired position smoothly
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 5f);
        }
    }
}
