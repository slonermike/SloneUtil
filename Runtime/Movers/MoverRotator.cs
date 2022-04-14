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
using UnityEditor;
using System.Collections;

using Slonersoft.SloneUtil.Core;

namespace Slonersoft.SloneUtil.Movers {
	public class MoverRotator : Mover {

		[MenuItem("Slonersoft/Randomize/Rotation")]
		static void RandomizeSelectionRotation() {
			foreach (GameObject o in Selection.gameObjects) {
				o.transform.RotateAround(o.transform.position, Vector3.forward, Random.Range(-180f, 180f));
			}
		}

		[MenuItem("Slonersoft/Randomize/Scale")]
		static void RandomizeSelectionScale() {
			foreach (GameObject o in Selection.gameObjects) {
				Vector3 newScale = o.transform.localScale;
				newScale.Scale(new Vector3(Random.Range(.9f, 1.1f), Random.Range(0.9f, 1.1f), 1f));
				o.transform.localScale = newScale;
			}
		}

		[MenuItem("Slonersoft/Randomize/Sprite Order")]
		static void RandomizeSpriteOrder() {
			foreach (GameObject o in Selection.gameObjects) {
				SpriteRenderer renderer = o.GetComponent<SpriteRenderer>();
				if (renderer) {
					renderer.sortingOrder = Random.Range(0,50);
				}
			}
		}

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
				rotateVector = CoreUtils.AdvanceValue (rotateVector, goalRotateVector, acceleration);

				if (rotateVector == goalRotateVector) {
					acceleration = 0f;
				}
			}

			Vector3 finalRotation = rotateVector + randomizedRotation;
			if (localRotation) {
				moverTransform.Rotate (finalRotation * Time.deltaTime);
			} else {
				moverTransform.Rotate(finalRotation * Time.deltaTime);
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

}
