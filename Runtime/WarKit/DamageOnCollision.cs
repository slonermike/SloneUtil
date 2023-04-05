using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slonersoft.SloneUtil.WarKit {
    [RequireComponent(typeof(Damageable))]
    public class DamageOnCollision : MonoBehaviour
    {
        [Tooltip("-1 for death on collision.")]
        public float damageMultiplier = -1f;
        public float minImpulse = 1f;
        Damageable damageable;

        void Awake() {
            damageable = GetComponent<Damageable>();
            if (damageable == null) {
                this.enabled = false;
            }
        }

        private void OnCollisionEnter(Collision other) {
            Vector3 flattenedImpulse = Vector3.ProjectOnPlane(other.impulse, transform.up);
            if (flattenedImpulse.sqrMagnitude < minImpulse * minImpulse) {
                return;
            }

            Warrior attacker = other.gameObject.GetComponent<Warrior>();
            float damageDone = damageMultiplier < 0 ? damageable.GetHealth() : flattenedImpulse.magnitude * damageMultiplier;
            damageable.DoDamageAfterFrame(
                new BlipDamage(
                    damageDone,
                    attacker,
                    other.contacts[0].point,
                    other.contacts[0].normal
                )
            );
        }
    }

}