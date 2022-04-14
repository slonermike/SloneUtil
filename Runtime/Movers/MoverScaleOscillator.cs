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

namespace Slonersoft.SloneUtil {
	public class MoverScaleOscillator : Mover {

		[Tooltip("Time in seconds to make a full oscillation.")]
		public Vector3 wavelength = Vector3.one;

		[Tooltip("Scale multiplier to which the object will expand/contract.")]
		public Vector3 magnitude = Vector3.one;

		[Tooltip("Use to start at a scale partway into the animation.")]
		public Vector3 startAnimPct = Vector3.zero;

		[Tooltip("True to randomize the start time offset.")]
		public bool randomizeStart = false;

		[Tooltip("Type of curve on the oscillation.")]
		public OscillationType curveType = OscillationType.LINEAR;

		Vector3 originalScale;
		VectorOscillator oscillator;

		// Use this for initialization
		void Start () {
			originalScale = moverTransform.localScale;
			oscillator = new VectorOscillator (wavelength, startAnimPct, curveType, randomizeStart);
		}

		// Update is called once per frame
		void Update () {
			Vector3 targetScale = originalScale;
			targetScale.Scale (magnitude);
			moverTransform.localScale = oscillator.Evaluate (originalScale, targetScale);
		}
	}

}
