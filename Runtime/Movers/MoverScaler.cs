using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Slonersoft.SloneUtil.Core;

namespace Slonersoft.SloneUtil.Movers {
	public class MoverScaler : Mover {

		[Tooltip("Amount of time (seconds) to take to change scale.")]
		public float scaleTime = 1.0f;

		[Tooltip("Time after which the scaling will begin.")]
		public float startDelay = 0f;

		[Tooltip("Scale to finish at after scaleTime.")]
		public Vector3 scaleGoal = Vector3.zero;

		[Tooltip("True if you want to scale to a multiple of the starting scale.  False if worldspace scale.")]
		public bool scaleAsMultiplier = true;
		public Coroutine changeCoroutine;

		IEnumerator Scale_coroutine () {
			CancelScale();

			yield return new WaitForSeconds(startDelay);
			float timeRemaining = scaleTime;
			Vector3 startScale = moverTransform.localScale;
			Vector3 endScale = scaleGoal;

			if (scaleAsMultiplier) {
				endScale.Scale (scaleGoal);
			}

			do {
				timeRemaining = CoreUtils.AdvanceValue(timeRemaining, 0f, 1f);
				float progressPct = 1f - (timeRemaining / scaleTime);
				transform.localScale = Vector3.Lerp(startScale, endScale, progressPct);
				yield return new WaitForEndOfFrame();
			} while (timeRemaining > 0f);
		}

		void OnEnable()
		{
			changeCoroutine = StartCoroutine(Scale_coroutine());
		}

		void OnDisable() {
			CancelScale();
		}

		void CancelScale() {
			if (changeCoroutine != null) {
				StopCoroutine(changeCoroutine);
				changeCoroutine = null;
			}
		}
	}

}
