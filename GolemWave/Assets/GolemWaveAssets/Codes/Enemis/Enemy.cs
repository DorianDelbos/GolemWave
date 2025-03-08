using UnityEngine;

namespace GolemWave
{
    public partial class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private Transform laserTransform;
        [SerializeField] private Transform headTransform;
        private Transform player;
        private Rigidbody rb;
        private Vector3 targetPoint;
        private float newTargetTimer;
        public bool UpdateBool { get; private set; } = false;
        public Transform Player { get => player; private set => player = value; }

        private GravityMovementController controller;

        private void Awake()
        {
            InitializeHealth();
            rb = GetComponent<Rigidbody>();
            controller = GetComponent<GravityMovementController>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            player = GameObject.FindWithTag("Player").transform;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 posToPlayer = player.transform.position - transform.position;
            UpdateBool = posToPlayer.sqrMagnitude <= 10 * 10 && posToPlayer.sqrMagnitude >= 3 * 3;

            controller.ApplyMovement();

            newTargetTimer -= Time.deltaTime;

            if (posToPlayer.sqrMagnitude <= 3 * 3)
            {
                UpdateBool = false;
                laserTransform.gameObject.SetActive(true);

                if (newTargetTimer <= 0)
                {
                    GenerateTarget();
                }

                headTransform.rotation = Quaternion.Lerp(headTransform.rotation, Quaternion.LookRotation(targetPoint - transform.position, transform.up) * Quaternion.Euler(0, -90, 0), 4f * Time.deltaTime);
            }
            else
            {
                if (newTargetTimer <= 0)
                    laserTransform.gameObject.SetActive(false);
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
}
