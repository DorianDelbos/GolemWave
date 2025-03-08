using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour, IDamageable
{
    [SerializeField] float hp;
    float maxHealth;

    [SerializeField] Image healthBarForeground;
    [SerializeField] Canvas healthBarCanvas;

    float reduceSpeed = 1f;
    float targetHP = 1;

    Camera cam;
    private void Awake()
    {
        maxHealth = hp;

        if (!healthBarForeground) Debug.LogError("incorrect ennemy");
    }

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        healthBarCanvas.transform.rotation = Quaternion.LookRotation(healthBarCanvas.transform.position - cam.transform.position);

        healthBarForeground.fillAmount = Mathf.MoveTowards(healthBarForeground.fillAmount, targetHP, reduceSpeed * Time.deltaTime);
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public bool TakeDamage()
    {
        hp -= 5;

        targetHP = hp / maxHealth;

        if (hp <= 0)
        {
            Die();
            return false;
        }
        return true;
    }
}