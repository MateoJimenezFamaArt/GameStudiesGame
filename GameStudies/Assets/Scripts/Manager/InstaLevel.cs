using UnityEngine;

public class InstaLevel : MonoBehaviour
{
    public float killDamage = 999999f; // Damage dealt to player or enemy

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            EnemyAudioManager audioManager = other.GetComponent<EnemyAudioManager>();

            if (enemyHealth != null)
            {
                // Disable audio source before killing the enemy
                if (audioManager != null)
                {
                    audioManager.GetComponent<AudioSource>().enabled = false;
                }

                enemyHealth.TakeDamage(killDamage, ElementType.Light); // Use appropriate element type
                Debug.Log("An enemy has entered the kill zone and took damage!");
            }
        }
    }
}
