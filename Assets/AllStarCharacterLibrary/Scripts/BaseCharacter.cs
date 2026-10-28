using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the base class for all characters both for human players and AI controlled characters
public abstract class BaseCharacter: MonoBehaviour, ICombatantEntity
{
    [Header("Core Stats")]
    [SerializeField] protected float currentHealth = 100f;
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float moveSpeed = 8f;
    [SerializeField] protected float attackCooldownDuration = 1.2f;

    [Header("Visual & Animation")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected bool disableRootMotion = true;

    protected Rigidbody characterRigidbody;

    //Shared Animator Parameter Hashes
    protected int speedHash;
    protected int moveXHash;
    protected int moveZHash;
    protected int abilityTriggerHash;
    protected int ability2TriggerHash;
    protected int knockedOutTriggerHash;

    // Added for updating animation : OCT 20
    [SerializeField] protected float dampTime = 0.15f;


    //Cooldown & Feature Flags
    protected bool isCooldownActive = false;

    [SerializeField] protected List<Hitbox> hitboxes = new List<Hitbox>();
    private Dictionary<string, Hitbox> hitboxMap = new Dictionary<string, Hitbox>();

    [SerializeField] protected CharacterProfileSO characterProfile;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    
    // Updates the Move Speed for the AI Character OCT 20
    public float MoveSpeed => moveSpeed;

    protected virtual void Awake()
    {
        characterRigidbody = GetComponent<Rigidbody>();

        if(animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        // Initialize the Character profile to update the character settings
        InitializeFromProfile();

        if(animator != null)
        {
            if (disableRootMotion) animator.applyRootMotion = false;

            speedHash = Animator.StringToHash("Speed");
            moveXHash = Animator.StringToHash("MoveX");
            moveZHash = Animator.StringToHash("MoveZ");
            abilityTriggerHash = Animator.StringToHash("AbilityTrigger");
            ability2TriggerHash = Animator.StringToHash("Ability2Trigger");
            knockedOutTriggerHash = Animator.StringToHash("KnockedOut");
        }

        InitializeHitboxMap();
    }


    protected virtual void InitializeFromProfile()
    {
        if(characterProfile != null)
        {
            maxHealth = characterProfile.maxHealth;
            currentHealth = maxHealth;
            moveSpeed = characterProfile.moveSpeed;
            attackCooldownDuration = characterProfile.attackCooldown;

            if(characterRigidbody != null)
            {
                characterRigidbody.mass = characterProfile.characterMass;
            }
        }
    }

    private void InitializeHitboxMap()
    {
        if(hitboxes.Count == 0)
        {
            hitboxes.AddRange(GetComponentsInChildren<Hitbox>(true));
        }
        foreach(Hitbox hb in hitboxes)
        {
            if(hb != null && !hitboxMap.ContainsKey(hb.HitBoxID))
            {
                hitboxMap.Add(hb.HitBoxID, hb);// Adds to the dictionary : String Name "Right Hand" to the hitbox
            }
        }
    }

    // Triggering the Animation Events

    public virtual void AE_EnableHitbox(string id)
    {
        if(hitboxMap.TryGetValue(id, out Hitbox hb))
        {
            hb.EnableHitbox();
        }
        else
        {
            Debug.Log("Hitbox ID is not found on the hitbox dictionary");
        }
    }

    public virtual void AE_DisableHitbox(string id)
    {
        if(hitboxMap.TryGetValue(id, out Hitbox hb))
        {
            hb.DisableHitbox();
        }
    }

    public virtual void AE_DisableAllHitboxes()
    {
        foreach(var hb in hitboxMap.Values)
        {
            hb.DisableHitbox();
        }
    }

    public virtual void ExecuteLightAttack()
    {
        if (isCooldownActive) return;

        if(animator != null)
        {
            animator.SetTrigger(abilityTriggerHash);
        }
        StartCoroutine(LightAttackCooldownRoutine());
    }

    public virtual void ExecuteHeavyAttack()
    {
        if (isCooldownActive) return;

        if (animator != null)
        {
            animator.SetTrigger(ability2TriggerHash);
        }
        StartCoroutine(LightAttackCooldownRoutine());
    }


    public virtual IEnumerator LightAttackCooldownRoutine()
    {
        isCooldownActive = true;
        yield return new WaitForSeconds(attackCooldownDuration);
        isCooldownActive = false;
    }


    public virtual void TakeDamage(DamagePayload payload)
    {
        currentHealth = Mathf.Max(currentHealth - payload.damageAmount, 0f);
        string attackerName = payload.attacker != null ? payload.attacker.name : "Unknown Source";

        Debug.Log($"<color=#FF4500>[{gameObject.name}] HIT REGISTERED:</color> " +
            $"Took {payload.damageAmount} HP from {attackerName}. " + $"Remaining HP: {currentHealth} / {maxHealth}");

        
        if (!IsAlive())
        {
            {
                OnDeath();
            }
        }
        ApplyKnockback(payload.knockbackDirection, payload.knockbackForce);
        
    }

    protected virtual void ApplyKnockback(Vector3 direction, float force)
    {
      if(characterRigidbody != null && force > 0f)
        {
            Vector3 finalForce = direction.normalized * (force / characterRigidbody.mass);
            characterRigidbody.AddForce(finalForce, ForceMode.Impulse);
        }
    }

    public bool IsAlive() => currentHealth > 0f;

    protected virtual void OnDeath()
    {
        Debug.Log($"[{gameObject.name}] Has been defeated.");
    }

    // Updated the Damp Time : OCT 20
    public virtual void UpdateMovementAnimation(Vector2 moveInput, float dampTimeOverride = -1f)
    {
        if (animator == null) return;

        float activeDampTime = dampTimeOverride >= 0f ? dampTimeOverride : dampTime;

        animator.SetFloat(moveXHash, moveInput.x, activeDampTime, Time.deltaTime);
        animator.SetFloat(moveZHash, moveInput.y, activeDampTime, Time.deltaTime);
        animator.SetFloat(speedHash, moveInput.magnitude, activeDampTime, Time.deltaTime);
    }

}
