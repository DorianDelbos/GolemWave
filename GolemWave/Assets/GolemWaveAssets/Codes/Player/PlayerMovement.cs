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

        private void InitializeMovement()
        {
            rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            Cursor.lockState = CursorLockMode.Locked;
        }

        void UpdateMovement()
        {
            RaycastHit hit;

            // Raycast entre le joueur et le centre de gravité
            Vector3 gravityDirectionToCenter = centerOfGravity - transform.position; // Direction vers le centre de gravité
            if (Physics.Raycast(transform.position, gravityDirectionToCenter.normalized, out hit, gravityDirectionToCenter.magnitude))
            {
                // Si le raycast touche une surface, on met la gravité à la normale de l'impact
                gravityDirection = -hit.normal;
            }
            else
            {
                // Si aucune surface n'est touchée, la gravité va vers le centre de gravité
                gravityDirection = (centerOfGravity - transform.position).normalized;
            }

            // Gravité dirigée vers le bas si aucun centre de gravité n'est défini
            if (centerOfGravity == Vector3.zero)
            {
                gravityDirection = Vector3.down;
            }

            Vector3 gravityUp = -gravityDirection;

            // Ajuste l'orientation de l'objet à la normale du sol ou du centre de gravité
            lookAtTarget.transform.up = Vector3.Lerp(lookAtTarget.transform.up, gravityUp, 5f * Time.deltaTime);

            // Calcule la direction de la caméra projetée sur la surface
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;

            cameraForward = Vector3.ProjectOnPlane(cameraForward, gravityUp).normalized;
            cameraRight = Vector3.ProjectOnPlane(cameraRight, gravityUp).normalized;

            // Calcule la direction de mouvement
            Vector3 moveDirection = (cameraForward * playerDirectionInput.y) + (cameraRight * playerDirectionInput.x);

            if (moveDirection.magnitude > 0.1f)
            {
                moveDirection = Vector3.ProjectOnPlane(moveDirection, gravityDirection).normalized;

                Quaternion targetRotation = Quaternion.LookRotation(moveDirection, -gravityDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            rb.MovePosition(rb.position + moveDirection * speed * Time.deltaTime);
            rb.AddForce(gravityDirection.normalized * 9.81f * 0.3f, ForceMode.Force);

            UpdatePlayerRotation();
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
        }

        void UpdatePlayerRotation()
        {
            if (gravityDirection == Vector3.down) return;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, 5f))
            {
                Vector3 newUp = hit.normal;
                Vector3 forwardProjected = Vector3.ProjectOnPlane(transform.forward, newUp).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(forwardProjected, newUp);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
            }
        }

        private void OnDrawGizmos()
        {
        }

        IEnumerator Maxipute(GameObject pute)
        {
            yield return new WaitForSeconds(1f);

            pute.SetActive(true);
        }
    }
}
