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

    private AIBrain aiBrain;


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
        aiBrain = GetComponent<AIBrain>(); // Oct 29th
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
            dummyRenderer.material.color = Color.darkGray;
        }
       if(aiBrain != null && aiBrain.KnockOutState != null)
        {
            aiBrain.TransitionToState(aiBrain.KnockOutState);
        }
    }



}
