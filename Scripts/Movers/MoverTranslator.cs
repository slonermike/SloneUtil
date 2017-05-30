/// <summary>
/// 2017 Slonersoft Games
/// MoverTranslator
/// Moves an object each frame at the specified rate.
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverTranslator : MonoBehaviour {

	// Speed in units per second.
	public Vector3 velocity = Vector3.zero;

	// True to move in local space, false to move in world space.
	public bool useLocalAxes = false;

	// Update is called once per frame
	void Update () {
		if (useLocalAxes) {
			transform.position += (velocity.x * transform.right) + (velocity.y * transform.up) + (velocity.z * transform.forward) * Time.deltaTime;
		} else {
			transform.position += velocity * Time.deltaTime;
		}
	}
}
