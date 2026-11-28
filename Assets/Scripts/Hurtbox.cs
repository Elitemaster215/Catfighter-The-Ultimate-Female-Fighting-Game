using UnityEngine;

[RequireComponent(typeof(Collider))]


public class Hurtbox : MonoBehaviour
{
    [SerializeField] private GameObject ownerEntity;
    private ICombatantEntity combatantContract;

    private void Awake()
    {
        if(ownerEntity == null)
        {
            ownerEntity = transform.root.gameObject;
        }

        combatantContract = ownerEntity.GetComponent<ICombatantEntity>();

        if(combatantContract == null)
        {
            Debug.LogError($"<color=red>[HURTBOX ERROR]</color> No combatantEntity script found on {ownerEntity.name}", this);
        }
        Collider hurtboxCollider = GetComponent<Collider>();
        if(hurtboxCollider != null && hurtboxCollider.isTrigger)
        {
            hurtboxCollider.isTrigger = true;
        }
    }

    public void ReceivePayload(DamagePayload payload)
    {
        if(combatantContract != null && combatantContract.IsAlive())
        {
            combatantContract.TakeDamage(payload);
        }
    }

    public GameObject GetOwner() => ownerEntity;

}
