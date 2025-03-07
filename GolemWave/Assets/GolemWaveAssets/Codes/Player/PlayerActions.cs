using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    InputSystem_Actions playerControls;
    private bool isShooting = false; // Variable pour d�tecter si le bouton est maintenu

    private void Awake()
    {
        playerControls = new InputSystem_Actions();
    }

    void Start()
    {
        playerControls.Player.Attack.started += ctx => isShooting = true; // D�tecte l'appui
        playerControls.Player.Attack.canceled += ctx => isShooting = false; // D�tecte le rel�chement
    }

    void Update()
    {
        if (isShooting)
        {
            Debug.Log("shoot");

            Vector3 shootDirection = Camera.main.transform.forward;

            Vector3 newForward = Vector3.Lerp(transform.forward, shootDirection, 20f * Time.deltaTime);
            newForward.y = transform.forward.y;
            transform.forward = newForward;

        }
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
