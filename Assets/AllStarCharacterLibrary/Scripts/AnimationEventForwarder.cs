using UnityEngine;

public class AnimationEventForwarder : MonoBehaviour
{

    private BaseCharacter character;

    private void Awake()
    {
        character = GetComponentInParent<BaseCharacter>();
    }

    public void AE_EnableHitbox(string id)
    {
        if (character != null) character.AE_EnableHitbox(id);
    }

    public void AE_DisableHitbox(string id)
    {
        if (character != null) character.AE_DisableHitbox(id);
    }

    public void AE_DisableAllHitboxes()
    {
        if (character != null) character.AE_DisableAllHitboxes();
    }


}
