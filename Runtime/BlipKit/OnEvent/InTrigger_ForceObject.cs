using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slonersoft.SloneUtil.BlipKit {
    public class InTrigger_ForceObject : MonoBehaviour
    {
        public float pushVelocity = 5f;
        public float pushCurveRadius = 1f;
        public AnimationCurve pushCurve = AnimationCurve.Constant(0f, 1f, 1f);
        public Transform centerDelegate;
        public bool flattenPushVec = true;

        private void OnTriggerEnter(Collider other) {
            Debug.Log("entered");
        }
        private void OnTriggerExit(Collider other) {
            Debug.Log("Exited");
        }
        private void OnTriggerStay(Collider other) {
            Transform center = centerDelegate ? centerDelegate : transform;
            Vector3 toObject = (other.transform.position - center.position);
            if (flattenPushVec) {
                toObject = Vector3.ProjectOnPlane(toObject, center.up);
            }
            float distancePct = Mathf.Clamp01(toObject.magnitude / pushCurveRadius);
            float distanceModifier = pushCurve.Evaluate(distancePct);
            other.gameObject.SendBlip(
                Blip.Type.FORCE,
                new BlipForce(toObject.normalized * distanceModifier * pushVelocity)
            );
        }
    }

}