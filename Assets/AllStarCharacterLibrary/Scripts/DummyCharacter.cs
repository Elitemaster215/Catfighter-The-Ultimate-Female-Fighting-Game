using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class DummyCharacter : BaseCharacter
{
    [SerializeField] private bool autoResetHealthOnDeath = true;
    [SerializeField] private float respawnDelay = 2.0f;
    [SerializeField] private Renderer dummyRenderer;
    [SerializeField] private Color hitColor = Color.red;
    private Color originalColor;
    private Coroutine flashCoroutine;


    protected override void Awake()
    {
        base.Awake();
        if (dummyRenderer == null)
        {
            dummyRenderer = GetComponentInChildren<Renderer>();
        }

        if(dummyRenderer != null && dummyRenderer.material.HasProperty("_Color"))
        {
            originalColor = dummyRenderer.material.color;
        }
    }

    public override void TakeDamage(DamagePayload payload)
    {
        base.TakeDamage(payload);
        if(dummyRenderer != null && IsAlive())
        {
            StartCoroutine(HitFlashRoutine());
        }
    }

    private IEnumerator HitFlashRoutine()
    {
        dummyRenderer.material.color = hitColor;
        yield return new WaitForSeconds(0.15f);
        dummyRenderer.material.color = originalColor;
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        if(dummyRenderer != null)
        {
            dummyRenderer.material.color = Color.gray;
        }
        if(autoResetHealthOnDeath)
        {
            StartCoroutine(ResetDummyRoutine());
        }
    }
    private IEnumerator ResetDummyRoutine()
    {
        Debug.Log($"<color=#YELLOW>[{gameObject.name}] RECOVERY:</color> Reseting health in {respawnDelay} seconds...");
        yield return new WaitForSeconds(respawnDelay);

        currentHealth = maxHealth;
        if(dummyRenderer != null)
        {
            dummyRenderer.material.color = originalColor;
        }

        Debug.Log($"<color=#00FF00>[{gameObject.name}] READY:</color> Health fully restored({currentHealth}/{maxHealth}).");
    }

}
