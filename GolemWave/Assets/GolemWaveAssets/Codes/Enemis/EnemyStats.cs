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

        public void TakeDamage(int damages)
        {
            //Instantiate(damageCanvasPf, damageCanvasSpawn.position, Quaternion.identity, damageCanvasSpawn);

            Health -= damages;
            Vector3 positionDamageText = transform.position + Vector3.up + Random.insideUnitSphere * Random.Range(0.0f, 2.0f);
            TextHolder instance = Instantiate(textHolderPrefab, positionDamageText, Quaternion.identity);
            instance.Text = damages.ToString();
            instance.Size *= Random.Range(1.0f, 5.0f);
            if (healthComponent.IsDead) Death();
        }
    }
}
