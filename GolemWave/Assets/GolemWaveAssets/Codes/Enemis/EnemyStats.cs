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

    float lowerOpacityTimer = 2f;
    bool dimmed = false;

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
        if (!healthBarCanvas.gameObject.activeInHierarchy) return;

        healthBarCanvas.transform.rotation = Quaternion.LookRotation(healthBarCanvas.transform.position - cam.transform.position);
        healthBarForeground.fillAmount = Mathf.MoveTowards(healthBarForeground.fillAmount, targetHP, reduceSpeed * Time.deltaTime);

        lowerOpacityTimer -= Time.deltaTime;

        if (lowerOpacityTimer < 0 && !dimmed)
        {
            lowerOpacityTimer = 0;
            Image[] images = healthBarCanvas.GetComponentsInChildren<Image>();

            foreach (Image image in images)
            {
                Color color = image.color;
                color.a = Mathf.Lerp(color.a, 0.5f, 6f * Time.deltaTime);
                image.color = color;

                if (color.a <= 0.5f)
                    dimmed = true;
            }
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public bool TakeDamage()
    {
        if (!healthBarCanvas.gameObject.activeInHierarchy)
        {
            healthBarCanvas.gameObject.SetActive(true);
        }
        else
        {
            Image[] images = healthBarCanvas.GetComponentsInChildren<Image>();

            foreach (Image image in images)
            {
                Color color = image.color;
                color.a = 1f;
                image.color = color;
            }

            dimmed = false;
            lowerOpacityTimer = 2f;
        }

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