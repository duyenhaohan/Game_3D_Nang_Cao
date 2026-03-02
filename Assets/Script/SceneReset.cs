using UnityEngine;

public class SceneReset : MonoBehaviour
{
    void Awake()
    {
        Time.timeScale = 1f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("Scene Reset Done");
    }
}