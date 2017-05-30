/************************************************************
 * 
 *                    Mover Oscillator
 *                 2017 Slonersoft Games
 * 
 * Unity component for oscillating an object's position.
 * 
 * Allows separate parameters along each axis, allowing complex
 * "spirograph-style" movement patterns without the overhead of
 * animations or animation curves.
 * 
 ************************************************************/

using UnityEngine;
using System.Collections;

public class MoverOscillator : Mover {

	[Tooltip("Magnitude of oscillation along each axis.")]
	public Vector3 magnitude;

	[Tooltip("Time (in seconds) of a full oscillation.")]
	public Vector3 wavelength = Vector3.one;

	[Tooltip("Animation percentage offset for starting oscillation.")]
	public Vector3 startOffsetPct;

	[Tooltip("Type of curve on the oscillation.")]
	public OscillationType curveType = OscillationType.LINEAR;

	[Tooltip("True to oscillate in local space, false for global space.")]
	public bool localMotion = true;

	[Tooltip("True to rotate in both directions, with the start position as the center.")]
	public bool biDirectional = true;

	VectorOscillator oscillator;
	private Vector3 startPos;

	// Use this for initialization
	void Start () {

		// Bi-directionals start in the middle of the upswing.
		if (biDirectional) {
			startOffsetPct += Vector3.one * 0.25f;
		}

		oscillator = new VectorOscillator (wavelength, startOffsetPct, curveType);
		startPos = localMotion ? transform.localPosition : transform.position;
	}

	// Update is called once per frame
	void Update () {
		Vector3 newPos;

		if (biDirectional) {
			newPos = oscillator.Evaluate (startPos - magnitude, startPos + magnitude);
		} else {
			newPos = oscillator.Evaluate (startPos, startPos + magnitude);
		}

		if (localMotion) {
			transform.localPosition = newPos;
		} else {
			transform.position = newPos;
		}
	}
}
