using UnityEngine;
using System.Collections;

namespace GolemWave
{
    public class CrystalDamage : MonoBehaviour, IDamageable
    {
        [SerializeField] private int initialHealth;
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private TextHolder textHolderPrefab;
        private float dissolveDuration = 5.0f;
        private string cutoffProperty = "_Cutoff_Height";

        private HealthComponent healthComponent;
        private Material material;

        public int Health { get => healthComponent.Health; set => healthComponent.Health = value; }

        private void Awake()
        {
            healthComponent = new HealthComponent(initialHealth);
            Renderer renderer = GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                material = renderer.material; // Get the material instance
            }
        }

        private void OnEnable()
        {
            healthComponent.onHealthChanged += healthBar.OnHealthChanged;
        }

        private void OnDisable()
        {
            healthComponent.onHealthChanged -= healthBar.OnHealthChanged;
        }

        void IDamageable.TakeDamage(int damages, Vector3 position)
        {
            Health -= damages;
            TextHolder instance = Instantiate(textHolderPrefab, position, Quaternion.identity);
            instance.Text = damages.ToString();
            instance.Size *= Random.Range(1.0f, 5.0f);
            if (Health <= 0) Death();
        }

        public void Death()
        {
            StartCoroutine(DissolveAndDestroy());
        }

        private IEnumerator DissolveAndDestroy()
        {
            if (material == null)
            {
                Destroy(gameObject);
                yield break;
            }

            float elapsedTime = 0f;
            float startValue = material.GetFloat(cutoffProperty);
            float endValue = -5.0f; // Adjust this based on your shader's behavior

            while (elapsedTime < dissolveDuration)
            {
                elapsedTime += Time.deltaTime;
                float lerpValue = Mathf.Lerp(startValue, endValue, elapsedTime / dissolveDuration);
                material.SetFloat(cutoffProperty, lerpValue);
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}
