using UnityEngine;

public class AIBrain : MonoBehaviour
{
    [Header("State References")]
    [SerializeField] private AIState initialState;
    [SerializeField] private AIIdleState idleState;
    [SerializeField] private AIEngageState engageState;
    [SerializeField] private AIAttackState attackState;

    [Header("Targeting and Character References")]
    [SerializeField] private Transform playerTarget;
    [SerializeField] private BaseCharacter character;

    public AIIdleState IdleState => idleState;
    public AIEngageState EngageState => engageState;
    public AIAttackState AttackState => attackState;
    public Transform PlayerTarget => playerTarget;
    public BaseCharacter Character => character;

    private AIState currentState;



    private void Awake()
    {
        if (character == null)
        {
            character = GetComponent<BaseCharacter>();
        }

        if (idleState != null) idleState.Initialize(this);
        if (engageState != null) engageState.Initialize(this);
        if (attackState != null) attackState.Initialize(this);
    }


    void Start()
    {
        if (initialState != null)
        {
            TransitionToState(initialState);
        }
        else if (idleState != null)
        {
            TransitionToState(idleState);
        }
        else
        {
            Debug.Log("References to inital state or idle state is null");
        }
    }

    void Update()
    {
        if(currentState != null)
        {
            currentState.UpdateState();
        }
    }

    public void TransitionToState(AIState newState)
    {
        if (newState == null || newState == currentState) return;

        if(currentState != null)
        {
            currentState.ExitState();
        }
        currentState = newState;
        currentState.EnterState();

    }


}
