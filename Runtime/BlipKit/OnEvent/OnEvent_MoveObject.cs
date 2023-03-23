using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Slonersoft.SloneUtil.Core;

namespace Slonersoft.SloneUtil.BlipKit {

    public class OnEvent_MoveObject : MonoBehaviour
    {
        Coroutine moveRoutine = null;
        public GameObject eventFromObject;
        public Transform objToMove;

        [System.Serializable]
        public class EventPair {
            [Tooltip("The event to trigger the move.")]
            public Blip.Type eventType = Blip.Type.ACTIVATE;

            [Tooltip("Position to move to")]
            public Vector3 position;

            public bool localSpace = true;
            public bool relativeToStart = true;
            public float moveTime = 1f;
            public bool smooth = true;

            private Vector3 _startPos;
            public Vector3 startPos {
                get {
                    return _startPos;
                }
                set {
                    _startPos = value;
                }
            }
        }

        public List<EventPair> pairs;

        IEnumerator Move_coroutine(Vector3 newPosition, float moveTime, bool smooth, bool local)
		{
			Vector3 startPosition;
			if (local)
				startPosition = objToMove.localPosition;
			else
				startPosition = objToMove.position;

			float startTime = Time.time;

			if (moveTime > 0) {
				while (startTime + moveTime > Time.time) {
					yield return new WaitForFixedUpdate();
					float pct = (Time.time - startTime) / moveTime;

					if (local) {
						if (smooth) {
							objToMove.localPosition = CoreUtils.LerpSmooth (startPosition, newPosition, pct);
						} else {
							objToMove.localPosition = Vector3.Lerp (startPosition, newPosition, pct);
						}
					} else {
						if (smooth) {
							objToMove.position = CoreUtils.LerpSmooth (startPosition, newPosition, pct);
						} else {
							objToMove.position = Vector3.Lerp (startPosition, newPosition, pct);
						}
					}
				}
			}

			if (local) {
				objToMove.localPosition = newPosition;
			} else {
				objToMove.position = newPosition;
			}

			moveRoutine = null;
		}

        public void CancelMove()
		{
			if (moveRoutine != null) {
				StopCoroutine (moveRoutine);
				moveRoutine = null;
			}
		}

        void Awake() {
            objToMove = objToMove ? objToMove : transform;
            eventFromObject = eventFromObject ? eventFromObject : gameObject;

            foreach (EventPair e in pairs) {
                e.startPos = e.localSpace ? objToMove.localPosition : objToMove.position;
            }

            foreach (EventPair p in pairs) {
                eventFromObject.ListenForBlips(p.eventType, delegate() {
                    CancelMove();
                    Vector3 newPosition = p.position;
                    if (p.relativeToStart) {
                        newPosition += p.startPos;
                    }
			        moveRoutine = StartCoroutine (Move_coroutine(newPosition, p.moveTime, p.smooth, p.localSpace));
                });
            }
        }
    }
}
