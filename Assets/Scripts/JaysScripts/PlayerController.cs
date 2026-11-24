using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : BaseCharacter
{
    [Header("Knockout & UI Settings")]
    [SerializeField] private GameObject knockedOutUIPanel; // Oct 27th 
    [SerializeField] private float showUIPanelDelay = 1.0f;
    [SerializeField] private float sceneResetDelay = 3.0f;

    private Vector2 rawMovementInput;
    private Rigidbody rb;
    private PlayerInput playerInput;

    [Header("Jump")]
    public Vector3 jump;
    public float jumpForce = 2f;
    public bool isGrounded;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        jump = new Vector3(0f, 2f, 0f);
    }

    public void OnMove(InputValue value)
    {
        if (!IsAlive()) return;
        rawMovementInput = value.Get<Vector2>();
    }

    public void OnLightAttack(InputValue value)
    {
        if (!IsAlive()) return;
        if (value.isPressed) ExecuteLightAttack();
    }

    public void OnHeavyAttack(InputValue value)
    {
        if (!IsAlive()) return;
        if (value.isPressed) ExecuteHeavyAttack();
    }

    public void OnJump(InputValue value)
    {
        if (!IsAlive()) return;
        if (value.isPressed) rb.AddForce(jump * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }
    void OnCollisionStay()
    {
        isGrounded = true;
    }

    protected override void OnDeath()
    {
        base.OnDeath(); 

        // 1. Disable player input component to lock controls
        if (playerInput != null) playerInput.enabled = false;

        // 2. Stop current movement and animations
        rawMovementInput = Vector2.zero;
        UpdateMovementAnimation(Vector2.zero);

        // 3. Fire KnockedOut animation trigger
        if (animator != null)
        {
            animator.SetTrigger(knockedOutTriggerHash);
        }

        // 4. Begin the UI and scene reload sequence
        StartCoroutine(KnockoutSequenceRoutine());
    }

    private IEnumerator KnockoutSequenceRoutine()
    {
        // Award round win to AI opponent
        MatchData.Fighter2Wins++;

        yield return new WaitForSeconds(showUIPanelDelay);

        // Display Round/Match Result UI
        if (knockedOutUIPanel != null)
        {
            knockedOutUIPanel.SetActive(true);
        }

        yield return new WaitForSeconds(sceneResetDelay);

        // Evaluate Match Winner vs Next Round
        if (MatchData.HasFighter2WonMatch())
        {
            Debug.Log($"MATCH OVER: AI Wins the Match! ({MatchData.Fighter2Wins} - {MatchData.Fighter1Wins})");
            // Reset scores if player restarts match or returns to main menu
            MatchData.ResetMatch();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            Debug.Log($"ROUND OVER: AI leads ({MatchData.Fighter2Wins} - {MatchData.Fighter1Wins})");
            // Reload scene for next round
            MatchData.MatchOngoing();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void FixedUpdate()
    {
        if (!IsAlive()) return; // Prevent movement processing after defeat

        Vector3 movementVector = new Vector3(rawMovementInput.x, 0f, rawMovementInput.y).normalized;
        rb.MovePosition(rb.position + movementVector * moveSpeed * Time.fixedDeltaTime);

        UpdateMovementAnimation(rawMovementInput);
    }
}