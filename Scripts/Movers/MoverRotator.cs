/************************************************************
 * 
 *                     Mover Rotator
 *                 2016 Slonersoft Games
 * 
 * Continually rotates around each of the 3 axes according to
 * the rotateVector.
 * 
 ************************************************************/

using UnityEngine;
using System.Collections;

public class MoverRotator : Mover {

	[Tooltip("Each axis can have a different amount of rotation.")]
	public Vector3 rotateVector;

	[Tooltip("Maximum randomized difference from the base rotation speed.")]
	public Vector3 randomizationVector;

	[Tooltip("True to rotate around local axes, false to rotate around global axes.")]
	public bool localRotation = false;

	Vector3 randomizedRotation;

	Vector3 goalRotateVector;
	float acceleration = 0.0f;

	void OnEnable()
	{
		randomizedRotation = new Vector3 (
			Random.Range (-randomizationVector.x, randomizationVector.x),
			Random.Range (-randomizationVector.y, randomizationVector.y),
			Random.Range (-randomizationVector.z, randomizationVector.z)
		);
	}

	// Update is called once per frame
	void Update () {
		if (acceleration != 0f) {
			rotateVector = SloneUtil.AdvanceValue (rotateVector, goalRotateVector, acceleration);

			if (rotateVector == goalRotateVector) {
				acceleration = 0f;
			}
		}

		Vector3 finalRotation = rotateVector + randomizedRotation;
		if (localRotation) {
			moverTransform.Rotate (finalRotation * Time.deltaTime);
		} else {
			moverTransform.RotateAround (moverTransform.position, Vector3.right, finalRotation.x * Time.deltaTime);
			moverTransform.RotateAround (moverTransform.position, Vector3.up, finalRotation.y * Time.deltaTime);
			moverTransform.RotateAround (moverTransform.position, Vector3.forward, finalRotation.z * Time.deltaTime);
		}
	}

	// Change the speed to a new value.
	//
	// newSpeed: Speed in degrees/sec to change to.
	// accel: 0 to change immediately, positive to change over time (degrees/sec).
	//
	public void ChangeSpeed(Vector3 newSpeed, float accel = 0f)
	{
		if (accel < 0f) {
			acceleration = 0f;
			rotateVector = newSpeed;
		} else {
			acceleration = accel;
			goalRotateVector = newSpeed;
		}
	}
}
