using System.Collections;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    public float killDamage = 999999f; // Damage dealt to player or enemy

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the Player
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(killDamage); // Use appropriate element type
                Debug.Log("Player has entered the kill zone and took damage!");
            }
        }
        // Check if the collider belongs to an enemy
        else if (other.CompareTag("Enemy"))
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
