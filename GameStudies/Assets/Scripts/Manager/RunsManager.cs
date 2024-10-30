using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RunsManager : MonoBehaviour
{
    public static RunsManager Instance { get; private set; }

    public PlayerStats playerStats; // Reference to the PlayerStats ScriptableObject

    private PlayerHealth health; //reference to health

    public float PreservedHealth;
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
        StartCoroutine(HoldUpForReference());
    }

    public void CompleteRun()
    {
        runsCompleted++;
        PreservedHealth = health.currentHealth;
        UpdateEnemyDifficulty();
        Debug.Log("El valor de la vida es "+ health.currentHealth);
        Debug.Log("El valor guardado es :" + PreservedHealth);      
        // You can call other methods here to modify player stats or update UI
    }

    private void UpdateEnemyDifficulty()
    {
        Debug.Log("Pa le subimos a los enemigos");
    }

    private IEnumerator HoldUpForReference()
    {
        yield return new WaitForSeconds(0.3f); // Wait for 5 seconds (adjust as needed)
        health = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
    }
}
