using UnityEngine;
using UnityEngine.InputSystem;

namespace GolemWave
{
    public partial class Player
    {
        [Header("Movement")]
        [SerializeField] private float jumpForce = 12.0f;

        [SerializeField] Transform bodyTransform;
        private Rigidbody rb;
        private GravityMovementController controller;
        private Vector2 playerDirectionInput;

        float initialSpeed;
        float speedCoeff = 1f;
        Quaternion targetRunRot = Quaternion.identity;

        private void InitializeMovement()
        {
            rb = GetComponent<Rigidbody>();
            controller = GetComponent<GravityMovementController>();
            initialSpeed = controller.Speed;

            rb.constraints = RigidbodyConstraints.FreezeRotation;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void UpdateMovement()
        {
            controller.Speed = initialSpeed * speedCoeff;
            controller.MovementDirection = playerDirectionInput;
            controller.ApplyMovement();

            bodyTransform.localRotation = Quaternion.Lerp(bodyTransform.localRotation, targetRunRot, 10f * Time.fixedDeltaTime);
        }

        private void ReadMovement(InputAction.CallbackContext ctx)
        {
            playerDirectionInput = ctx.ReadValue<Vector2>();
        }

        private void ReadJump(InputAction.CallbackContext ctx)
        {
            if (!Physics.Raycast(transform.position, -transform.up, out RaycastHit _, 0.5f)) return;
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            Debug.Log("allo");
        }

        private void StartRun(InputAction.CallbackContext ctx)
        {
            targetRunRot = Quaternion.Euler(bodyTransform.localRotation.x, bodyTransform.localRotation.y, -30f);
            speedCoeff = 1.3f;
        }

        private void EndRun(InputAction.CallbackContext ctx)
        {
            targetRunRot = Quaternion.identity;
            speedCoeff = 1f;
        }
    }
}
