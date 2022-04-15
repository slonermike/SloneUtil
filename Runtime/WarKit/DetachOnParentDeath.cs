using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slonersoft.SloneUtil.Core;
using Slonersoft.SloneUtil.BlipKit;

namespace Slonersoft.SloneUtil.WarKit {
	public class DetachOnParentDeath : MonoBehaviour {

		[Tooltip("Set to a positive number to automatically destroy this a specified time after the parent dies.")]
		public float destroyAfterTime = -1f;

		public void OnParentDied()
		{
			transform.SetParent (null);

			if (destroyAfterTime > 0f) {
				gameObject.DestroyAfterTime (destroyAfterTime);
				FadeTrails ();
				StopEmitters ();
			}
		}

		void FadeTrails()
		{
			if (destroyAfterTime <= 0f) {
				return;
			}

			TrailRenderer trail = GetComponent<TrailRenderer> ();
			if (trail == null) {
				return;
			}

			if (trail.time > destroyAfterTime) {
				trail.time = destroyAfterTime;
			}
		}

		void StopEmitters()
		{
			ParticleSystem system = GetComponent<ParticleSystem> ();
			if (system) {
				system.Stop ();
			}
		}
	}

	public static class DeatchOnParentDeathUtil {
		public static void DoChildDetachments(this GameObject o)
		{
			DetachOnParentDeath[] dods = o.GetComponentsInChildren<DetachOnParentDeath> ();
			foreach (DetachOnParentDeath dod in dods) {
				dod.OnParentDied ();
			}
		}
	}

}
