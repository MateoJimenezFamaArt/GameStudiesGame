using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    public LevelCreator levelCreator; // Reference to the LevelCreator script
    private int totalEnemyCount;       // Total count of enemies
    private int remainingEnemies;      // Count of remaining enemies
    private bool levelCleared;         // Flag to track if level has been cleared
    private PlayerHealth health; // Reference to health

    private TextMeshProUGUI enemiesLeftText; // UI text for enemies left display

    private void Start()
    {
        StartCoroutine(InitializeEnemyCountWithDelay(0f)); // Delay to initialize enemy count
        StartCoroutine(HoldUpForReference());

        // Find the EnemiesLeft TextMeshProUGUI component
        enemiesLeftText = GameObject.Find("EnemiesLeft").GetComponent<TextMeshProUGUI>();
    }

    // Coroutine to initialize enemy count after a delay
    private IEnumerator InitializeEnemyCountWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        InitializeEnemyCount();
        StartCoroutine(CheckRemainingEnemiesPeriodically(2.0f)); // Check every 2 seconds
    }

    private void InitializeEnemyCount()
    {
        totalEnemyCount = 0;

        // Count all enemies currently in the scene
        foreach (var enemy in FindObjectsOfType<EnemyHealth>())
        {
            // Subscribe to each enemy's OnDeath event
            enemy.OnDeath += OnEnemyDeath;
            totalEnemyCount++;
        }

        remainingEnemies = totalEnemyCount;
        UpdateEnemiesLeftText(); // Initial update for the UI text
        Debug.Log("Total enemies in the level: " + totalEnemyCount);
    }

    private void OnEnemyDeath()
    {
        remainingEnemies--;
        UpdateEnemiesLeftText(); // Update the UI text whenever an enemy dies
        Debug.Log("Enemy defeated! Remaining enemies: " + remainingEnemies);
    }

    // Coroutine to periodically check remaining enemies
    private IEnumerator CheckRemainingEnemiesPeriodically(float interval)
    {
        while (!levelCleared)
        {
            yield return new WaitForSeconds(interval);

            if (remainingEnemies <= 0 && totalEnemyCount > 0) // Check if all enemies are defeated
            {
                Debug.Log("All enemies defeated! Player cleared the level.");
                RunsManager.Instance.CompleteRun();
                levelCleared = true; // Set the level cleared flag
                StartCoroutine(ChangeSceneAfterDelay(5.0f)); // Wait 5 seconds and change scene
            }
            if (remainingEnemies == 1)
            {
                StartCoroutine(ChangeSceneAfterDelay(35.0f));
            }
        }
    }

    public int GetRemainingEnemyCount()
    {
        return remainingEnemies;
    }

    // Coroutine to change the scene after a 5-second delay
    private IEnumerator ChangeSceneAfterDelay(float delay)
    {
        if (remainingEnemies == 1)
        {
            enemiesLeftText.text = "The tree will harvest the last soul";
        }
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("KlondikeTest"); // Replace with the actual scene name
    }

    private IEnumerator HoldUpForReference()
    {
        yield return new WaitForSeconds(0.3f); // Wait for 0.3 seconds to find references
        health = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
    }

    // Updates the EnemiesLeft text in the UI
    private void UpdateEnemiesLeftText()
    {
        if (enemiesLeftText != null)
        {
            enemiesLeftText.text = $"Enemies Left: {remainingEnemies} / {totalEnemyCount}";
        }
    }
}
