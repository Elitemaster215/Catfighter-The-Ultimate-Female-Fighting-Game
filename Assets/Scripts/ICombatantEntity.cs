using UnityEngine;

public interface ICombatantEntity
{
    // All combatants will have access to these functions
    void TakeDamage(DamagePayload payload);
    bool IsAlive();
}
