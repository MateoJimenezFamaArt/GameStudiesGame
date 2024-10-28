using UnityEngine;

public class RunsManager : MonoBehaviour
{
    public static RunsManager Instance { get; private set; }

    public PlayerStats playerStats; // Reference to the PlayerStats ScriptableObject
    private PlayerStatsManager playerStatsManager;

    public int runsCompleted = 0; // Number of runs completed

    private void Awake()
    {
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist through scene loads
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
            return;
        }
        // Initialize player stats
        playerStats.ResetStats();
    }

   /* private void Start()
    {
            // Find PlayerStatsManager
        playerStatsManager = FindObjectOfType<PlayerStatsManager>();
        if (playerStatsManager == null)
        {
            Debug.LogError("PlayerStatsManager not found in the scene!");
        }
    }*/

    public void CompleteRun()
    {
        runsCompleted++;
        UpdateEnemyDifficulty();
        // You can call other methods here to modify player stats or update UI
    }

    private void UpdateEnemyDifficulty()
    {
        Debug.Log("Pa le subimos a los enemigos");
    }
}
