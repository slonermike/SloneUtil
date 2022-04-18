using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slonersoft.SloneUtil.Core;

namespace Slonersoft.SloneUtil.Movers {
    /// <summary>
    /// Follows an object's path exactly (cut no corners).
    /// </summary>
    public class FollowObjectPath : MonoBehaviour
    {
        /// <summary>
        /// The transform of the object to follow.
        /// </summary>
        public Transform target;

        /// <summary>
        /// Distance from the target at which we stop and wait.
        /// </summary>
        public float followDistance = 1f;

        /// <summary>
        /// Top speed in units per second to perform the follow.
        /// </summary>
        public float speed = 20f;

        /// <summary>
        /// True if it should automatically update position, false to call UpdatePosition manually.
        /// </summary>
        public bool selfUpdate = true;

        /// <summary>
        /// Total length along the laid path markers.
        /// </summary>
        public float totalInnerLength = 0f;

        /// <summary>
        /// Distance between laid markers.
        /// </summary>
        public float pathGranularity = 0.5f;

        /// <summary>
        /// Beginning of the path is nearest this object, end of the path is nearest the target object.
        /// </summary>
        private List<Vector3> path = new List<Vector3>();

        // Start is called before the first frame update
        void Start()
        {
        }

        void AddPoint() {
            if (path.Count > 0) {
                float distSq = (target.position - path.Last()).sqrMagnitude;

                // If we haven't moved far enough, don't add a point.
                if (distSq < pathGranularity * pathGranularity) {
                    return;
                }

                // The path within the laid points has grown.
                totalInnerLength += Mathf.Sqrt(distSq);
            }

            path.Add(target.position);
        }

        void RemovePoint() {
            if (path.Count > 1) {
                totalInnerLength -= (path[1] - path[0]).magnitude;
            }

            path.RemoveAt(0);
        }

        float GetTotalPathLength() {
            float totalRemainingDistance = totalInnerLength;

            if (path.Count > 0) {
                totalRemainingDistance += (path[0] - transform.position).magnitude;
                totalRemainingDistance += (path.Last() - target.position).magnitude;
            }

            return totalRemainingDistance;
        }

        public void UpdatePosition() {
            if (target == null) {
                return;
            }

            Vector3 newPosition = transform.position;
            float remainingThisMove = speed * Time.deltaTime;

            if (target.position != transform.position)
                AddPoint();

            float totalRemainingDistance = GetTotalPathLength();

            if (remainingThisMove + followDistance > totalRemainingDistance) {
                remainingThisMove = totalRemainingDistance - followDistance;
            }

            // While there are entries left in the path and you're greatert han the follow distance away...
            while (remainingThisMove > 0f && path.Count > 0) {
                Vector3 nextPosition = path[0];
                Vector3 toNextPosition = (nextPosition - newPosition);
                float distToNextPosition = toNextPosition.magnitude;
                if (distToNextPosition < remainingThisMove) {
                    newPosition = nextPosition;
                    remainingThisMove -= distToNextPosition;
                    RemovePoint();
                } else {
                    newPosition = newPosition + (toNextPosition.normalized * remainingThisMove);
                    break;
                }
            }

            transform.position = newPosition;
        }

        // Update is called once per frame
        void Update()
        {
            // TODO: should this be happening in FixedUpdate?
            if (selfUpdate) {
                UpdatePosition();
            }
        }
    }

}
