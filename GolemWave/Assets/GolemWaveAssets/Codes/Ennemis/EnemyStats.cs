using UnityEngine;

namespace GolemWave
{
    public partial class Enemy
    {
        [Header("Health")]
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private int initialHealth;
        [SerializeField] private TextHolder textHolderPrefab;
        private HealthComponent healthComponent;

        public int Health { get => healthComponent.Health; set => healthComponent.Health = value; }

        private void InitializeHealth()
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

        public void Death()
        {
            Destroy(gameObject);
        }

        public void TakeDamage(int damages, Vector3 position)
        {
            Health -= damages;
            TextHolder instance = Instantiate(textHolderPrefab, position, Quaternion.identity);
            instance.Text = damages.ToString();
            instance.Size *= Random.Range(1.0f, 2.0f);
            if (healthComponent.IsDead) Death();
        }
    }
}
