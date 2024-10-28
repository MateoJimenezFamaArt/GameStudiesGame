using UnityEngine;

public class PlayerStatsInitializer : MonoBehaviour
{
    private void Start()
    {
        // Only reset stats if GameManager hasn't initialized them yet
        if (RunsManager.Instance.runsCompleted == 0)
        {
            RunsManager.Instance.playerStats.ResetStats();
        }
    }
}
