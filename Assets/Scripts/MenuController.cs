using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private string gameplaySceneName = "Level1";

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }


    public void StartGame()
    {
        Debug.Log("Starting game");
        SceneManager.LoadScene(gameplaySceneName);
    }


    public void ExitGame()
    {
        Debug.Log("Exiting the application");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
