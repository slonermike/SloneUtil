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

public class MoverRotateOscillator : MonoBehaviour {

	[Tooltip("Magnitude of oscillation around each axis.")]
	public Vector3 magnitude;

	[Tooltip("Frequency of oscillation around each axis.")]
	public Vector3 frequency;

	[Tooltip("Time offset for starting each axis of oscillation.")]
	public Vector3 startOffset;

	Vector3 startRotation;
	float startTime;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		startRotation = transform.localRotation.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		float time = startTime - Time.time;

		float xOffset = magnitude.x * Mathf.Sin ((time + startOffset.x) * frequency.x);
		float yOffset = magnitude.y * Mathf.Sin ((time + startOffset.y) * frequency.y);
		float zOffset = magnitude.z * Mathf.Sin ((time + startOffset.z) * frequency.z);

		Vector3 offset = new Vector3 (xOffset, yOffset, zOffset);

		transform.localEulerAngles = startRotation + offset;
	}
}
