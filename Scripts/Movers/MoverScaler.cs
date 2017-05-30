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

	void Start()
	{
		if (scaleTime <= 0f) {
			Debug.LogError ("MoverScaler given zero or negative scale time: " + scaleTime);
			scaleTime = 0.1f;
		}

		startScale = transform.localScale;

		if (scaleAsMultiplier) {
			Vector3 finalScale = startScale;
			finalScale.Scale (scaleGoal);
			scaleGoal = finalScale;
		}

		scaleSpeed = (scaleGoal - startScale).magnitude / scaleTime;
	}

	void Update()
	{
		if (startDelay > 0f) {
			startDelay -= Time.deltaTime;
		}

		transform.localScale = SloneUtil.AdvanceValue (transform.localScale, scaleGoal, scaleSpeed);

		// Disable it when we meet the goal.
		if (transform.localScale == scaleGoal) {
			this.enabled = false;
		}
	}
}
