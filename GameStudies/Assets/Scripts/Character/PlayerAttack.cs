using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerAttack : MonoBehaviour
{
    public LayerMask hitMask;
    public PlayerStats playerStats; // Reference to the PlayerStats ScriptableObject
    public GameObject rightHand; // Right hand object
    public Material[] elementMaterials; // Array of materials for elements

    // UI elements for cooldown timers
    public TextMeshProUGUI lightAttackCooldownText;
    public TextMeshProUGUI heavyAttackCooldownText;

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

        // Find and assign the UI TextMeshProUGUI components
        lightAttackCooldownText = GameObject.Find("LTimer").GetComponent<TextMeshProUGUI>();
        heavyAttackCooldownText = GameObject.Find("HTimer").GetComponent<TextMeshProUGUI>();

        // Initialize cooldown text
        UpdateCooldownText(lightAttackCooldownText, 0f);
        UpdateCooldownText(heavyAttackCooldownText, 0f);
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
            StartCoroutine(AttackCooldown(AttackType.Light, playerStats.currentLightAttackCooldownReduction, lightAttackCooldownText));
        }
        else if (attackType == AttackType.Heavy)
        {
            animator.SetTrigger("HeavyAttack");
            StartCoroutine(AttackCooldown(AttackType.Heavy, playerStats.currentHeavyAttackCooldownReduction, heavyAttackCooldownText));
        }
    }

    private IEnumerator AttackCooldown(AttackType attackType, float cooldownDuration, TextMeshProUGUI cooldownText)
    {
        if (attackType == AttackType.Light) canLightAttack = false;
        else if (attackType == AttackType.Heavy) canHeavyAttack = false;

        float timeRemaining = cooldownDuration;
        while (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateCooldownText(cooldownText, timeRemaining);
            yield return null;
        }

        UpdateCooldownText(cooldownText, 0f);

        if (attackType == AttackType.Light) canLightAttack = true;
        else if (attackType == AttackType.Heavy) canHeavyAttack = true;
    }

    private void UpdateCooldownText(TextMeshProUGUI text, float timeRemaining)
    {
        text.text = timeRemaining > 0 ? $"Cooldown: {timeRemaining:F1}s" : "Ready!";
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is an enemy
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                // Apply damage with elemental effect
                float damage = currentAttackType == AttackType.Light 
                    ? playerStats.currentDamage * playerStats.currentLightAttackDamage 
                    : playerStats.currentDamage * playerStats.currentHeavyAttackDamage;

                enemyHealth.TakeDamage(damage, currentElementType);

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
        Renderer rightHandRenderer = rightHand.GetComponent<Renderer>();
        rightHandRenderer.material = elementMaterial;
    }

    // Function to cycle through elements for debugging
    private void CycleElement()
    {
        currentElementType = (ElementType)(((int)currentElementType + 1) % elementMaterials.Length); // Loop through elements
        UpdateElementMaterial();
    }
}
