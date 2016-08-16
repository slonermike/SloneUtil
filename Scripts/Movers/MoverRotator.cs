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

public class MoverRotator : MonoBehaviour {

	[Tooltip("Each axis can have a different amount of rotation.")]
	public Vector3 rotateVector;

	[Tooltip("True to rotate locally, false to rotate globally.")]
	public bool localRotation = false;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (localRotation) {
			transform.Rotate (rotateVector * Time.deltaTime);
		} else {
			transform.RotateAround (transform.position, Vector3.right, rotateVector.x * Time.deltaTime);
			transform.RotateAround (transform.position, Vector3.up, rotateVector.y * Time.deltaTime);
			transform.RotateAround (transform.position, Vector3.forward, rotateVector.z * Time.deltaTime);
		}
	}
}
