using System.Collections;
using UnityEngine;

namespace GolemWave
{
    public class LaserDamage : MonoBehaviour
    {
        [SerializeField] private AudioSource tickAudioSource;
        [SerializeField] private int damages;
        private Coroutine damageRoutine;
        private float timer = 0.4f;

        private IEnumerator DamageRoutine(IDamageable damageable)
        {
            while (damageable.Health > 0)
            {
                damageable.TakeDamage(damages);
                tickAudioSource.Play();
                yield return new WaitForSeconds(timer);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out IDamageable damageable)) return;

            if (damageRoutine != null) StopCoroutine(damageRoutine);
            damageRoutine = StartCoroutine(DamageRoutine(damageable));
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out IDamageable _)) return;

            if (damageRoutine != null) StopCoroutine(damageRoutine);
            damageRoutine = null;
        }
    }
}
