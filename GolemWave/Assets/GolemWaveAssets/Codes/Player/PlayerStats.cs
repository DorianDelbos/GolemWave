using UnityEngine;

public partial class Player
{
    [SerializeField] int hp;

    public bool TakeDamage()
    {
        hp -= 2;

        if (hp <= 0)
        {
            Death();
            return false;
        }
        return true;
    }

    public void Death()
    {

    }
}
