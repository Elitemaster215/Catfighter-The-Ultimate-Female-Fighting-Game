using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    [Header("Player Health Bar")]
    [SerializeField] private BaseCharacter playerCharacter;
    [SerializeField] private Image playerHealthBarImage;

    [Header("Opponent Health Bar")]
    [SerializeField] private BaseCharacter opponentCharacter;
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
            playerWinsText.text = $"Wins: {MatchData.PlayerWins}";

        if (aiWinsText != null)
            aiWinsText.text = $"Wins: {MatchData.AIWins}";
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthDisplay(playerCharacter, playerHealthBarImage);
        UpdateHealthDisplay(opponentCharacter, opponentHealthBarImage);
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
