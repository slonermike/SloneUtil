using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slonersoft.SloneUtil.Core;

namespace Slonersoft.SloneUtil.Movers {
	public class RotateTo : Mover {

		public float rotateTime = 1.0f;
		public Vector3 rotateAngles;
		public bool localMotion = true;
		public bool smoothMotion = true;

		Coroutine moveRoutine;

		void OnEnable()
		{
			Rotate(rotateAngles, rotateTime, smoothMotion, localMotion);
		}

		void OnDisable()
		{
			CancelRotate();
		}

		IEnumerator Rotate_coroutine(Vector3 newAngles, float time, bool smooth = true, bool localMovement = true)
		{
			Vector3 startAngles;
			if (localMovement)
				startAngles = moverTransform.localEulerAngles;
			else
				startAngles = moverTransform.eulerAngles;

			float startTime = Time.time;

			if (rotateTime > 0) {
				while (startTime + rotateTime > Time.time) {
					yield return new WaitForEndOfFrame();
					float pct = (Time.time - startTime) / rotateTime;

					if (smooth) {
						pct = CoreUtils.LerpSmooth (0f, 1f, pct);
					}

					if (localMovement) {
						moverTransform.localEulerAngles = CoreUtils.LerpEulerAngles (startAngles, newAngles, pct);
					} else {
						moverTransform.eulerAngles = CoreUtils.LerpEulerAngles (startAngles, newAngles, pct);
					}
				}
			}

			if (localMovement) {
				moverTransform.localEulerAngles = newAngles;
			} else {
				moverTransform.eulerAngles = newAngles;
			}
		}

		public void Rotate(Vector3 newAngles, float time, bool smooth = true, bool localMovement = true)
		{
			CancelRotate();
			moveRoutine = StartCoroutine (Rotate_coroutine(newAngles, rotateTime, smooth, localMovement));
		}

		public void CancelRotate()
		{
			if (moveRoutine != null) {
				StopCoroutine (moveRoutine);
				moveRoutine = null;
			}
		}
	}


}
