using UnityEngine;

[System.Serializable]

public class DamagePayload
{
    [Header("Payload Stats")]
    public float damageAmount;
    public float knockbackForce;

    [Header("Position Data")]
    public Vector3 hitPoint;
    public Vector3 knockbackDirection;

    [Header("Attacker Reference")]
    public GameObject attacker;

    public DamagePayload(float damage,float knockback, Vector3 direction, Vector3 point, GameObject source)
    {
        damageAmount = damage;
        knockbackForce = knockback;
        knockbackDirection = direction;
        hitPoint = point;
        attacker = source;
    }
}
