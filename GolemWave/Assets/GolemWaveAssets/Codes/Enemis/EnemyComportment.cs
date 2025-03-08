using UnityEngine;

public class EnemyComportment : MonoBehaviour
{
    [SerializeField] GameObject laserPf;
    [SerializeField] Transform laserSpawn;
    GameObject laser;

    Transform player;
    Rigidbody rb;

    Vector3 targetPoint;
    float newTargetTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.SqrMagnitude(transform.position - player.position) > 10 * 10) return;

        Vector3 posToPlayer = player.transform.position - transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(posToPlayer), 20f * Time.deltaTime);

        newTargetTimer -= Time.deltaTime;

        if (posToPlayer.sqrMagnitude <= 3 * 3)
        {
            if (!laser)
            {
                laser = Instantiate(laserPf, laserSpawn.position, transform.rotation, transform);
            }

            if (newTargetTimer <= 0)
            {
                GenerateTarget();
            }

            laser.transform.rotation = Quaternion.Lerp(laser.transform.rotation, Quaternion.LookRotation(targetPoint - laserSpawn.position), 4f * Time.deltaTime);
        }
        else
        {
            if (laser && newTargetTimer <= 0) Destroy(laser);

            rb.MovePosition(transform.position + posToPlayer.normalized * 20f * Time.deltaTime);
        }
    }

    void GenerateTarget()
    {
        Vector2 randomCirclePoint = Random.insideUnitCircle * 1f;
        Vector3 localPoint = new Vector3(randomCirclePoint.x, 0f, randomCirclePoint.y);
        Vector3 rightDirection = Vector3.Cross(player.forward, Vector3.up).normalized;
        Vector3 forwardDirection = Vector3.Cross(rightDirection, player.forward).normalized;

        Vector3 rotatedPoint = player.position + localPoint.x * rightDirection + localPoint.z * forwardDirection;
        rotatedPoint.y += 0.2f;

        targetPoint = rotatedPoint;

        newTargetTimer = 0.5f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, 10);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(targetPoint, 0.1f);
    }
}
