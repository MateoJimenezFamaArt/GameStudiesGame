using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    [Tooltip("Assign a GameObject to toggle when calling ToggleGameObject.")]
    public GameObject objectToToggle;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None; // Unlocks the cursor
        Cursor.visible = true; // Makes the cursor visible
    }

    // Method to quit the game
    public void QuitGame()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();
    }

    // Method to toggle the specified GameObject
    public void ToggleGameObject()
    {
        if (objectToToggle != null)
        {
            bool newState = !objectToToggle.activeSelf;
            objectToToggle.SetActive(newState);
            Debug.Log($"{objectToToggle.name} has been toggled to {(newState ? "enabled" : "disabled")}.");
        }
        else
        {
            Debug.LogWarning("No GameObject assigned to toggle.");
        }
    }

    // Method to load a different scene by name
    public void GoToScene(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
            Debug.Log($"Loading scene: {sceneName}");
        }
        else
        {
            Debug.LogError($"Scene '{sceneName}' cannot be loaded. Make sure it exists in the build settings.");
        }
    }
}
