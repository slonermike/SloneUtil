using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverScaler : Mover {

	[Tooltip("Amount of time (seconds) to take to change scale.")]
	public float scaleTime = 1.0f;

	[Tooltip("Time after which the scaling will begin.")]
	public float startDelay = 0f;

	[Tooltip("Scale to finish at after scaleTime.")]
	public Vector3 scaleGoal = Vector3.zero;

	[Tooltip("True if you want to scale to a multiple of the starting scale.  False if worldspace scale.")]
	public bool scaleAsMultiplier = true;

	float scaleSpeed;
	Vector3 startScale;

	void OnEnable()
	{
		scaleSpeed = 0f;
		if (startDelay > 0f) {
			Invoke("UpdateSpeed", startDelay);
		} else {
			UpdateSpeed();
		}
	}

	public void UpdateSpeed()
	{
		startScale = moverTransform.localScale;

		if (scaleAsMultiplier) {
			Vector3 finalScale = startScale;
			finalScale.Scale (scaleGoal);
			scaleGoal = finalScale;
		}

		if (scaleTime <= 0f) {
			moverTransform.localScale = scaleGoal;
			scaleSpeed = 0f;
		} else {
			scaleSpeed = (scaleGoal - startScale).magnitude / scaleTime;
		}
	}

	void Update()
	{
		if (scaleSpeed > 0f) {
			moverTransform.localScale = SloneUtil.AdvanceValue (moverTransform.localScale, scaleGoal, scaleSpeed);

			// Once we reach it, stop advancing.
			if (moverTransform.localScale == scaleGoal) {
				scaleSpeed = 0f;
			}
		}
	}
}
