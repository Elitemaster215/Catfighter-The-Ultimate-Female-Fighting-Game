using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenuScene";
    [SerializeField] private InputActionProperty mainMenuInputAction;
    [SerializeField] private GameObject characterSelectionPanel;
    [SerializeField] private GameObject gameHUDPanel;



    private void OnEnable()
    {
        mainMenuInputAction.action?.Enable();
    }

    private void OnDisable()
    {
        mainMenuInputAction.action?.Disable();
    }



    // Update is called once per frame
    private void Update()
    {
        if(mainMenuInputAction.action != null && mainMenuInputAction.action.WasPressedThisFrame())
        {
            ReturnToMainMenu();
        }
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Returning to the main menu");
        SceneManager.LoadScene(mainMenuSceneName);
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
