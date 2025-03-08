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
            UpdateLaser();
            UpdateMovement();
        }
    }
}
