using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GolemWave
{
    public partial class Player
    {
        [Header("Movement")]
        [SerializeField] private float speed = 5.0f;
        [SerializeField] private float jumpForce = 12.0f;
        [SerializeField] private float rotationSpeed = 10.0f;
        [SerializeField] private Transform lookAtTarget;

        private Rigidbody rb;
        private Vector2 playerDirectionInput;
        private Vector3 centerOfGravity;
        Vector3 gravityDirection;

        Transform currentGravityZone = null;
        Transform lockedGravityZone = null;

        Vector3 tempGravityCenter = Vector3.zero;


        private void InitializeMovement()
        {
            rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            Cursor.lockState = CursorLockMode.Locked;
        }

        void UpdateMovement()
        {
            // Raycast pour obtenir la normale du sol sous le joueur
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, 2f))
            {
                gravityDirection = -hit.normal;
            }
            else
            {
                if (tempGravityCenter != Vector3.zero)
                {
                    gravityDirection = (transform.position - tempGravityCenter).normalized;
                }
                else gravityDirection = centerOfGravity == Vector3.zero ? Vector3.down : (centerOfGravity - transform.position).normalized;
            }

            Vector3 gravityUp = -gravityDirection;

            // Ajuste l'orientation de l'objet à la normale du sol
            lookAtTarget.transform.up = Vector3.Lerp(lookAtTarget.transform.up, gravityUp, 5f * Time.deltaTime);

            // Calcule la direction de la caméra projetée sur la surface
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;

            cameraForward = Vector3.ProjectOnPlane(cameraForward, gravityUp).normalized;
            cameraRight = Vector3.ProjectOnPlane(cameraRight, gravityUp).normalized;

            // Calcule la direction de mouvement (corrigée pour suivre la pente)
            Vector3 moveDirection = (cameraForward * playerDirectionInput.y) + (cameraRight * playerDirectionInput.x);

            if (moveDirection.magnitude > 0.1f)
            {
                moveDirection = Vector3.ProjectOnPlane(moveDirection, gravityDirection).normalized;

                Quaternion targetRotation = Quaternion.LookRotation(moveDirection, -gravityDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // Applique le mouvement corrigé
            rb.MovePosition(rb.position + moveDirection * speed * Time.deltaTime);

            // Applique la gravité corrigée
            rb.AddForce(gravityDirection.normalized * 9.81f * 0.3f, ForceMode.Force);

            UpdatePlayerRotation();

            if (!Physics.Raycast(transform.position, -transform.up, out hit, 0.5f) && tempGravityCenter != Vector3.zero) tempGravityCenter = Vector3.zero;
        }

        private void GravityHandle(Collider other) // Enter
        {
            if (!other.CompareTag("GravityZone")) return;

            if (currentGravityZone)
            {
                GameObject go = currentGravityZone.gameObject;
                StartCoroutine(Maxipute(go));
            }

            currentGravityZone = other.transform;
            currentGravityZone.gameObject.SetActive(false);
            centerOfGravity = other.transform.position;
        }

        void ReadMovement(InputAction.CallbackContext ctx)
        {
            playerDirectionInput = ctx.ReadValue<Vector2>();
        }

        void ReadJump(InputAction.CallbackContext ctx)
        {
            RaycastHit hit;
            if (!Physics.Raycast(transform.position, -transform.up, out hit, 0.5f)) return;

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            tempGravityCenter = hit.point;
        }

        Vector3 hitPoint;
        void UpdatePlayerRotation()
        {
            if (gravityDirection == Vector3.down) return;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, 5f))
            {
                hitPoint = hit.point;

                Vector3 newUp = hit.normal;
                Vector3 forwardProjected = Vector3.ProjectOnPlane(transform.forward, newUp).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(forwardProjected, newUp);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(hitPoint, 0.1f);
        }

        IEnumerator Maxipute(GameObject pute)
        {
            yield return new WaitForSeconds(1f);

            pute.SetActive(true);
        }
    }
}
