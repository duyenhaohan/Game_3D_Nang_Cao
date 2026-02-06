using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public string gameSceneName = "Player";
    public string mainMenuSceneName = "PlayMenuScene";

    public void RestartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit Game");
    }
}
