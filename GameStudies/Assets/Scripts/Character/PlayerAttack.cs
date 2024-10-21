using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float punchRange = 2f;
    public LayerMask hitMask;
    public PlayerStats playerStats; // Reference to the PlayerStats ScriptableObject
    private Animator  animator;

        private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click to punch
        {
            animator.SetTrigger("Attack");
            Punch();
        }
    }

    private void Punch()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, punchRange, hitMask))
        {
            Debug.Log("Hit: " + hit.collider.name);
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
            
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage((int)playerStats.currentDamage); // Use currentDamage for attack
            }
        }
        Debug.DrawRay(transform.position, transform.forward * punchRange, Color.red, 1f);
    }
}
