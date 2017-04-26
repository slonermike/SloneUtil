/************************************************************
 * 
 *                    Scale Oscillator
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

public class MoverOscillator : MonoBehaviour {

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

	ValueOscillator[] oscillators;
	private Vector3 startPos;
	private Vector3 endPos;

	// Use this for initialization
	void Start () {
		oscillators = new ValueOscillator[3];
		oscillators [0] = new ValueOscillator (wavelength.x, startOffsetPct.x, curveType);
		oscillators [1] = new ValueOscillator (wavelength.y, startOffsetPct.y, curveType);
		oscillators [2] = new ValueOscillator (wavelength.z, startOffsetPct.z, curveType);
		startPos = localMotion ? transform.localPosition : transform.position;
		endPos = startPos + magnitude;
	}

	// Update is called once per frame
	void Update () {
		float x = SloneUtil.LerpUnbounded (startPos.x, endPos.x, oscillators [0].Evaluate ());
		float y = SloneUtil.LerpUnbounded (startPos.y, endPos.y, oscillators [1].Evaluate ());
		float z = SloneUtil.LerpUnbounded (startPos.z, endPos.z, oscillators [2].Evaluate ());

		Vector3 newPos = new Vector3 (x, y, z);

		if (localMotion) {
			transform.localPosition = newPos;
		} else {
			transform.position = newPos;
		}
	}
}
