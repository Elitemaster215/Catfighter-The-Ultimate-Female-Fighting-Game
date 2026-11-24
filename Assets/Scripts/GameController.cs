using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameMode { PlayerVsAI, AIvsAI };

public class GameController : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private InputActionProperty mainMenuInputAction;
    [SerializeField] public GameObject characterSelectionPanel;
    [SerializeField] private GameObject gameHUDPanel;
    [SerializeField] private HealthBarUI healthBarUI;

    [Header("Cinemachine settings")]
    [SerializeField] private CinemachineTargetGroup targetGroup;
    [SerializeField] private float targetWeight = 1.0f;
    [SerializeField] private float targetRadius = 1.0f;

    [Header("Spawn Locations")]
    [SerializeField] private Transform spawnPoint1;
    [SerializeField] private Transform spawnPoint2;

    [Header("Character Prefabs")]
    [SerializeField] private GameObject playerPreb;
    [SerializeField] private GameObject aiFighter1Prefab;
    [SerializeField] private GameObject aiFighter2Prefab;

    private GameObject fighter1Instance;
    private GameObject fighter2Instance;

    [Header("UI Images")]
    [SerializeField] private Image PvA;
    [SerializeField] private Image AvA;
    [SerializeField] private Sprite activeImage;
    [SerializeField] private Sprite disabledImage;

    public static bool fightOngoing = false;
    private void OnEnable()
    {
        mainMenuInputAction.action?.Enable();
    }

    private void OnDisable()
    {
        mainMenuInputAction.action?.Disable();
    }
    private void Start()
    {
        PvA.sprite = activeImage;
        AvA.sprite = disabledImage;

        if (MatchData.fightOngoing == true)
        {
            CloseCharacterSelectionPanel();
            if (MatchData.matchisPvAI == true) { SpawnFighters(GameMode.PlayerVsAI); }
            else if (MatchData.matchisPvAI == false) { SpawnFighters(GameMode.AIvsAI); }
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void Update()
    {
        if(mainMenuInputAction.action != null && mainMenuInputAction.action.WasPressedThisFrame())
        {
            ReturnToMainMenu();
        }
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Returning to main menu");
        SceneManager.LoadScene(mainMenuSceneName);
        fightOngoing = false;
    }

    public void SelectPlayerVsAI()
    {
        PvA.sprite = activeImage;
        AvA.sprite = disabledImage;
        MatchData.matchisPvAI = true;
    }

    //UI Button Handler for AI vs AI
    public void SelectAIVsAI()
    {
        PvA.sprite = disabledImage;
        AvA.sprite = activeImage;
        MatchData.matchisPvAI = false;
    }

    public void StartGame()
    {
        //Spawn fighters
        if (MatchData.matchisPvAI == true)
        {
            SpawnFighters(GameMode.PlayerVsAI);
            CloseCharacterSelectionPanel();
        }
        else if (MatchData.matchisPvAI == false)
        {
            SpawnFighters(GameMode.AIvsAI);
            CloseCharacterSelectionPanel();
        }
    }

    // Set the opponent targets for the AI in the AI brain
    private void InitializeFighterConnections()
    {
        if (fighter1Instance == null || fighter2Instance == null) return;

        AIBrain brain1 = fighter1Instance.GetComponent<AIBrain>();
        AIBrain brain2 = fighter2Instance.GetComponent<AIBrain>();

        if (brain1 != null)
        {
            brain1.SetTarget(fighter2Instance.transform);
            if (brain1.KnockOutState != null)
            {
                brain1.KnockOutState.SetIsFigher1(true);
            }
        }
        if (brain2 != null)
        {
            brain2.SetTarget(fighter1Instance.transform);
            if (brain2.KnockOutState != null)
            {
                brain2.KnockOutState.SetIsFigher1(false);
            }
        }

        healthBarUI.fighter1Character = fighter1Instance.GetComponent<BaseCharacter>();
        healthBarUI.fighter2Character = fighter2Instance.GetComponent<BaseCharacter>();
    }

    private void UpdateHeathBarUI()
    {

    }

    private void UpdateCinemachineTargetGroup()
    {
        if (targetGroup == null || fighter1Instance == null || fighter2Instance == null) return;

        targetGroup.Targets = new List<CinemachineTargetGroup.Target>
        {
            new CinemachineTargetGroup.Target
            {
                Object = fighter1Instance.transform,
                Weight = targetWeight,
                Radius = targetRadius
            },
            new CinemachineTargetGroup.Target
            {
                Object = fighter2Instance.transform,
                Weight = targetWeight,
                Radius = targetRadius
            }
        };

    }

    public void SpawnFighters(GameMode mode)
    {
        // Destroy any existing fighters
        if (fighter1Instance != null) Destroy(fighter1Instance);
        if (fighter2Instance != null) Destroy(fighter2Instance);

        if (mode == GameMode.PlayerVsAI)
        {
            fighter1Instance = Instantiate(playerPreb, spawnPoint1.position, spawnPoint1.rotation);
            fighter2Instance = Instantiate(aiFighter1Prefab, spawnPoint2.position, spawnPoint2.rotation);
        }
        else if (mode == GameMode.AIvsAI)
        {
            fighter1Instance = Instantiate(aiFighter2Prefab, spawnPoint1.position, spawnPoint1.rotation);
            fighter2Instance = Instantiate(aiFighter1Prefab, spawnPoint2.position, spawnPoint2.rotation);
        }

        InitializeFighterConnections();
        UpdateCinemachineTargetGroup();
        UpdateHeathBarUI();
    }

    public void CloseCharacterSelectionPanel()
    {
        if(characterSelectionPanel != null)
        {
            characterSelectionPanel.SetActive(false);
        }
        if (gameHUDPanel != null)
        {
            gameHUDPanel.SetActive(true);
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
