using UnityEngine;

namespace GolemWave
{
    public interface IDamageable
    {
        public int Health { get; }

        void TakeDamage(int damages, Vector3 position);
        void Death();
    }
}
