using UnityEngine;

public class ToggleUI : MonoBehaviour
{

    private Canvas Stats;
    private bool isTimeFrozen = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Stats = GameObject.Find("Stats").GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleCanvas();
            ToggleTimeFreeze();
        }

        
    }

    public void ToggleCanvas()
    {
        if (Stats != null)
        {
            Stats.enabled = !Stats.enabled;
        }
        else
        {
            Debug.LogWarning("Canvas reference is missing.");
        }
    }

    public void ToggleTimeFreeze()
    {
        if (isTimeFrozen)
        {
            Time.timeScale = 1f;  // Unfreeze time
            isTimeFrozen = false;
        }
        else
        {
            Time.timeScale = 0f;  // Freeze time
            isTimeFrozen = true;
        }
    }
}
