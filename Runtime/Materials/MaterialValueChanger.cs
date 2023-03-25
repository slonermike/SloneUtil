using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slonersoft.SloneUtil.Core;

namespace Slonersoft.SloneUtil.Materials {
	public class MaterialValueChanger : MonoBehaviour {
        public string valueName = "_valueName";
        public float startValue = 0f;
        public float goalValue = 1f;
        public float changeTime = 1f;
		public float startDelay = 0f;
        public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        Coroutine changeCoroutine;

		IEnumerator Change_coroutine(MaterialInstancer instancer) {
            CancelCurrentChange();
            yield return new WaitForSeconds(startDelay);

            float timeRemaining = changeTime;

            do {
                timeRemaining = CoreUtils.AdvanceValue(timeRemaining, 0f, 1f);
                float progressPct = 1f - (timeRemaining / changeTime);
                float newVal = Mathf.Lerp(startValue, goalValue, progressPct);
                instancer.SetFloat(valueName, newVal);
                yield return new WaitForEndOfFrame();
            } while (timeRemaining > 0f);
        }

		void OnEnable()
		{
            MaterialInstancer instancer = gameObject.GetOrAddComponent<MaterialInstancer>();
			changeCoroutine = StartCoroutine(Change_coroutine(instancer));
		}

        void OnDisable() {
            CancelCurrentChange();
        }

        void CancelCurrentChange() {
            if (changeCoroutine != null) {
                StopCoroutine(changeCoroutine);
                changeCoroutine = null;
            }
        }
	}

}
