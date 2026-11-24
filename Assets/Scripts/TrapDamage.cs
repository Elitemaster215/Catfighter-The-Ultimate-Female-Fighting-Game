using System.Collections.Generic;
using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    [SerializeField] private float damageAmount = 15f;
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private GameObject attackerSource;

    private Collider hitboxCollider;

    [SerializeField] private GameObject trap;
    private float trapTimer = 10f;
    private float trapCooldown = 10f;
    public bool trapActive = false;

    private void Awake()
    {
        trap.SetActive(false);

        if (attackerSource == null)
        {
            attackerSource = transform.root.gameObject;
        }

        hitboxCollider = GetComponent<Collider>();
        if (hitboxCollider != null)
        {
            hitboxCollider.isTrigger = true;
        }
    }

    private void Update()
    {
        if (trapActive == false)
        {
            trap.SetActive(false);
            hitboxCollider.enabled = false;

            if (trapCooldown <= 0)
            {
                trapActive = true;
                trapCooldown = 10f;
            }
            else
            {
                trapCooldown -= Time.deltaTime;
            }
        }
        if (trapActive == true)
        {
            trap.SetActive(true);
            hitboxCollider.enabled = true;

            if (trapTimer <= 0)
            {
                trapActive = false;
                trapTimer = 10f;
            }
            else
            {
                trapTimer -= Time.deltaTime;
            }
        }
        //Debug.Log("trapCooldown " + trapCooldown + " trapTimer " + trapTimer + trapActive);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Hurtbox>(out Hurtbox targetHurtbox))
        {
            if (targetHurtbox.GetOwner() == attackerSource) return;

            Debug.Log(attackerSource + "character entered trap");

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
        }
    }
}
