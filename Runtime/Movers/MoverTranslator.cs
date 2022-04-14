/// <summary>
/// 2017 Slonersoft Games
/// MoverTranslator
/// Moves an object each frame at the specified rate.
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverTranslator : Mover {

	[Tooltip("Speed in units per second.")]
	public Vector3 velocity = Vector3.zero;

	[Tooltip("True to move in local space, false to move in world space.")]
	public bool useLocalAxes = false;

	[Tooltip("Rate at which the velocity drops to zero.")]
	public float damping = 0f;

	// Update is called once per frame
	void Update () {
		if (damping > 0f) {
			velocity = SloneUtil.AdvanceValue(velocity, Vector3.zero, damping);
		}
		if (useLocalAxes) {
			moverTransform.position += (velocity.x * moverTransform.right) + (velocity.y * moverTransform.up) + (velocity.z * moverTransform.forward) * Time.deltaTime;
		} else {
			moverTransform.position += velocity * Time.deltaTime;
		}
	}
}
