/// <summary>
/// 2017 Slonersoft Games
/// MoverTranslator
/// Moves an object each frame at the specified rate.
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slonersoft.SloneUtil.Core;

namespace Slonersoft.SloneUtil.Movers {
	public class MoverTranslator : Mover {

		[Tooltip("Speed in units per second.")]
		public Vector3 velocity = Vector3.zero;

		[Tooltip("True to move in local space, false to move in world space.")]
		public bool useLocalAxes = false;

		[Tooltip("Rate at which the velocity drops to zero.")]
		public float damping = 0f;
		private Rigidbody rb;

		void Start() {
			rb = GetComponent<Rigidbody>();
		}

		// Update is called once per frame
		void Update () {
			if (damping > 0f) {
				velocity = CoreUtils.AdvanceValue(velocity, Vector3.zero, damping);
			}

			Vector3 curVelocity = velocity;
			if (useLocalAxes) {
				curVelocity = (velocity.x * moverTransform.right) + (velocity.y * moverTransform.up) + (velocity.z * moverTransform.forward);
			}

			if (rb != null) {
				rb.velocity = curVelocity;
			} else {
				moverTransform.position += curVelocity * Time.deltaTime;
			}
		}
	}
}
