using UnityEngine;

public class AIAttackState : AIState
{
    [Header("Attack Timing & Rotation")]
    [SerializeField] private float attackStateDuration = 1.2f;
    [SerializeField] private float faceTargetRotationSpeed = 5f;

    [Header("Randomization Tuning")]
    [Range(0f, 1f)]
    [SerializeField] private float heavyAttackChance = 0.5f;

    private float timer;

    public override void EnterState()
    {
        timer = attackStateDuration;

        if((brain.Character != null))
        {
            brain.Character.UpdateMovementAnimation(Vector2.zero);

            // Choose a random float between 0 and 1
            if(Random.value < heavyAttackChance)
            {
                brain.Character.ExecuteHeavyAttack();
                Debug.Log("Executing heavy attack");
            }
            else
            {
                brain.Character.ExecuteLightAttack();
                Debug.Log("Executing light attack");
            }
        }
        Debug.Log("Entering Attack State");
    }


    public override void UpdateState()
    {
        //Rotate to face the player/opponent when attacking
        if(brain.PlayerTarget != null & brain.Character != null)
        {
            Vector3 directionToPlayer = brain.PlayerTarget.position - brain.Character.transform.position;
            directionToPlayer.y = 0f;

            if(directionToPlayer != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                brain.Character.transform.rotation = Quaternion.Slerp(brain.Character.transform.rotation,
                    targetRotation, faceTargetRotationSpeed * Time.deltaTime);
            }

        }
        timer -= Time.deltaTime;

        //Return to the Engage state when the timer reaches zero
        if(timer <= 0)
        {
            brain.TransitionToState(brain.EngageState);
        }
    }

    public override void ExitState()
    {
        Debug.Log("Exiting the Attack State");
    }


}
