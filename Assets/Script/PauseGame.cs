using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseCanvas;
    bool isPaused = false;

    void Start()
    {
        pauseCanvas.SetActive(false);
        // Không cần quản lý cursor gì cả
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            pauseCanvas.SetActive(isPaused);
            Time.timeScale = isPaused ? 0f : 1f;
        }
    }
    
    public void ResumeGame()
    {
        isPaused = false;
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}