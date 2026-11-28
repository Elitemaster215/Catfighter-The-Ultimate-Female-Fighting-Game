using UnityEngine;

public class AIIdleState : AIState
{

    [SerializeField] private float idleDuration = 5.0f;
    private float timer;

    public override void EnterState()
    {
        timer = idleDuration;
    }

    public override void UpdateState()
    {
        timer -= Time.deltaTime;

        if(timer <= 0f)
        {
            // Transition to the next state
            brain.TransitionToState(brain.EngageState);
        }
    }

    public override void ExitState()
    {
        Debug.Log("Exiting the idle state");
    }



}
