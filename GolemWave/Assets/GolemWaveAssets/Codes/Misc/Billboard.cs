using UnityEngine;

namespace GolemWave
{
    [ExecuteInEditMode]
    public class Billboard : MonoBehaviour
    {
        private Camera mainCamera;
        [SerializeField] private bool invertDirection = false;

        void Update()
        {
            mainCamera ??= Camera.main;
            if (mainCamera == null) return;

            Vector3 direction = mainCamera.transform.forward;
            transform.forward = invertDirection ? -direction : direction;
        }
    }

}