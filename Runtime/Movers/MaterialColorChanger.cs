using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slonersoft.SloneUtil {
	public class MaterialColorChanger : MonoBehaviour {

		public SharedMaterialInstance sharedMaterialInstance;
		public Color endColor = Color.white;
		public float changeTime = 1.0f;
		float startTime;

		Color startColor;

		void OnEnable()
		{
			Material m = sharedMaterialInstance.GetMaterialInstance ();
			if (m != null) {
				startColor = m.color;
			}

			startTime = Time.time;
		}

		// Update is called once per frame
		void Update () {
			float pct = 1.0f;
			if (changeTime > 0f) {
				pct = Mathf.Clamp01((Time.time - startTime) / changeTime);
			}

			sharedMaterialInstance.GetMaterialInstance().color = SloneUtil.Lerp (startColor, endColor, pct);
		}
	}

}
