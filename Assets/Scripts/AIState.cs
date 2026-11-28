using UnityEngine;

public abstract class AIState : MonoBehaviour
{
    protected AIBrain brain;

    public virtual void Initialize(AIBrain aIBrain)
    {
        brain = aIBrain;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
   
}
