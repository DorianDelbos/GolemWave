using UnityEngine;
using UnityEngine.InputSystem;

namespace GolemWave
{
    public partial class Player
    {
        [Header("Movement")]
        [SerializeField] private float jumpForce = 12.0f;

        private Rigidbody rb;
        private GravityMovementController controller;
        private Vector2 playerDirectionInput;

        private void InitializeMovement()
        {
            rb = GetComponent<Rigidbody>();
            controller = GetComponent<GravityMovementController>();

            rb.constraints = RigidbodyConstraints.FreezeRotation;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void UpdateMovement()
        {
            controller.MovementDirection = playerDirectionInput;
            controller.ApplyMovement();
        }

        private void ReadMovement(InputAction.CallbackContext ctx)
        {
            playerDirectionInput = ctx.ReadValue<Vector2>();
        }

        private void ReadJump(InputAction.CallbackContext ctx)
        {
            if (!Physics.Raycast(transform.position, -transform.up, out RaycastHit _, 0.5f)) return;
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }
}
