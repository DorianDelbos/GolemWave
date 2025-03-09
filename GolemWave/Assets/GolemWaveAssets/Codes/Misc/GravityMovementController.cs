using System.Collections;
using UnityEngine;

namespace GolemWave
{
    public class GravityMovementController : MonoBehaviour
    {
        [SerializeField] private float speed = 5.0f;
        [SerializeField] private float rotationSpeed = 10.0f;
        [SerializeField] private Transform lookAtTarget;

        private Rigidbody rb;
        private Transform currentGravityZone = null;
        private float gravityForce = 9.81f;

        public float Speed { get => speed; set => speed = value; }
        public Vector3 CenterOfGravity { get; private set; }
        public Vector3 GravityDirection { get; private set; }
        public Vector3 MovementDirection { get; set; }

        private Enemy enemyScript;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            enemyScript = GetComponent<Enemy>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("GravityZone")) return;

            if (currentGravityZone)
            {
                GameObject previousZone = currentGravityZone.gameObject;
                StartCoroutine(ReactivateGravityZone(previousZone));
            }

            currentGravityZone = other.transform;
            CenterOfGravity = other.transform.position;

            if (transform.CompareTag("Player"))
                currentGravityZone.gameObject.SetActive(false);
        }

        public void ApplyMovement()
        {
            CalculateGravityDirection();
            if (CompareTag("Player"))
                HandleMovement();
            else HandleEnemyMovement(enemyScript.UpdateBool ? enemyScript.Player.position : rb.transform.position);
            ApplyGravity();
            UpdatePlayerRotation();
        }

        private void CalculateGravityDirection()
        {
            if (CenterOfGravity == Vector3.zero)
            {
                GravityDirection = Vector3.down;
                return;
            }

            Vector3 gravityDirectionToCenter = CenterOfGravity - transform.position;
            if (Physics.Raycast(transform.position, gravityDirectionToCenter.normalized, out RaycastHit hit, gravityDirectionToCenter.magnitude))
            {
                GravityDirection = -hit.normal;
            }
            else
            {
                GravityDirection = gravityDirectionToCenter.normalized;
            }
        }

        private void ApplyGravity()
        {
            rb.AddForce(GravityDirection.normalized * gravityForce * 100.0f * Time.deltaTime, ForceMode.Force);
        }

        private void HandleMovement()
        {
            Vector3 gravityUp = -GravityDirection;
            AdjustPlayerOrientation(gravityUp);

            Vector3 moveDirection = CalculateMoveDirection(gravityUp);
            if (moveDirection.magnitude > 0.1f)
            {
                RotateTowardsMovement(moveDirection);
            }
            rb.MovePosition(rb.position + moveDirection * speed * Time.deltaTime);
        }

        private void HandleEnemyMovement(Vector3 target)
        {
            Vector3 gravityUp = -GravityDirection;
            AdjustPlayerOrientation(gravityUp);

            Vector3 moveDirection = CalculateEnemyMoveDirection(target);
            if (moveDirection.magnitude > 0.1f)
            {
                RotateTowardsMovement(moveDirection);
            }
            rb.MovePosition(rb.position + moveDirection * speed * Time.deltaTime);
        }

        private void AdjustPlayerOrientation(Vector3 gravityUp)
        {
            if (lookAtTarget)
                lookAtTarget.transform.up = Vector3.Lerp(lookAtTarget.transform.up, gravityUp, 5f * Time.deltaTime);
        }

        private Vector3 CalculateMoveDirection(Vector3 gravityUp)
        {
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;

            cameraForward = Vector3.ProjectOnPlane(cameraForward, gravityUp).normalized;
            cameraRight = Vector3.ProjectOnPlane(cameraRight, gravityUp).normalized;

            return (cameraForward * MovementDirection.y) + (cameraRight * MovementDirection.x);
        }

        private Vector3 CalculateEnemyMoveDirection(Vector3 target)
        {
            Vector3 posToTarget = target - rb.position;

            // A faire ça nsm
            // prendre la même direction mais aligner le vecteur sur rb.forward

            // Update ça marche???
            return posToTarget.normalized;
        }

        private void RotateTowardsMovement(Vector3 moveDirection)
        {
            moveDirection = Vector3.ProjectOnPlane(moveDirection, GravityDirection).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, -GravityDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        private void UpdatePlayerRotation()
        {
            if (GravityDirection == Vector3.down) return;

            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 5f))
            {
                Vector3 newUp = hit.normal;
                Vector3 forwardProjected = Vector3.ProjectOnPlane(transform.forward, newUp).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(forwardProjected, newUp);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        private IEnumerator ReactivateGravityZone(GameObject gravityZone)
        {
            yield return new WaitForSeconds(1f);
            gravityZone.SetActive(true);
        }
    }
}
