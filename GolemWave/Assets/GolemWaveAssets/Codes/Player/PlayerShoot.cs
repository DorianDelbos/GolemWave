using UnityEngine;
using UnityEngine.InputSystem;

namespace GolemWave
{
    public partial class Player
    {
        [Header("Attack")]
        [SerializeField] private Transform laserTransform;
        [SerializeField] private Transform headTransform;
        private bool isShooting;

        private void ReadShoot(InputAction.CallbackContext ctx)
        {
            isShooting = ctx.ReadValueAsButton();
        }

        private void UpdateLaser()
        {
            if (!isShooting)
            {
                RotateHead(transform.forward);
                laserTransform.gameObject.SetActive(false);
                return;
            }

            laserTransform.gameObject.SetActive(true);

            Vector3 shootDirection = GetHeadDirection();

            shootDirection.Normalize();
            laserTransform.transform.forward = shootDirection;

            Vector3 newForward = Vector3.Lerp(transform.forward, shootDirection, 20f * Time.deltaTime);
            newForward.y = transform.forward.y;
            transform.forward = newForward;

            RotateHead(shootDirection);
        }

        private void RotateHead(Vector3 rotation)
        {
            Quaternion result = Quaternion.LookRotation(rotation, Vector3.up) * Quaternion.Euler(0, -90, 0);
            headTransform.rotation = Quaternion.Lerp(headTransform.rotation, result, 20f * Time.deltaTime);
        }

        private Vector3 GetHeadDirection()
        {
            Plane plane = new Plane(-Camera.main.transform.forward, transform.position + Camera.main.transform.forward * 6);

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

            float distance;
            if (plane.Raycast(ray, out distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                return (hitPoint - laserTransform.transform.position).normalized;
            }

            return Camera.main.transform.forward;
        }
    }
}