using System.Collections.Generic;
using UnityEngine;

public class EnemySpaceship : MonoBehaviour
{
    // States for enemy behavior
    public enum State
    {
        Patrol,
        Chase,
        Attack
    }

    private State currentState;

    // Waypoints for patrolling
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    // Target to chase
    public Transform target;

    // Behavior tree variables
    private BehaviorTreeNode behaviorTreeRoot;

    // A* path finding variables
    private List<Vector3> path = new List<Vector3>();
    private int currentPathIndex = 0;

    void Start()
    {
        // Initialize to patrol state
        currentState = State.Patrol;

        // Define the behavior tree
        var chaseAction = new ActionNode(Chase);
        var attackAction = new ActionNode(Attack);
        var sequence = new SequenceNode(chaseAction, attackAction);
        behaviorTreeRoot = sequence;
    }

    void Update()
    {
        // Move to patrol state if not already patrolling
        if (currentState != State.Patrol)
        {
            currentState = State.Patrol;
        }

        // Execute behavior tree
        behaviorTreeRoot.Execute();

        // Follow the path if a path exists
        if (path.Count > 0)
        {
            FollowPath();
        }
    }

    void Patrol()
    {
        // Implement patrol behavior here
        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints assigned.");
            return;
        }

        // Move towards the current waypoint
        Vector3 direction = (waypoints[currentWaypointIndex].position - transform.position).normalized;
        transform.Translate(direction * Time.deltaTime * patrolSpeed, Space.World);

        // Check if the enemy has reached the current waypoint
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    void Chase()
    {
        // Implement chase behavior here
        if (target != null && Vector3.Distance(transform.position, target.position) < chaseRange)
        {
            // Move towards the target
            Vector3 direction = (target.position - transform.position).normalized;
            transform.Translate(direction * Time.deltaTime * chaseSpeed, Space.World);

            // Calculate a new path to the target
            CalculatePath(target.position);
        }
        else
        {
            // Switch back to patrol if target is out of range
            currentState = State.Patrol;

        }
    }

    void Attack()
    {
        // Implement attack behavior here
        if (target != null && Vector3.Distance(transform.position, target.position) < attackRange && Time.time > nextFireTime)
        {
            // Fire projectile at the target
            if (projectilePrefab != null && firePoint != null)
            {
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
                Vector3 direction = (target.position - firePoint.position).normalized;
                projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed; // Adjust speed as needed
            }

            // Set next fire time
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    // Detect when the player enters the trigger zone
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.transform;
            currentState = State.Chase;
        }
    }

    // Detect when the player exits the trigger zone
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;
            currentState = State.Patrol;
        }
    }

    // Define the behavior tree classes
    public abstract class BehaviorTreeNode
    {
        public abstract bool Execute();
    }

    public class SequenceNode : BehaviorTreeNode
    {
        private readonly BehaviorTreeNode[] children;

        public SequenceNode(params BehaviorTreeNode[] nodes)
        {
            children = nodes;
        }

        public override bool Execute()
        {
            foreach (var child in children)
            {
                if (!child.Execute())
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class ActionNode : BehaviorTreeNode
    {
        private readonly System.Action action;

        public ActionNode(System.Action action)
        {
            this.action = action;
        }

        public override bool Execute()
        {
            action?.Invoke();
            return true;
        }
    }

    void CalculatePath(Vector3 targetPosition)
    {

        // The path should be a list of Vector3 positions from the current position to the target position

        path = new List<Vector3>
        {
            transform.position,
            new Vector3(0, 0, 0), // Placeholder waypoint
            new Vector3(1, 1, 0), // Placeholder waypoint
            targetPosition
        };

        currentPathIndex = 0;
    }

    void FollowPath()
    {
        // Move towards the current waypoint in the path
        Vector3 direction = (path[currentPathIndex] - transform.position).normalized;
        transform.Translate(direction * Time.deltaTime * chaseSpeed, Space.World);

        // Check if the enemy has reached the current waypoint
        if (Vector3.Distance(transform.position, path[currentPathIndex]) < 0.5f)
        {
            currentPathIndex++;
        }
    }

    // Parameters for movement and attack
    public float patrolSpeed = 3f;
    public float chaseSpeed = 5f;
    public float chaseRange = 10f;
    public float attackRange = 10f;
    public float fireRate = 1f;
    public float nextFireTime = 0f;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;
}