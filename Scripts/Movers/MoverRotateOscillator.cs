/************************************************************
 * 
 *                Mover Rotate Oscillator
 *                 2016 Slonersoft Games
 * 
 * Continually oscillates around each of the 3 axes according to
 * the specified magnitude and frequency.
 * 
 ************************************************************/

using UnityEngine;
using System.Collections;

public class MoverRotateOscillator : Mover {

	[Tooltip("Magnitude of oscillation around each axis.")]
	public Vector3 magnitude;

	[Tooltip("Time it takes to get from here to there and back.")]
	public Vector3 wavelength = Vector3.one;

	[Tooltip("Percentage offset for starting each axis of oscillation.")]
	public Vector3 startPctOffset;

	[Tooltip("Type of curve to use for the oscillation.")]
	public OscillationType curveType = OscillationType.LINEAR;

	[Tooltip("True to oscillate in both directions with the start point as the center.")]
	public bool biDirectional = true;

	Vector3 startRotation;
	VectorOscillator oscillator;

	// Use this for initialization
	void Start () {
		if (biDirectional) {
			startPctOffset += Vector3.one * 0.25f;
		}
		oscillator = new VectorOscillator (wavelength, startPctOffset, curveType);
		startRotation = moverTransform.localRotation.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		if (biDirectional) {
			moverTransform.localEulerAngles = oscillator.Evaluate (startRotation - magnitude, startRotation + magnitude);
		} else {
			moverTransform.localEulerAngles = oscillator.Evaluate (startRotation, startRotation + magnitude);
		}
	}
}
