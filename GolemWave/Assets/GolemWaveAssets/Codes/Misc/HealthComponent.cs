using System;
using UnityEngine;

namespace GolemWave
{
    public class HealthComponent
    {
        private int health;
        public int Health
        {
            get => health;
            set
            {
                health = Mathf.Clamp(value, 0, maxHealth);
                onHealthChanged?.Invoke(health, maxHealth);
            }
        }

        private int maxHealth;
        public int MaxHealth
        {
            get => maxHealth;
            set
            {
                maxHealth = Mathf.Max(1, value);
                Health = Mathf.Clamp(Health, 0, maxHealth);
            }
        }

        public float Factor => Health / MaxHealth;
        public bool IsDead => Health <= 0;

        public event Action<int, int> onHealthChanged;

        public HealthComponent(int initialMaxHealth)
        {
            MaxHealth = initialMaxHealth;
            Health = maxHealth;
        }
    }
}
