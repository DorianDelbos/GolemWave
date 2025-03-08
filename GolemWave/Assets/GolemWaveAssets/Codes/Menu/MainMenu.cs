#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GolemWave
{
    public class MainMenu : MonoBehaviour
    {
        public void OpenBossFightScene(int bossIndex)
        {
            SceneManager.LoadScene($"Boss{bossIndex}");
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
}
