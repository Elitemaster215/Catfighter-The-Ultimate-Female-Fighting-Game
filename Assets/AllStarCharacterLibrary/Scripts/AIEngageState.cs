using UnityEngine;

public class AIEngageState : AIState
{
    private enum EngagementMovementMode
    {
        InPosition,
        Approaching,
        Retreating
    }

    [Header("Distance Thresholds Movement Buffer")]
    [SerializeField] private float idealAttackDistance = 2.0f;
    [SerializeField] private float startApproachDistance = 3.0f;
    [SerializeField] private float startRetreatDistance = 1.0f;
    [SerializeField] private float rotationSpeed = 10f;

    private Rigidbody rb;
    private EngagementMovementMode currentMovementMode = EngagementMovementMode.InPosition;
    private Vector3 currentMovementDirection = Vector3.zero;

    // Damp time unique for the AI Engage State OCT 20
    [SerializeField] private float aiDampTime = 0.05f; 

    public override void Initialize(AIBrain aIBrain)
    {
        base.Initialize(aIBrain);
        rb = GetComponentInParent<Rigidbody>();
    }

    public override void EnterState()
    {
        currentMovementMode = EngagementMovementMode.InPosition;
        currentMovementDirection = Vector3.zero;
        Debug.Log("Entering the engage state");
    }

    public override void UpdateState()
    {
        if (brain.PlayerTarget == null || brain.Character == null) return;

        Transform characterTransform = brain.Character.transform;

        Vector3 directionToPlayer = brain.PlayerTarget.position - characterTransform.position;
        directionToPlayer.y = 0f;
        float currentDistance = directionToPlayer.magnitude;

        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            characterTransform.rotation = Quaternion.Slerp(characterTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        EvaluateMovementMode(currentDistance, directionToPlayer.normalized);

        // Check if the AI character is in position to attack
        if(currentMovementMode == EngagementMovementMode.InPosition && brain.AttackState != null)
        {
            brain.TransitionToState(brain.AttackState);
            return;
        }

        // Update the Animation values OCT 20
        UpdateAnimatorValues(characterTransform);
    }

    // Update the Animator Blend Tree values OCT 20
    private void UpdateAnimatorValues(Transform characterTransform)
    {
        if (brain.Character == null) return;

        Vector3 localMovement = characterTransform.InverseTransformDirection(currentMovementDirection);
        Vector2 animInput = new Vector2(localMovement.z, localMovement.x);

        if (animInput.sqrMagnitude > 0.01f)
        {
            animInput.Normalize();
        }

        brain.Character.UpdateMovementAnimation(animInput, aiDampTime);
    }

    private void EvaluateMovementMode(float distance, Vector3 normalizedDirection)
    {
        switch (currentMovementMode)
        {
            case EngagementMovementMode.InPosition:
                if (distance > startApproachDistance)
                {
                    currentMovementMode = EngagementMovementMode.Approaching;
                }
                else if (distance < startRetreatDistance)
                {
                    currentMovementMode = EngagementMovementMode.Retreating;
                }
                currentMovementDirection = Vector3.zero;
                break;

            case EngagementMovementMode.Approaching:
                if (distance <= idealAttackDistance)
                {
                    currentMovementMode = EngagementMovementMode.InPosition;
                    currentMovementDirection = Vector3.zero;
                }
                else
                {
                    currentMovementDirection = normalizedDirection;
                }
                break;

            case EngagementMovementMode.Retreating:
                if (distance >= idealAttackDistance)
                {
                    currentMovementMode = EngagementMovementMode.InPosition;
                    currentMovementDirection = Vector3.zero;
                }
                else
                {
                    currentMovementDirection = -normalizedDirection;
                }
                break;
        }
    }

    // get the move speed from the DummyChararacter class OCT 20
    private void FixedUpdate()
    {
        if (rb != null && currentMovementDirection != Vector3.zero)
        {
            float moveSpeed = brain.Character != null ? brain.Character.MoveSpeed : 4f;

            rb.MovePosition(rb.position + currentMovementDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }

    public override void ExitState()
    {
        currentMovementMode = EngagementMovementMode.InPosition;
        currentMovementDirection = Vector3.zero;

        if (brain.Character != null)
        {
            brain.Character.UpdateMovementAnimation(Vector2.zero);
        }

        Debug.Log("Exiting the engage state");
    }
}