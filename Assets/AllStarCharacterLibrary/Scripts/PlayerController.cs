using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : BaseCharacter
{
    [Header("Knockout & UI Settings")]
    [SerializeField] private GameObject knockedOutUIPanel;
    [SerializeField] private float showUIPanelDelay = 1.0f;
    [SerializeField] private float sceneResetDelay = 3.0f;

    private Vector2 rawMovementInput;
    private Rigidbody rb;
    private PlayerInput playerInput;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
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

    protected override void OnDeath()
    {
        base.OnDeath(); // Logs defeat message[cite: 25]

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
        // Wait for KO animation play context
        yield return new WaitForSeconds(showUIPanelDelay);

        // Enable Knocked Out UI panel
        if (knockedOutUIPanel != null)
        {
            knockedOutUIPanel.SetActive(true);
        }

        // Display UI before resetting scene
        yield return new WaitForSeconds(sceneResetDelay);

        // Reload the current scene to reset player & AI health to max
        string activeSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(activeSceneName);
    }

    private void FixedUpdate()
    {
        if (!IsAlive()) return; // Prevent movement processing after defeat

        Vector3 movementVector = new Vector3(rawMovementInput.x, 0f, rawMovementInput.y).normalized;
        rb.MovePosition(rb.position + movementVector * moveSpeed * Time.fixedDeltaTime);

        UpdateMovementAnimation(rawMovementInput);
    }
}