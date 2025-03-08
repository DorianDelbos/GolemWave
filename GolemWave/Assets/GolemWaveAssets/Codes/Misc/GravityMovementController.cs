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
        private float gravityForce = 3.0f;

        public float Speed { get => speed; set => speed = value; }
        public Vector3 CenterOfGravity { get; private set; }
        public Vector3 GravityDireciton { get; private set; }
        public Vector3 MovementDirection { get; set; }

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
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
            currentGravityZone.gameObject.SetActive(false);
            CenterOfGravity = other.transform.position;
        }

        public void ApplyMovement()
        {
            CalculateGravityDirection();
            ApplyGravity();
            HandleMovement();
            UpdatePlayerRotation();
        }

        private void CalculateGravityDirection()
        {
            if (CenterOfGravity == Vector3.zero)
            {
                GravityDireciton = Vector3.down;
                return;
            }

            Vector3 gravityDirectionToCenter = CenterOfGravity - transform.position;
            if (Physics.Raycast(transform.position, gravityDirectionToCenter.normalized, out RaycastHit hit, gravityDirectionToCenter.magnitude))
            {
                GravityDireciton = -hit.normal;
            }
            else
            {
                GravityDireciton = gravityDirectionToCenter.normalized;
            }
        }

        private void ApplyGravity()
        {
            rb.AddForce(GravityDireciton.normalized * gravityForce, ForceMode.Force);
        }

        private void HandleMovement()
        {
            Vector3 gravityUp = -GravityDireciton;
            AdjustPlayerOrientation(gravityUp);

            Vector3 moveDirection = CalculateMoveDirection(gravityUp);
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

        private void RotateTowardsMovement(Vector3 moveDirection)
        {
            moveDirection = Vector3.ProjectOnPlane(moveDirection, GravityDireciton).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, -GravityDireciton);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        private void UpdatePlayerRotation()
        {
            if (GravityDireciton == Vector3.down) return;

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
