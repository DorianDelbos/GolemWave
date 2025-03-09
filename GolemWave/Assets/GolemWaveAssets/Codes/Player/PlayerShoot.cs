using UnityEngine;
using UnityEngine.InputSystem;

namespace GolemWave
{
    public partial class Player
    {
        [Header("Attack")]
        [SerializeField] private Transform laserTransform;
        [SerializeField] private Transform headTransform;
        [SerializeField] private int damages;
        private bool isShooting;

        float localScaleZ;

        private void ReadShoot(InputAction.CallbackContext ctx)
        {
            isShooting = ctx.ReadValueAsButton() && Time.timeScale > 0;
        }

        public void DealDamage(IDamageable damageable)
        {
            while (damageable.Health <= 0) return;

            healthComponent.Health += 1;
            Vector3 impactPoint = (damageable as MonoBehaviour).GetComponent<Collider>().ClosestPoint(transform.position);
            damageable.TakeDamage(damages, impactPoint);
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
            laserTransform.forward = shootDirection;
            laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, localScaleZ);

            Vector3 gravityDirection = controller.CenterOfGravity == Vector3.zero ? Vector3.down : (controller.CenterOfGravity - transform.position).normalized;
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
            // Essai de raycast vers le curseur
            if (TryGetRaycastHit(out Vector3 impactPoint))
            {
                return (impactPoint - laserTransform.position).normalized;
            }

            // Si pas d'impact, projection sur un plan devant le joueur
            Plane plane = new Plane(-Camera.main.transform.forward, transform.position + Camera.main.transform.forward * 10f);
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (plane.Raycast(ray, out float distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                return (hitPoint - laserTransform.position).normalized;
            }

            return Camera.main.transform.forward; // Fallback si tout échoue
        }

        private bool TryGetRaycastHit(out Vector3 impactPoint)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 10f + (Camera.main.transform.position - transform.position).magnitude))
            {
                impactPoint = hit.point;
                localScaleZ = (hit.point - laserTransform.position).magnitude / 10f;
                return true;
            }

            localScaleZ = 1;
            impactPoint = Vector3.zero;
            return false;
        }
    }
}