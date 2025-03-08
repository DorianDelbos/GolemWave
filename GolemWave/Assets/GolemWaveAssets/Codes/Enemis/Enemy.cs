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
            controller.ApplyMovement();

            if (Vector3.SqrMagnitude(transform.position - player.position) > 10 * 10) return;

            Vector3 posToPlayer = player.transform.position - transform.position;

            newTargetTimer -= Time.deltaTime;

            if (posToPlayer.sqrMagnitude <= 3 * 3)
            {
                laserTransform.gameObject.SetActive(true);

                if (newTargetTimer <= 0)
                {
                    GenerateTarget();
                }

                laserTransform.rotation = Quaternion.Lerp(laserTransform.rotation, Quaternion.LookRotation(targetPoint - transform.position), 4f * Time.deltaTime);

                Quaternion newHeadRot = Quaternion.LookRotation(targetPoint - transform.position) * Quaternion.Euler(0, -90, 0);
                headTransform.rotation = Quaternion.Lerp(headTransform.rotation, newHeadRot, 20f * Time.deltaTime);
            }
            else
            {
                if (newTargetTimer <= 0)
                    laserTransform.gameObject.SetActive(false);

                //rb.MovePosition(transform.position + posToPlayer.normalized * 20f * Time.deltaTime);
                headTransform.localRotation = Quaternion.Lerp(headTransform.localRotation, Quaternion.identity, 20f * Time.deltaTime);
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
