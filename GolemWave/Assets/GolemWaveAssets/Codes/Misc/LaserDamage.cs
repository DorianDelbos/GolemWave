using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    AudioSource tickSound;

    public bool Hit { get; private set; } = false;

    float timer = 0f;
    [SerializeField] float tick;

    IDamageable damageable;

    private void Awake()
    {
        tickSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Hit)
        {
            timer += Time.deltaTime;

            if (timer >= 0.1f)
            {
                if (!damageable.TakeDamage())
                {
                    Hit = false;
                    damageable = null;
                };

                tickSound.Play();
                timer = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamageable>() != null)
        {
            Hit = true;
            damageable = other.GetComponent<IDamageable>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IDamageable>() != null)
        {
            Hit = false;
            damageable = null;
            timer = 0f;
        }
    }
}
