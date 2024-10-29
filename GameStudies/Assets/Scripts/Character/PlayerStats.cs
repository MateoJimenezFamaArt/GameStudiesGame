using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats", order = 1)]
public class PlayerStats : ScriptableObject
{
    public float defaultHealth = 100f;
    public float defaultSpeed = 5f;
    public float defaultDamage = 20f;
    public float defaultDiveForce = 10f;
    public float defaultLightAttackCooldownReduction = 1f;
    public float defaultHeavyAttackCooldownReduction = 1f;
    public float defaultJumpForce = 10f;
    public float defaultBounciness = 1f;
    public float defaultHeavyAttackDamage = 3f;
    public float defaultLightAttackDamage = 1f;

    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentSpeed;
    [HideInInspector] public float currentDamage;
    [HideInInspector] public float currentDiveForce;
    [HideInInspector] public float currentLightAttackCooldownReduction;
    [HideInInspector] public float currentHeavyAttackCooldownReduction;
    [HideInInspector] public float currentJumpForce;
    [HideInInspector] public float currentBounciness;
    [HideInInspector] public float currentHeavyAttackDamage;
    [HideInInspector] public float currentLightAttackDamage;

    // Events for each stat change
    public UnityEvent<float> OnHealthChanged = new UnityEvent<float>();
    public UnityEvent<float> OnSpeedChanged = new UnityEvent<float>();
    public UnityEvent<float> OnDamageChanged = new UnityEvent<float>();
    public UnityEvent<float> OnDiveForceChanged = new UnityEvent<float>();
    public UnityEvent<float> OnLightAttackCooldownReductionChanged = new UnityEvent<float>();
    public UnityEvent<float> OnHeavyAttackCooldownReductionChanged = new UnityEvent<float>();
    public UnityEvent<float> OnJumpForceChanged = new UnityEvent<float>();
    public UnityEvent<float> OnBouncinessChanged = new UnityEvent<float>();
    public UnityEvent<float> OnHeavyAttackDamageChanged = new UnityEvent<float>();
    public UnityEvent<float> OnLightAttackDamageChanged = new UnityEvent<float>();

    public void ResetStats()
    {
        ModifyHealth(defaultHealth - currentHealth);
        ModifySpeed(defaultSpeed - currentSpeed);
        ModifyDamage(defaultDamage - currentDamage);
        ModifyDiveForce(defaultDiveForce - currentDiveForce);
        ModifyLightAttackCooldownReduction(defaultLightAttackCooldownReduction - currentLightAttackCooldownReduction);
        ModifyHeavyAttackCooldownReduction(defaultHeavyAttackCooldownReduction - currentHeavyAttackCooldownReduction);
        ModifyJumpForce(defaultJumpForce - currentJumpForce);
        ModifyBounciness(defaultBounciness - currentBounciness);
        ModifyHeavyAttackDamage(defaultHeavyAttackDamage - currentHeavyAttackDamage);
        ModifyLightAttackDamage(defaultLightAttackDamage - currentLightAttackDamage);
    }

    public void ModifyHealth(float amount) { currentHealth += amount; OnHealthChanged.Invoke(currentHealth); }
    public void ModifySpeed(float amount) { currentSpeed += amount; OnSpeedChanged.Invoke(currentSpeed); }
    public void ModifyDamage(float amount) { currentDamage += amount; OnDamageChanged.Invoke(currentDamage); }
    public void ModifyDiveForce(float amount) { currentDiveForce += amount; OnDiveForceChanged.Invoke(currentDiveForce); }
    public void ModifyLightAttackCooldownReduction(float amount) { currentLightAttackCooldownReduction += amount; OnLightAttackCooldownReductionChanged.Invoke(currentLightAttackCooldownReduction); }
    public void ModifyHeavyAttackCooldownReduction(float amount) { currentHeavyAttackCooldownReduction += amount; OnHeavyAttackCooldownReductionChanged.Invoke(currentHeavyAttackCooldownReduction); }
    public void ModifyJumpForce(float amount) { currentJumpForce += amount; OnJumpForceChanged.Invoke(currentJumpForce); }
    public void ModifyBounciness(float amount) { currentBounciness += amount; OnBouncinessChanged.Invoke(currentBounciness); }
    public void ModifyHeavyAttackDamage(float amount) { currentHeavyAttackDamage += amount; OnHeavyAttackDamageChanged.Invoke(currentHeavyAttackDamage); }
    public void ModifyLightAttackDamage(float amount) { currentLightAttackDamage += amount; OnLightAttackDamageChanged.Invoke(currentLightAttackDamage); }
}
