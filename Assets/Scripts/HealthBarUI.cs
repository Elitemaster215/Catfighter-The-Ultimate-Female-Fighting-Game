using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("Player Health Bar")]
    public BaseCharacter fighter1Character;
    [SerializeField] private Image playerHealthBarImage;

    [Header("Opponent Health Bar")]
    public BaseCharacter fighter2Character;
    [SerializeField] private Image opponentHealthBarImage;

    //Oct 30 
    [Header("Round Score Displays")]
    [SerializeField] private TMP_Text playerWinsText;
    [SerializeField] private TMP_Text aiWinsText;


    //Oct 30 
    private void Start()
    {
        // Displays current round score (e.g. "Rounds: 1 / 2")
        if (playerWinsText != null)
            playerWinsText.text = $"Player Wins: {MatchData.Fighter1Wins}";

        if (aiWinsText != null)
            aiWinsText.text = $"Opponent Wins: {MatchData.Fighter2Wins}";
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthDisplay(fighter1Character, playerHealthBarImage);
        UpdateHealthDisplay(fighter2Character, opponentHealthBarImage);
    }

    private void UpdateHealthDisplay(BaseCharacter  character, Image fillImage)
    {
        if (character == null || fillImage == null) return;

        if(character.MaxHealth > 0f)
        {
            float fillRatio = Mathf.Clamp01(character.CurrentHealth / character.MaxHealth);
            fillImage.fillAmount = fillRatio;
        }
    }


}
