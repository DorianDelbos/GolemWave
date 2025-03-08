using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    InputSystem_Actions playerControls;
    public bool IsShooting { get; private set; }

    [SerializeField] Transform laserSpawn;
    [SerializeField] GameObject laserPf;
    [SerializeField] Transform head;

    GameObject laser;

    private void Awake()
    {
        playerControls = new InputSystem_Actions();
    }

    void Start()
    {
        playerControls.Player.Attack.started += ctx =>
        {
            IsShooting = true;
            laser = Instantiate(laserPf, laserSpawn.position, transform.rotation, transform);
        };

        playerControls.Player.Attack.canceled += ctx =>
        {
            IsShooting = false;
            if (laser != null) Destroy(laser);
        };
    }

    void Update()
    {
        if (IsShooting && laser != null)
        {
            Vector3 shootDirection = GetShootDirection();

            shootDirection.Normalize();
            laser.transform.forward = shootDirection;

            Vector3 newForward = Vector3.Lerp(transform.forward, shootDirection, 20f * Time.deltaTime);
            newForward.y = transform.forward.y;
            transform.forward = newForward;

            Quaternion newHeadRot = Quaternion.LookRotation(shootDirection, Vector3.up) * Quaternion.Euler(0, -90, 0);
            head.rotation = Quaternion.Lerp(head.rotation, newHeadRot, 20f * Time.deltaTime);
        }

        else head.localRotation = Quaternion.Lerp(head.localRotation, Quaternion.identity, 20f * Time.deltaTime);
    }

    Vector3 GetShootDirection()
    {
        Plane plane = new Plane(-Camera.main.transform.forward, transform.position + Camera.main.transform.forward * 6);

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        float distance;
        if (plane.Raycast(ray, out distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            return (hitPoint - laser.transform.position).normalized;
        }

        return Camera.main.transform.forward;
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
