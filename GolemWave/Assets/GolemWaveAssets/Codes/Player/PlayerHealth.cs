using UnityEngine;
using UnityEngine.SceneManagement;

namespace GolemWave
{
    public partial class Player
    {
        [Header("Health")]
        [SerializeField] private int initialHealth;
        [SerializeField] private HUD hud;
        private HealthComponent healthComponent;

        public int Health { get => healthComponent.Health; set => healthComponent.Health = value; }

        public void InitializeHealth()
        {
            healthComponent = new HealthComponent(initialHealth);
            healthComponent.onHealthChanged += hud.OnHealthChanged;
        }

        public void TakeDamage(int damages, Vector3 position)
        {
            Health -= damages;
            if (healthComponent.IsDead) Death();
        }

        public void Death()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}