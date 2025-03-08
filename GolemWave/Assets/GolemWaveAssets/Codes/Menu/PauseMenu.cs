using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused;

    public void Toggle(InputAction.CallbackContext ctx)
    {
        Toggle();
    }

    public void Toggle()
    {
        if (isPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0.0f;
        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LeaveGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
