using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Vector3 offset = new Vector3(0, 2, 0); // Adjusts the position above the enemy

    private EnemyHealth enemyHealth;
    private Transform enemyTransform;

    private void Start()
    {
        enemyHealth = GetComponentInParent<EnemyHealth>();
        enemyTransform = enemyHealth.transform;

        if (enemyHealth != null)
        {
            healthSlider.maxValue = enemyHealth.maxHealth;
            healthSlider.value = enemyHealth.maxHealth;
        }
        else
        {
            Debug.LogError("EnemyHealth component not found on parent object.");
        }
    }

    private void Update()
    {
        if (enemyHealth != null)
        {
            healthSlider.value = enemyHealth.currentHealth;
            // Keep the health bar above the enemy
            //healthSlider.transform.position = Camera.main.WorldToScreenPoint(enemyTransform.position + offset);
        }
    }
}
