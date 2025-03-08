using UnityEngine;

namespace GolemWave
{
    public class CrystalDamage : MonoBehaviour, IDamageable
    {
        [SerializeField] private int initialHealth;
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private TextHolder textHolderPrefab;
        private HealthComponent healthComponent;

        public int Health { get => healthComponent.Health; set => healthComponent.Health = value; }

        private void Awake()
        {
            healthComponent = new HealthComponent(initialHealth);
        }

        private void OnEnable()
        {
            healthComponent.onHealthChanged += healthBar.OnHealthChanged;
        }

        private void OnDisable()
        {
            healthComponent.onHealthChanged -= healthBar.OnHealthChanged;
        }

        void IDamageable.TakeDamage(int damages)
        {
            Health -= damages;
            Vector3 positionDamageText = transform.position + Vector3.up * 4.0f + Random.insideUnitSphere * Random.Range(0.0f, 2.0f);
            TextHolder instance = Instantiate(textHolderPrefab, positionDamageText, Quaternion.identity);
            instance.Text = damages.ToString();
            instance.Size *= Random.Range(1.0f, 5.0f);
            if (Health <= 0) Death();
        }

        public void Death()
        {
            Destroy(gameObject);
        }
    }
}
