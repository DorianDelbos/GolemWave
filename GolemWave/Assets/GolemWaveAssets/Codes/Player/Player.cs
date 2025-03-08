using UnityEngine;

namespace GolemWave
{
    public partial class Player : MonoBehaviour, IDamageable
    {
        private void Awake()
        {
            InitializeHealth();
            InitializeInputs();
            InitializeMovement();
        }

        private void OnEnable()
        {
            EnableInputs();
        }

        private void OnDisable()
        {
            DisableInputs();
        }

        private void Start()
        {

        }

        private void Update()
        {
            UpdateMovement();
            UpdateLaser();
        }

        private void FixedUpdate()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            GravityHandle(other);
        }

        private void OnTriggerExit(Collider other)
        {
            GravityZoneExit(other);
        }
    }
}
