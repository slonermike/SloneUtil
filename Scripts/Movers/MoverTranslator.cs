/// <summary>
/// 2017 Slonersoft Games
/// MoverTranslator
/// Moves an object each frame at the specified rate.
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverTranslator : Mover {

	// Speed in units per second.
	public Vector3 velocity = Vector3.zero;

	// True to move in local space, false to move in world space.
	public bool useLocalAxes = false;

	// Update is called once per frame
	void Update () {
		if (useLocalAxes) {
			moverTransform.position += (velocity.x * moverTransform.right) + (velocity.y * moverTransform.up) + (velocity.z * moverTransform.forward) * Time.deltaTime;
		} else {
			moverTransform.position += velocity * Time.deltaTime;
		}
	}
}
