using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slonersoft.SloneUtil.Movers {
    public class Shaker : MonoBehaviour
    {
        public float rotationFactor = 1f;

        float maxMagnitude = 0.0f;
        float timeLeft_s = 0.0f;
        float timeTotal_s = 0.0f;

        void Start() {
            if (transform.parent == null) {
                Debug.LogWarning($"Shaker is on {name} which has no parent to center it.");
            }
        }

        void Update () {

            // Subtract time until it reaches zero, then the shake is done.  Reset the system.
            timeLeft_s -= Time.deltaTime;
            if (timeLeft_s <= 0.0f) {
                timeLeft_s = 0.0f;
                timeTotal_s = 0.0f;
                maxMagnitude = 0.0f;
            }

            // Set the local position to the shaken offset.
            transform.localPosition = GetOffset ();
            Vector3 rotation = GetRotation(transform.localPosition);
            transform.localRotation = Quaternion.Euler (rotation);
        }

        // Generates a random camera offset based on the current state of the shake.
        //
        Vector2 GetOffset()
        {

            // Default offset is zero -- no shake.
            Vector2 offset = Vector2.zero;

            // If there is a current shake in play.
            if (timeLeft_s > 0f && timeTotal_s > 0f) {

                // Choose a random direction.
                float direction = Random.Range (0.0f, 360.0f);

                // Create a vector (relative to camera orientation) with a random direction rotated around the forward axis.
                offset = Quaternion.AngleAxis (direction, transform.forward) * transform.right;

                // Determine the current effective spread (between 0 and maxSpread) and multiply the offset by that.
                offset *= Mathf.Lerp (0.0f, maxMagnitude, timeLeft_s / timeTotal_s);
            }

            return offset;
        }

        Vector3 GetRotation(Vector2 offset)
        {
            return new Vector3 (offset.y * rotationFactor, offset.x * rotationFactor, 0f);
        }

        // Begin a new shake.
        //
        // time_s: Time (in seconds) for the shake to take.
        // magnitude: distance (in game units) for the shake to move the camera at its most extreme.
        //
        public void Shake(float time_s, float magnitude)
        {
            if (magnitude >= maxMagnitude) {
                maxMagnitude = magnitude;
                timeLeft_s = Mathf.Max(time_s, timeLeft_s);
                timeTotal_s = timeLeft_s;
            }
        }
    }
}