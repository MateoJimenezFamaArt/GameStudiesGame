using UnityEngine;

public class EnemyLookAtPlayer : MonoBehaviour
{
    public float rotationSpeed = 5f; // Speed at which the enemy rotates
    public float chaseRadius = 10f;  // Radius within which the enemy should look at the player

    private Transform player;
    private EnemyStateMachine enemyStateMachine;

    private void Start()
    {
        // Find the player at runtime using the Player tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player object not found! Ensure a GameObject with the 'Player' tag exists in the scene.");
        }

        // Get the EnemyStateMachine component at runtime
        enemyStateMachine = GetComponent<EnemyStateMachine>();
        if (enemyStateMachine == null)
        {
            Debug.LogError("EnemyStateMachine component not found on this GameObject!");
        }
    }

    private void Update()
    {
        // Check if player is within the chase radius
        if (player != null && IsPlayerInChaseRadius())
        {
            RotateTowardsPlayer();
        }
    }

    private bool IsPlayerInChaseRadius()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        return distanceToPlayer <= chaseRadius;
    }

    private void RotateTowardsPlayer()
    {
        // Calculate direction to the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0; // Keep the enemy upright (ignore vertical rotation)

        // Check if the direction is valid
        if (directionToPlayer != Vector3.zero)
        {
            // Calculate the target rotation
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            // Smoothly rotate towards the player
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
