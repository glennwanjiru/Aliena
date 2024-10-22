using UnityEngine;

public class GroundEnemies : MonoBehaviour
{
    public Transform player;        // Reference to the player object
    public float detectionDistance; // Distance at which the enemy detects the player
    public float patrolRange;       // Range within which the enemy patrols
    public float patrolSpeed;       // Speed of patrolling
    public float moveSpeed;         // Speed of approaching the player
    public float rotationSpeed;     // Speed of rotation when patrolling

    private Vector2 leftPoint;      // Left point of patrol range
    private Vector2 rightPoint;     // Right point of patrol range
    private bool movingRight = true; // Flag for patrol direction

    void Start()
    {
        // Set initial patrol points
        leftPoint = transform.position - Vector3.right * patrolRange / 2;
        rightPoint = transform.position + Vector3.right * patrolRange / 2;

    }

    void Update()
    {
        // Check if player is within detection distance
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionDistance)
        {
            // Move towards the player
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        }
        else
        {
            // Patrol left and right
            if (movingRight)
            {
                transform.position = Vector2.MoveTowards(transform.position, rightPoint, patrolSpeed * Time.deltaTime);
                if (transform.position.x >= rightPoint.x)
                {
                    movingRight = false;
                }
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, leftPoint, patrolSpeed * Time.deltaTime);
                if (transform.position.x <= leftPoint.x)
                {
                    movingRight = true;
                }
            }
        }

        // Rotate enemy towards patrol direction
        Vector2 targetDirection = (movingRight) ? Vector2.right : -Vector2.right;
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, targetDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
