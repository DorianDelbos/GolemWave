using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [SerializeField] int hp;

    public bool TakeDamage()
    {
        hp -= 2;

        if (hp <= 0)
        {
            Die();
            return false;
        }
        return true;
    }

    public void Die()
    {

    }
}
