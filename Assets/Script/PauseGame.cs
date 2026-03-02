using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseCanvas;
    private bool isPaused = false;
    private bool isCursorHidden = false; // Trạng thái ẩn chuột

    void Start()
    {
        // Đảm bảo canvas tắt khi bắt đầu
        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);
        
        // Bắt đầu game với chuột hiện
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        isCursorHidden = false;
        
        isPaused = false;
    }

    void Update()
    {
        // Xử lý ẩn chuột khi click vào màn hình game
        if (!isPaused && Input.GetMouseButtonDown(0)) // Click chuột trái
        {
            // Kiểm tra xem có click vào UI không
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                // Click vào màn hình game thì ẩn chuột
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                isCursorHidden = true;
                Debug.Log("🖱️ Ẩn chuột - Click vào game");
            }
        }

        // Nhấn ESC để pause hoặc hiện chuột
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                // Khi pause game, hiện chuột
                ShowCursor();
                PauseGameMethod();
            }
        }
    }

    void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        isCursorHidden = false;
        Debug.Log("🖱️ Hiện chuột");
    }

    void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isCursorHidden = true;
        Debug.Log("🖱️ Ẩn chuột");
    }

    public void PauseGameMethod()
    {
        isPaused = true;
        
        // Hiện pause canvas
        if (pauseCanvas != null)
            pauseCanvas.SetActive(true);
        
        // Dừng thời gian game
        Time.timeScale = 0f;
        
        // Chuột đã được hiện trước khi pause
        Debug.Log("⏸️ Game Paused");
    }

    public void ResumeGame()
    {
        isPaused = false;
        
        // Tắt pause canvas
        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);
        
        // Tiếp tục thời gian game
        Time.timeScale = 1f;
        
        // Khôi phục trạng thái chuột trước khi pause
        if (isCursorHidden)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
        Debug.Log("▶️ Game Resumed");
    }
// Hàm cho nút Resume trong UI
    public void OnResumeButton()
    {
        ResumeGame();
    }

    // Hàm cho nút Restart
    public void OnRestartButton()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Hàm cho nút Quit
    public void OnQuitButton()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}