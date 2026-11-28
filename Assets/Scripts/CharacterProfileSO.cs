using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterProfile", menuName = "ScriptableObjects/Character Profile")]

public class CharacterProfileSO : ScriptableObject
{
    public string characterName = "Default Fighter";
    public Sprite characterPortrait;
    public GameObject characterPrefab;

    public float maxHealth = 100f;
    public float moveSpeed = 8f;
    public float characterMass = 1.0f;
    public float attackCooldown = 1.2f;
}
