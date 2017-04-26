/************************************************************
 * 
 *                    Scale Oscillator
 *                 2017 Slonersoft Games
 * 
 * Unity component for oscillating an object's scale.
 * 
 ************************************************************/

using UnityEngine;
using System.Collections;

public class MoverScaleOscillator : MonoBehaviour {
	
	[Tooltip("Time in seconds to make a full oscillation.")]
	public float wavelength = 1.0f;

	[Tooltip("Scale to which the object will expand/contract.")]
	public Vector3 targetScale = Vector3.one;

	[Tooltip("Check to use the targetScale as a multiplier of the initial scale of the object.")]
	public bool targetAsMultiplier = false;

	[Tooltip("Use to start at a scale partway into the animation.")]
	[Range(0f,1f)]
	public float startAnimPct = 0f;

	[Tooltip("True to randomize the start time offset.")]
	public bool randomizeStart = false;

	[Tooltip("Type of curve on the oscillation.")]
	public OscillationType curveType = OscillationType.LINEAR;

	Vector3 originalScale;
	ValueOscillator oscillator;

	// Use this for initialization
	void Start () {
		originalScale = transform.localScale;
		oscillator = new ValueOscillator (wavelength, startAnimPct, curveType, randomizeStart);

		if (targetAsMultiplier) {
			targetScale.Scale (originalScale);
		}
	}
	
	// Update is called once per frame
	void Update () {
		float pct = oscillator.Evaluate ();
		transform.localScale = SloneUtil.LerpUnbounded (originalScale, targetScale, pct);
	}
}
