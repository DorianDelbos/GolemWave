#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.None;
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
