using UnityEngine;

namespace GolemWave
{
    public partial class Enemy
    {
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private int initialHealth;
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
            if (healthComponent.IsDead) Death();
        }
    }
}
