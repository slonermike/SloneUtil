using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePhysics : Mover {

	public Vector3 velocity;
	public List<Vector3> accelerators = new List<Vector3> ();
	public float topSpeed = -1f;

	public bool localMotion = false;
	
	// Update is called once per frame
	void FixedUpdate () {
		foreach (Vector3 acc in accelerators) {
			velocity += acc * Time.deltaTime;
		}

		if (topSpeed > 0) {
			velocity = SloneUtil.CapMagnitude (velocity, topSpeed);
		}

		Vector3 frameMove = velocity * Time.fixedDeltaTime;
		if (localMotion) {
			Vector3 forward = moverTransform.forward;
			Vector3 right = moverTransform.right;
			Vector3 up = moverTransform.up;
			moverTransform.position += ((right * frameMove.x) + (up * frameMove.y) + (forward * frameMove.z));
		} else {
			moverTransform.position += frameMove;
		}
	}
}
