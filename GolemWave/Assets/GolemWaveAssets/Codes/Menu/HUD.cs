using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GolemWave
{
    public class HUD : MonoBehaviour
    {
        public static HUD Instance;
        [SerializeField] private Image healthBar;
        [SerializeField] private TMP_Text healthTextPercent;

        private void Awake()
        {
            Instance = this;
        }

        public void OnHealthChanged(int value, int maxValue)
        {
            float factor = Mathf.Clamp01(value / (float)maxValue);
            healthBar.fillAmount = factor;
            healthTextPercent.text = $"{factor * 100.0f}%";
        }
    }
}
