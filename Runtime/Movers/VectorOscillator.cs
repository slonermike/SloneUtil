/************************************************************
 *
 *                    Vector Oscillator
 *                 2017 Slonersoft Games
 *
 * Oscillates each of the three axes separately, defining a
 * parametric path along which it will move.
 *
 ************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slonersoft.SloneUtil {
	public class VectorOscillator {

		ValueOscillator[] oscillators = new ValueOscillator[3];

		// Current x value of the oscillation.
		public float x {
			get {
				return oscillators [0].Evaluate ();
			}
		}

		// Current y value of the oscillation.
		public float y {
			get {
				return oscillators [1].Evaluate ();
			}
		}

		// Current z value of the oscillation.
		public float z {
			get {
				return oscillators [2].Evaluate ();
			}
		}

		// Create a new vector oscillator.
		//
		// oscillationTime: The amount of time to do a full cycle from 0 to 1 and back for each axis.
		// startPctOffset: The percentage along the oscillationTime at which to start for each axis.
		// type: the type of curve on the oscillation
		// randomizeStart: True to choose a random time along the oscillationTime to start.
		//
		public VectorOscillator(Vector3 oscillationTime, Vector3 startPctOffset, OscillationType type = OscillationType.LINEAR, bool randomizeStart = false) {
			for (int i = 0; i < 3; i++) {
				oscillators [i] = new ValueOscillator (oscillationTime [i], startPctOffset [i], type, randomizeStart);
			}
		}

		// Get the current 0-1 oscillation value for each axis.
		//
		public Vector3 Evaluate()
		{
			return new Vector3 (x, y, z);
		}

		// Get interpolated values along each axis.
		//
		// minValues: the value at the valley of the curve for each axis.
		// maxValues: the value at the peak of the curve for each axis.
		//
		public Vector3 Evaluate(Vector3 minValues, Vector3 maxValues)
		{
			return new Vector3 (oscillators [0].Evaluate (minValues.x, maxValues.x),
				oscillators [1].Evaluate (minValues.y, maxValues.y),
				oscillators [2].Evaluate (minValues.z, maxValues.z));
		}
	}

}
