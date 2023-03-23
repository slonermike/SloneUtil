using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slonersoft.SloneUtil.Core;

namespace Slonersoft.SloneUtil.WarKit {
    public class FaceEnemy : MonoBehaviour
    {
        public float turnSpeed = 90f;
        const float RE_EVALUATE_TIME = 0.5f;
        public bool flattenDirection = true;

        TargetFinder finder;

        // Start is called before the first frame update
        void Start()
        {
            finder = gameObject.GetOrAddComponent<TargetFinder>();
        }

        void FixedUpdate() {
            if (finder.targetDamageable) {
                if (WarKitSettings.is2D()) {
                    CoreUtils2D.TurnToPoint(transform, finder.targetDamageable.transform.position, turnSpeed);
                } else {
                    Vector3 toTarget = finder.targetDamageable.transform.position - transform.position;
                    if (flattenDirection) {
                        toTarget = Vector3.ProjectOnPlane(toTarget, transform.up);
                    }
                    CoreUtils.TurnToPoint(transform, transform.position + toTarget, turnSpeed);
                }
            }
        }
    }

}

