using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public enum GameMode { PlayerVsAI, AIvsAI }; // Game Mode Enum


public class GameController : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenuScene";
    [SerializeField] private InputActionProperty mainMenuInputAction;
    [SerializeField] private GameObject characterSelectionPanel;
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

    public AudioManager audioManager;
    private object activeImage;

    public object PvA { get; private set; }
    public object AvA { get; private set; }

    private void Start()
    {
        if (!MatchData.IsMatchInProgess)
        {
            if (characterSelectionPanel != null) characterSelectionPanel.SetActive(true);
            if (gameHUDPanel != null) gameHUDPanel.SetActive(false);
        }
        else
        {
            SpawnFighters(MatchData.SelectedGameMode);
            CloseCharacterSelectionPanel();
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        PlayMusic();

    }

    private void PlayMusic()
    {
        if (audioManager != null)
        {
            audioManager.playMusic();
        }
        else
        {
            Debug.Log("Audio manager is null");
        }
    }

    private void OnEnable()
    {
        mainMenuInputAction.action?.Enable();
    }

    private void OnDisable()
    {
        mainMenuInputAction.action?.Disable();
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Returning to the main menu");
        SceneManager.LoadScene(mainMenuSceneName);
    }

    //UI Button Handler for Player Vs AI
    public void SelectPlayerVsAI()
    {
        //Spawn fighters
        SpawnFighters(GameMode.PlayerVsAI);
        CloseCharacterSelectionPanel();
    }

    //UI Button Handler for AI vs AI
    public void SelectAIVsAI()
    {
        SpawnFighters(GameMode.AIvsAI);
        CloseCharacterSelectionPanel();
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
    }



    // Setting the character refrences for the health bars
    private void UpdateHealthBarUI()
    {
        if (healthBarUI == null || fighter1Instance == null || fighter2Instance == null || gameHUDPanel == null) return;

        gameHUDPanel.SetActive(true);
        healthBarUI.fighter1Character = fighter1Instance.GetComponent<BaseCharacter>();
        healthBarUI.fighter2Character = fighter2Instance.GetComponent<BaseCharacter>();

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
            fighter1Instance = Instantiate(aiFighter1Prefab, spawnPoint1.position, spawnPoint1.rotation);
            fighter2Instance = Instantiate(aiFighter2Prefab, spawnPoint2.position, spawnPoint2.rotation);
        }

        MatchData.IsMatchInProgess = true;
        MatchData.SelectedGameMode = mode;

        InitializeFighterConnections();
        UpdateCinemachineTargetGroup();
        UpdateHealthBarUI();

       
    }
    public void CloseCharacterSelectionPanel()
    {
        if (characterSelectionPanel != null)
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

}
