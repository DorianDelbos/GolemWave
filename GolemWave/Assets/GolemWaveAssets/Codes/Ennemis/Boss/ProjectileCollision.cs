using GolemWave;
using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    Rigidbody rb;
    float ignoreCollisionsTimer = 0f;

    public void Initialize(Vector3 targetPosition)
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<Collider>().enabled = false;
        LaunchProjectile(targetPosition);
    }

    void LaunchProjectile(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        float height = 10f;
        float gravity = Physics.gravity.y;

        float time = Mathf.Sqrt(-2 * height / gravity) + Mathf.Sqrt(2 * (direction.y - height) / gravity);

        Vector3 horizontalDirection = new Vector3(direction.x, 0, direction.z);
        Vector3 velocity = horizontalDirection / time;
        velocity.y = Mathf.Sqrt(-2 * gravity * height);

        rb.linearVelocity = velocity;
    }

    private void Update()
    {
        ignoreCollisionsTimer += Time.deltaTime;
        if (ignoreCollisionsTimer > 1f)
        {
            GetComponent<Collider>().enabled = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (ignoreCollisionsTimer <= 1f) return;
        if (collision.transform.CompareTag("Projectile")) return;

        IDamageable damageable = collision.transform.GetComponent<IDamageable>();
        if (damageable != null)
        {
            Vector3 impactPoint = (damageable as MonoBehaviour).GetComponent<Collider>().ClosestPoint(transform.position);
            damageable.TakeDamage(5, impactPoint);
        }

        Destroy(gameObject, 5f);
    }
}
