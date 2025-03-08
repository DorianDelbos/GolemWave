using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GolemWave
{
    public partial class Player
    {
        [Header("Movement")]
        [SerializeField] private float speed = 5.0f;
        [SerializeField] private float jumpForce = 12.0f;
        [SerializeField] private float rotationSpeed = 10.0f;
        [SerializeField] private Transform lookAtTarget;

        private Rigidbody rb;
        private Vector2 playerDirectionInput;
        private Vector3 centerOfGravity;

        private void InitializeMovement()
        {
            rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            Cursor.lockState = CursorLockMode.Locked;
        }

        void UpdateMovement()
        {
            Vector3 gravityDirection = Vector3.down;
            if (centerOfGravity != Vector3.zero)
            {
                gravityDirection = (centerOfGravity - transform.position).normalized;
            }
            Vector3 gravityUp = -gravityDirection;

            lookAtTarget.transform.up = Vector3.Lerp(lookAtTarget.transform.up, gravityUp, 0.02f * Time.deltaTime);

            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;

            //if (Mathf.Abs(Vector3.Dot(cameraForward, gravityUp)) > 0.9f)
            //{
            //    cameraForward = Vector3.Cross(gravityUp, cameraRight).normalized;
            //}

            cameraForward = Vector3.ProjectOnPlane(cameraForward, gravityUp).normalized;
            cameraRight = Vector3.ProjectOnPlane(cameraRight, gravityUp).normalized;

            Vector3 moveDirection = (cameraForward * playerDirectionInput.y) + (cameraRight * playerDirectionInput.x);

            if (moveDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection, -gravityDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            rb.MovePosition(rb.position + moveDirection * speed * Time.deltaTime);
            rb.AddForce(gravityDirection * 9.81f, ForceMode.Force);
        }

        private void GravityHandle(Collider other)
        {
            if (!other.CompareTag("GravityZone")) return;
            centerOfGravity = other.transform.parent.position;
        }

        void ReadMovement(InputAction.CallbackContext ctx)
        {
            playerDirectionInput = ctx.ReadValue<Vector2>();
        }

        void ReadJump(InputAction.CallbackContext ctx)
        {
            if (!Physics.Raycast(transform.position, Vector3.down, 0.5f)) return;
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }
}
