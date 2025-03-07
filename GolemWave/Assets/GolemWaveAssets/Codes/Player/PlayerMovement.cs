using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    InputSystem_Actions playerControls;

    Vector2 playerDirectionInput;

    Rigidbody rb;

    Vector3 centerOfGravity;

    private void Awake()
    {
        playerControls = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerControls.Player.Move.started += OnMovementAction;
        playerControls.Player.Move.performed += OnMovementAction;
        playerControls.Player.Move.canceled += OnMovementAction;
    }

    // Update is called once per frame
    void Update()
    {
        //rb.linearVelocity = new Vector3(playerDirectionInput.x, 0, playerDirectionInput.y) * 20f;

        Vector3 playerVelocity = new Vector3(playerDirectionInput.x, 0, playerDirectionInput.y).normalized;

        //Vector3 cameraRelativeMovement = ConvertToCameraSpace(playerVelocity);
        //rb.linearVelocity = playerVelocity.normalized * 30f;

        //Vector3 gravity = Vector3.down;

        //if (centerOfGravity != Vector3.zero)
        //{
        //    gravity = centerOfGravity - transform.position;
        //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.forward, -gravity.normalized), 20f * Time.deltaTime);
        //}

        //rb.linearVelocity += gravity.normalized;

        //HandleRotation(cameraRelativeMovement);

        rb.Move(rb.transform.position + playerVelocity * 20f * Time.deltaTime, Quaternion.identity);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void OnMovementAction(InputAction.CallbackContext ctx)
    {
        playerDirectionInput = playerControls.Player.Move.ReadValue<Vector2>();
    }

    void HandleRotation(Vector3 _cameraRelativeMovement)
    {
        Vector3 formattedVelocity = _cameraRelativeMovement;
        formattedVelocity.y = 0;
        formattedVelocity.Normalize();

        if (formattedVelocity.magnitude > 0)
        {
            Quaternion lookRotation = Quaternion.LookRotation(formattedVelocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }

    private Vector3 ConvertToCameraSpace(Vector3 _vectorToRotate)
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 cameraForwardZProduct = _vectorToRotate.z * cameraForward;
        Vector3 cameraRightZProduct = _vectorToRotate.x * cameraRight;

        return cameraForwardZProduct + cameraRightZProduct;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        centerOfGravity = other.transform.parent.transform.position;
    }
}
