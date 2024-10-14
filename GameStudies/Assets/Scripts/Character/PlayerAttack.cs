using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float punchRange = 2f;
    public int punchDamage = 20;
    public LayerMask hitMask;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click to punch
        {
            Punch();
        }
    }

    private void Punch()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, punchRange, hitMask))
        {
            Debug.Log("Hit: " + hit.collider.name);
            // If the object has a health component, damage it
            PlayerHealth enemyHealth = hit.collider.GetComponent<PlayerHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(punchDamage);
            }
        }
        Debug.DrawRay(transform.position, transform.forward * punchRange, Color.red, 1f);
    }
}
