using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public float punchRange = 2f;
    public float lightAttackCooldown = 1f;
    public float heavyAttackCooldown = 2f;
    public LayerMask hitMask;
    public PlayerStats playerStats; // Reference to the PlayerStats ScriptableObject

    public GameObject rightHand; // Right hand object
    public Material[] elementMaterials; // Array of materials for elements

    private Animator animator;
    private bool canLightAttack = true;
    private bool canHeavyAttack = true;
    private AttackType currentAttackType;
    private ElementType currentElementType;

    // Enum for attack types
    private enum AttackType { Light, Heavy }

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentElementType = ElementType.Neutral; // Default element
        UpdateElementMaterial();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canLightAttack) // Left-click for light attack
        {
            StartAttack(AttackType.Light);
        }
        else if (Input.GetMouseButtonDown(1) && canHeavyAttack) // Right-click for heavy attack
        {
            StartAttack(AttackType.Heavy);
        }
        if (Input.GetKeyDown(KeyCode.E)) { CycleElement(); }
    }

    private void StartAttack(AttackType attackType)
    {
        currentAttackType = attackType;

        // Trigger appropriate animation
        if (attackType == AttackType.Light)
        {
            animator.SetTrigger("LightAttack");
            StartCoroutine(AttackCooldown(AttackType.Light));
            
        }
        else if (attackType == AttackType.Heavy)
        {
            animator.SetTrigger("HeavyAttack");
            StartCoroutine(AttackCooldown(AttackType.Heavy));
            
        }
    }

    private IEnumerator AttackCooldown(AttackType attackType)
    {
        if (attackType == AttackType.Light)
        {
            canLightAttack = false;
            yield return new WaitForSeconds(lightAttackCooldown);
            
            canLightAttack = true;
        }
        else if (attackType == AttackType.Heavy)
        {
            canHeavyAttack = false;
            yield return new WaitForSeconds(heavyAttackCooldown);
            canHeavyAttack = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is an enemy
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Le pegue");
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                // Apply damage with elemental effect
                float damage = currentAttackType == AttackType.Light ? playerStats.currentDamage : playerStats.currentDamage * 1.5f;
                enemyHealth.TakeDamage(damage, currentElementType);
                Debug.Log("Le zampe un traque al enemigo y le hize " + damage);
                Debug.Log("Y ese traque fue de " + currentElementType);

                // Apply knockback based on attack type
                Rigidbody enemyRb = other.gameObject.GetComponent<Rigidbody>();
                if (enemyRb != null)
                {
                    Vector3 knockbackDir = other.transform.position - transform.position;
                    knockbackDir.y = 0; // Ensure horizontal knockback

                    float knockbackForce = currentAttackType == AttackType.Light ? 5f : 10f;
                    enemyRb.AddForce(knockbackDir.normalized * knockbackForce, ForceMode.Impulse);
                }
            }
        }
    }

    private void UpdateElementMaterial()
    {
        Material elementMaterial = elementMaterials[(int)currentElementType];

        // Update the right hand's material to the current element
        Renderer rightHandRenderer = rightHand.GetComponent<Renderer>();
        rightHandRenderer.material = elementMaterial;
    }

    // Function to cycle through elements for debugging
    private void CycleElement()
    {
        currentElementType = (ElementType)(((int)currentElementType + 1) % 7); // Loop through elements
        UpdateElementMaterial();
        Debug.Log("Current Element: " + currentElementType);
    }
}
