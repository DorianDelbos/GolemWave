namespace GolemWave
{
    public interface IDamageable
    {
        public int Health { get; }

        void TakeDamage(int damages);
        void Death();
    }
}
