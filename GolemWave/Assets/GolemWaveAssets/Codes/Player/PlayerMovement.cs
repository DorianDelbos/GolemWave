using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    InputSystem_Actions playerControls;

    [SerializeField] float speed = 5f;
    [SerializeField] float rotationSpeed = 10f;

    [SerializeField] Transform lookAtTarget;
    [SerializeField] PlayerActions playerActions;

    Vector2 playerDirectionInput;
    Rigidbody rb;

    Vector3 centerOfGravity;

    private void Awake()
    {
        playerControls = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Start()
    {
        playerControls.Player.Move.started += OnMovementAction;
        playerControls.Player.Move.performed += OnMovementAction;
        playerControls.Player.Move.canceled += OnMovementAction;

        playerControls.Player.Jump.started += OnJumpAction;

        playerActions = GetComponent<PlayerActions>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
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

        rb.linearVelocity = gravityDirection * 9.81f;
    }


    private void OnEnable() => playerControls.Enable();
    private void OnDisable() => playerControls.Disable();

    void OnMovementAction(InputAction.CallbackContext ctx)
    {
        playerDirectionInput = playerControls.Player.Move.ReadValue<Vector2>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GravityZone"))
        {
            centerOfGravity = other.transform.parent.position;
        }
    }

    void OnJumpAction(InputAction.CallbackContext ctx)
    {
        rb.AddForce(transform.up * 4f, ForceMode.Impulse);
    }
}
