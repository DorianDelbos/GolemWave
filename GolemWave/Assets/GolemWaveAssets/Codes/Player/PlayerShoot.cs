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
                headTransform.localRotation = Quaternion.Lerp(headTransform.localRotation, Quaternion.identity, 20f * Time.deltaTime);
                laserTransform.gameObject.SetActive(false);
                return;
            }

            laserTransform.gameObject.SetActive(true);

            Vector3 shootDirection = GetHeadDirection();

            shootDirection.Normalize();
            laserTransform.transform.forward = shootDirection;

            Vector3 gravityDirection = centerOfGravity == Vector3.zero ? Vector3.down : (centerOfGravity - transform.position).normalized;
            Vector3 upDirection = -gravityDirection;

            Vector3 projectedShootDirection = Vector3.ProjectOnPlane(shootDirection, upDirection).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(projectedShootDirection, upDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);

            RotateHead(shootDirection);
        }

        private void RotateHead(Vector3 direction)
        {
            if (direction == Vector3.zero) return;

            Quaternion targetRotation = Quaternion.LookRotation(direction, transform.up) * Quaternion.Euler(0, -90, 0);
            headTransform.rotation = Quaternion.Slerp(headTransform.rotation, targetRotation, 10f * Time.deltaTime);
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