using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slonersoft.SloneUtil.WarKit {
    /// <summary>
    /// Implement and add to an entity to allow it to
    /// find and track targets.
    /// </summary>
    public abstract class Targeting : MonoBehaviour
    {
        public GameObject target {
            get {
                return GetTarget();
            }
        }

        public Damageable targetDamageable {
            get {
                return GetTargetDamageable();
            }
        }

        protected abstract GameObject GetTarget();
        protected virtual Damageable GetTargetDamageable() {
            return null;
        }
    }
}
