using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GolemWave
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Gradient opacityGradientTime;
        float decreaseSpeed = 0.5f;
        private float totalTime = 2.0f;
        private Coroutine fadeCoroutine;
        private Coroutine decreaseCoroutine;

        public void OnHealthChanged(int value, int maxValue)
        {
            float targetValue = (float)value / maxValue;

            if (decreaseCoroutine != null) StopCoroutine(decreaseCoroutine);
            decreaseCoroutine = StartCoroutine(DecreaseHealthRoutine(targetValue));

            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(HealthBarRoutine());
        }

        private IEnumerator DecreaseHealthRoutine(float targetValue)
        {
            while (slider.value > targetValue)
            {
                slider.value -= decreaseSpeed * Time.deltaTime;
                yield return null;
            }
            slider.value = targetValue;
            decreaseCoroutine = null;
        }

        private IEnumerator HealthBarRoutine()
        {
            float elapsedTime = 0.0f;
            while (elapsedTime < totalTime)
            {
                elapsedTime += Time.deltaTime;
                float factor = Mathf.Clamp01(elapsedTime / totalTime);
                canvasGroup.alpha = opacityGradientTime.Evaluate(factor).a;
                yield return null;
            }
            fadeCoroutine = null;
        }
    }
}