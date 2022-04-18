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
            // TODO: make it work in 3d.
            // Should there be some global 2d/3d setting for these scripts?
            if (finder.targetDamageable) {
                CoreUtils2D.TurnToPoint(transform, finder.targetDamageable.transform.position, turnSpeed);
            }
        }
    }

}

