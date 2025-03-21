using UnityEngine;

namespace GolemWave
{
    public partial class Player : MonoBehaviour, IDamageable, IAttacker
    {
        [SerializeField] private PauseMenu pauseMenu;

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
