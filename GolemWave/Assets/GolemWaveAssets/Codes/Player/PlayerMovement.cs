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
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Start()
    {
        playerControls.Player.Move.started += OnMovementAction;
        playerControls.Player.Move.performed += OnMovementAction;
        playerControls.Player.Move.canceled += OnMovementAction;

        playerActions = GetComponent<PlayerActions>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        lookAtTarget.transform.rotation = Quaternion.identity;

        Vector3 gravityDirection = Vector3.down;

        Vector3 moveDirection = ConvertToCameraSpace(new Vector3(playerDirectionInput.x, 0, playerDirectionInput.y));
        moveDirection = Vector3.ProjectOnPlane(moveDirection, gravityDirection).normalized;

        if (!playerActions.IsShooting)
        {
            if (moveDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection, -gravityDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
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

    private Vector3 ConvertToCameraSpace(Vector3 inputVector)
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        return cameraForward * inputVector.z + cameraRight * inputVector.x;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GravityZone"))
        {
            centerOfGravity = other.transform.parent.position;
        }
    }
}
