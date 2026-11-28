using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AIKnockOutState : AIState
{
    [Header("UI Scene Reset Settings")]
    [SerializeField] private GameObject knockOutPanel;
    [SerializeField] private float showUIPanelDelay = 1.0f;
    [SerializeField] private float sceneResetDelay = 3.0f;

    [SerializeField] private bool isFighter1 = false;


    public override void EnterState()
    {
        if (brain.Character != null)
        {
            //Zero out the locomotion parameters
            brain.Character.UpdateMovementAnimation(Vector2.zero);

            //Trigger the knockedOut animation
            brain.Character.ExecuteKnockOut();
        }
        StartCoroutine(KnockOutSequenceRoutine());
    }
    // start the coroutine to reset the scene

    private IEnumerator KnockOutSequenceRoutine()
    {
        // if fighter 1 was knocked out
       if(isFighter1)
        {
            MatchData.Fighter2Wins++; // Fighter 2 wins the round
        }
        else
        {
            MatchData.Fighter1Wins++;
        }

            yield return new WaitForSeconds(showUIPanelDelay);

        if (knockOutPanel != null)
        {
            knockOutPanel.SetActive(true);
        }

        yield return new WaitForSeconds(sceneResetDelay);

        // Evaluate Match Winner vs Next Round
        if (MatchData.HasFighter1WonMatch())
        {
            Debug.Log($"MATCH OVER: Fighter 1 Wins the Match! ({MatchData.Fighter1Wins} - {MatchData.Fighter2Wins})");
            MatchData.ResetMatch();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            Debug.Log($"ROUND OVER: Fighter 1 leads ({MatchData.Fighter1Wins} - {MatchData.Fighter2Wins})");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
    }

    public void SetIsFigher1(bool fighter1)
    {
        isFighter1 = fighter1;
    }

}





