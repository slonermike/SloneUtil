using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slonersoft.SloneUtil {
	// Moves things in local space to a specified point.
	//
	public class MoverRotationPositioner : Mover {

		public bool isMoving {
			get {
				return rotateRoutine != null;
			}
		}

		Coroutine rotateRoutine = null;

		IEnumerator Rotate_coroutine(Vector3 newRotation, float moveTime, bool localMovement = false)
		{
			Vector3 startRotation;
			if (localMovement)
				startRotation = moverTransform.localEulerAngles;
			else
				startRotation = moverTransform.eulerAngles;

			float startTime = Time.time;

			if (moveTime > 0) {
				while (startTime + moveTime > Time.time) {
					yield return new WaitForFixedUpdate();
					float pct = (Time.time - startTime) / moveTime;
					if (localMovement) {
						moverTransform.localEulerAngles = SloneUtil.LerpEulerAngles (startRotation, newRotation, pct);
					} else {
						moverTransform.eulerAngles = SloneUtil.LerpEulerAngles (startRotation, newRotation, pct);
					}
				}
			}

			if (localMovement) {
				moverTransform.localEulerAngles = newRotation;
			} else {
				moverTransform.eulerAngles = newRotation;
			}

			rotateRoutine = null;
		}

		public void Rotate(Vector3 newRotation, float moveTime, bool localMovement = false)
		{
			if (rotateRoutine != null) {
				StopCoroutine (rotateRoutine);
				rotateRoutine = null;
			}

			rotateRoutine = StartCoroutine (Rotate_coroutine(newRotation, moveTime, localMovement));
		}
	}

}
