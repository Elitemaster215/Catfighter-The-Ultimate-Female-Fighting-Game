using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]

public class Hitbox : MonoBehaviour
{
    [SerializeField] private string hitboxID = "RightHand"; // Unique name for the hitbox

    [SerializeField] private float damageAmount = 15f;
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private GameObject attackerSource;

    private HashSet<Collider> hitTargetsThisAttack = new HashSet<Collider>();
    private Collider hitboxCollider;
    public string HitBoxID => hitboxID;

    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private float effectDestroyDelay = 1f;
    [SerializeField] private float targetOffsetDistance = 0.15f;

    private void Awake()
    {
        if(attackerSource == null)
        {
            attackerSource = transform.root.gameObject;
        }

        hitboxCollider = GetComponent<Collider>();
        if(hitboxCollider != null)
        {
            hitboxCollider.isTrigger = true;
            hitboxCollider.enabled = false;

        }
    }

    public void EnableHitbox()
    {
        Debug.Log("Enabling the hitbox");
        hitTargetsThisAttack.Clear();
        if (hitboxCollider != null)
        {
            hitboxCollider.enabled = true;
        }
        else
        {
            Debug.Log("Hitbox collider is null");
        }
           
    }

    public void DisableHitbox()
    {
        if (hitboxCollider != null) hitboxCollider.enabled = false;
    }

    private void OnEnable()
    {
        hitTargetsThisAttack.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hitTargetsThisAttack.Contains(other)) return;

        if (other.TryGetComponent<Hurtbox>(out Hurtbox targetHurtbox))
        {
            if (targetHurtbox.GetOwner() == attackerSource) return;

            hitTargetsThisAttack.Add(other);

            Vector3 hitPoint = other.ClosestPoint(transform.position);
            Vector3 directionToTarget = (other.transform.position - hitPoint);
            directionToTarget.y = 0f;
            directionToTarget.Normalize();

            Vector3 offsetHitPoint = hitPoint + (directionToTarget * targetOffsetDistance);


            SpawnHitEffect(offsetHitPoint);

            Vector3 knockbackDirection = (other.transform.position - attackerSource.transform.position).normalized;
            knockbackDirection.y = 0.3f; // knocking the opponent slightly upward

            DamagePayload payload = new DamagePayload(
              damageAmount,
              knockbackForce,
              knockbackDirection,
              other.ClosestPoint(transform.position),
              attackerSource
            );

            targetHurtbox.ReceivePayload(payload);

            Invoke(nameof(ResetHitMemory), 0.5f);
        }
    }

    private void ResetHitMemory()
    {
        hitTargetsThisAttack.Clear();
    }

    private void SpawnHitEffect(Vector3 spawnPosition)
    {
        if(hitEffectPrefab != null)
        {
            GameObject tempEffect = Instantiate(hitEffectPrefab, spawnPosition, Quaternion.identity);

            Destroy(tempEffect, effectDestroyDelay);
        }
    }



}
