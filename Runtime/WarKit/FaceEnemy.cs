using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slonersoft.SloneUtil.Core;

namespace Slonersoft.SloneUtil.WarKit {
    public class FaceEnemy : MonoBehaviour
    {
        public float turnSpeed = 90f;
        const float RE_EVALUATE_TIME = 0.5f;

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
                    CoreUtils.TurnToPoint(transform, finder.targetDamageable.transform.position, turnSpeed);
                }
            }
        }
    }

}

