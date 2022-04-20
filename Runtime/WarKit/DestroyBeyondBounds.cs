using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Slonersoft.SloneUtil.BlipKit;
using Slonersoft.SloneUtil.Core;

namespace Slonersoft.SloneUtil.WarKit {
	/// <summary>
	/// This may be deprecated unless we re-implement a definition "playble space"
	/// </summary>
	public class DestroyBeyondBounds : MonoBehaviour {

		[Tooltip("0 = destroy at edge of gameplay space, 1.0 = destroy at edge plus screen width")]
		public float pctBeyond = 0f;

		[Tooltip("True to wait until it's in bounds at least once.")]
		public bool waitTillInBounds = false;

		[Tooltip("How long we need to be offscreen before calling it done.")]
		public float timeBeyond = 0.0f;

		[Tooltip("Object to destroy.  Null to destroy self.")]
		public GameObject destroyTarget;

		[Tooltip("Local offset for position to check")]
		public Vector3 localOffset = Vector3.zero;

		float currentTimeBeyond = 0f;

		void Start() {
			if (destroyTarget == null) {
				destroyTarget = gameObject;
			}
		}

		// Update is called once per frame
		void Update () {
			DestroyIfBeyondBounds(transform.TransformPoint(localOffset));
		}

		void DestroyIfBeyondBounds(Vector3 worldPosition) {
			bool withinBounds = CoreUtils.IsPointOnAnyScreen(worldPosition, pctBeyond);
			if (waitTillInBounds && !withinBounds) {
				return;
			}

			waitTillInBounds = false;

			if (!withinBounds) {
				if (timeBeyond > 0f) {
					if (currentTimeBeyond < timeBeyond) {
						currentTimeBeyond += Time.deltaTime;
					} else {
						currentTimeBeyond = Mathf.Max (currentTimeBeyond - Time.deltaTime, 0f);
					}
				}

				destroyTarget.SendBlip(Blip.Type.DIED);
				GameObject.Destroy (destroyTarget);
			}
		}
	}
}

