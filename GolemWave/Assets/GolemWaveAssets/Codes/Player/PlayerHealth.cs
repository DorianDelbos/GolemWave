using UnityEngine;

namespace GolemWave
{
    public partial class Player
    {
        [Header("Health")]
        [SerializeField] private int initialHealth;
        private HealthComponent healthComponent;

        public int Health { get => healthComponent.Health; set => healthComponent.Health = value; }

        public void InitializeHealth()
        {
            healthComponent = new HealthComponent(initialHealth);
        }

        public void TakeDamage(int damages, Vector3 position)
        {
            Health -= damages;
            if (healthComponent.IsDead) Death();
        }

        public void Death()
        {
            // TODO Player death
        }
    }
}