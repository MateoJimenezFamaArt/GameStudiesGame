using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject attackHitbox; // Hitbox for the attack
    public float attackDamage = 10;

    private Collider attackCollider;


    private void Start()
    {
        // Get the Collider component and ensure it's set as a trigger
        attackCollider = attackHitbox.GetComponent<Collider>();
        if (attackCollider != null)
        {
            attackCollider.isTrigger = true;
        }
        else
        {
            Debug.LogError("No Collider found on the attack hitbox!");
        }

        if (RunsManager.Instance.runsCompleted > 0)
        {
            attackDamage += RunsManager.Instance.moreEdmg;
        } else {attackDamage = 10;}

    }

    // This method should be triggered by the animation event at the correct time
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by enemy attack!");
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }
    }
}
