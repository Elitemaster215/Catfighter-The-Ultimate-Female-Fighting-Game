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


    public void PlayGame()
    {
        Debug.Log("Starting game");
        SceneManager.LoadScene(gameplaySceneName);
    }


    public void QuitGame()
    {
        Debug.Log("Exiting the application");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
