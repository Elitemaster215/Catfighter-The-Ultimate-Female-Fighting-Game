using UnityEngine;

public class AIBrain : MonoBehaviour
{
    [Header("State References")]
    [SerializeField] private AIState initialState;
    [SerializeField] private AIIdleState idleState;
    [SerializeField] private AIEngageState engageState;
    [SerializeField] private AIAttackState attackState;
    [SerializeField] private AIKnockOutState knockOutState; // Oct 29th

    [Header("Targeting and Character References")]
    [SerializeField] private Transform opponentTarget;
    [SerializeField] private BaseCharacter character;

    public AIIdleState IdleState => idleState;
    public AIEngageState EngageState => engageState;
    public AIAttackState AttackState => attackState;

    public AIKnockOutState KnockOutState => knockOutState; // Oct 29th
    public Transform OpponentTarget => opponentTarget;
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
        if (knockOutState != null) knockOutState.Initialize(this); // Oct 29th
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

    //Assign the opponent target when spawning fighters
    public void SetTarget(Transform target)
    {
        opponentTarget = target;
    }


}
