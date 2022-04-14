/************************************************************
 *
 *                    Value Oscillator
 *                 2017 Slonersoft Games
 *
 * Helper class for creating values that oscillate over time
 * without the use of a heavy-handed animation curve.
 *
 ************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slonersoft.SloneUtil {
	// Oscillation curve types.
	//
	public enum OscillationType {
		SMOOTH,				// Ease at both zero and one.
		LINEAR				// No easing.

		// TODO: Set these up!
		// BOUNCE,			// Ease at one, but not zero.
		// INVERTED_BOUNCE,	// Ease at zero, but not one.
	}

	public class ValueOscillator {

		float wavelength;
		float startTime;
		OscillationType curveType;

		// Create a new value oscillator.
		//
		// oscillationTime: The amount of time to do a full cycle from 0 to 1 and back.
		// startPctOffset: The percentage along the oscillationTime at which to start.
		// type: the type of curve on the oscillation
		// randomizeStart: True to choose a random time along the oscillationTime to start.
		//
		public ValueOscillator(float oscillationTime, float startPctOffset = 0.0f, OscillationType type = OscillationType.LINEAR, bool randomizeStart = false) {

			if (oscillationTime <= 0f) {
				Debug.LogError ("ValueOscillator cannot have oscillationTime (" + oscillationTime + ") less than or equal to zero.");
				oscillationTime = 1f;
			}

			wavelength = oscillationTime;
			curveType = type;

			if (randomizeStart) {
				startPctOffset = Random.Range (0f, 1f);
			}

			startTime = Time.time - (startPctOffset * wavelength);
		}

		// Evaluate the oscillator as a curve with output between 0 and 1.
		//
		private float Evaluate01()
		{
			// Multiply by 2 so that 50% yields 1.0 and 100% is back to 0.0.
			float pct = ((Time.time - startTime) / wavelength) * 2f;

			if (curveType == OscillationType.SMOOTH) {
				return SloneUtil.LerpSmooth (0f, 1f, pct, true);
			} else {
				if (curveType != OscillationType.LINEAR) {
					Debug.LogError ("Unknown OscillationType: " + curveType);
				}

				int baseNumber = Mathf.FloorToInt (pct);
				bool downSwing = (baseNumber % 2) == 1;
				float leftoverPct = pct - ((float)baseNumber);
				return downSwing ? 1.0f - leftoverPct : leftoverPct;
			}
		}

		// Get the current value of the oscillator.
		//
		// minValue: the value at the start of the curve.
		// maxValue: the value at the peak of the curve.
		//
		public float Evaluate(float minValue = 0f, float maxValue = 1f)
		{
			float pct = Evaluate01 ();
			return Mathf.Lerp (minValue, maxValue, pct);
		}

		// Get the current value of the oscillator.
		//
		// minValue: the value at the start of the curve.
		// maxValue: the value at the peak of the curve.
		//
		public Vector3 Evaluate(Vector3 minValue, Vector3 maxValue)
		{
			float pct = Evaluate01 ();
			return Vector3.Lerp (minValue, maxValue, pct);
		}
	}

}
