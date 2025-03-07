using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    InputSystem_Actions playerControls;
    public bool IsShooting { get; private set; }

    [SerializeField] Transform laserSpawn;
    [SerializeField] GameObject laserPf;

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
            laser = Instantiate(laserPf, laserSpawn.position, Quaternion.identity);
            laser.transform.parent = transform;
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
            laser.transform.forward = shootDirection;

            Vector3 newForward = Vector3.Lerp(transform.forward, shootDirection, 20f * Time.deltaTime);
            newForward.y = transform.forward.y;
            transform.forward = newForward;
        }
    }

    Vector3 GetShootDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // Centre de l'écran
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return (hit.point - laserSpawn.position).normalized; // Direction vers l'impact
        }
        else
        {
            return Camera.main.transform.forward; // Direction de la caméra si rien n'est touché
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
