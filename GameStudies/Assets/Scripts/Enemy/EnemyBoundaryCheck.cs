using UnityEngine;

public class EnemyBoundaryCheck : MonoBehaviour
{
    private LevelCreator levelCreator; // Reference to the LevelCreator
    private EnemyHealth enemyHealth;

    public float outOfBoundsDamage = 9999f; // High damage to ensure instant death

    void Start()
    {
        levelCreator = FindObjectOfType<LevelCreator>(); // Get a reference to LevelCreator
        enemyHealth = GetComponent<EnemyHealth>();

        // Check if the enemy is outside of the map bounds
        if (IsOutOfBounds())
        {
            // If the enemy is outside, apply damage and kill it instantly
            enemyHealth.TakeDamage(outOfBoundsDamage, ElementType.Light); // Replace with an appropriate element type
        }
    }

    private bool IsOutOfBounds()
    {
        if (levelCreator == null)
        {
            Debug.LogWarning("LevelCreator not found! Make sure it is present in the scene.");
            return false; // No bounds check if LevelCreator isn't found
        }

        // Calculate the boundaries based on the LevelCreator's dungeon dimensions
        float boundaryMinX = -levelCreator.dungeonWidth / 2f; // Left boundary
        float boundaryMaxX = levelCreator.dungeonWidth / 2f;  // Right boundary
        float boundaryMinZ = -levelCreator.dungeonLength / 2f; // Bottom boundary
        float boundaryMaxZ = levelCreator.dungeonLength / 2f;  // Top boundary

        Vector3 position = transform.position;
        return (position.x < boundaryMinX || position.x > boundaryMaxX ||
                position.z < boundaryMinZ || position.z > boundaryMaxZ);
    }
}
