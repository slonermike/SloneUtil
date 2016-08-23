/************************************************************
 * 
 *                    Mover Oscillator
 *                 2016 Slonersoft Games
 * 
 * Continually oscillates along each of the 3 axes according to
 * the specified magnitude and frequency.
 * 
 ************************************************************/

using UnityEngine;
using System.Collections;

public class MoverOscillator : MonoBehaviour {

	[Tooltip("Magnitude of oscillation along each axis.")]
	public Vector3 magnitude;

	[Tooltip("Frequency of oscillation along each axis.")]
	public Vector3 frequency;

	[Tooltip("Time offset for starting oscillation.")]
	public Vector3 startOffset;

	[Tooltip("True to oscillate in local space, false for global space.")]
	public bool localMotion = true;

	private float startTime;
	private Vector3 startPos;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		startPos = localMotion ? transform.localPosition : transform.position;
	}

	// Update is called once per frame
	void Update () {

		float time = startTime - Time.time;

		float xOffset = magnitude.x * Mathf.Sin ((time + startOffset.x) * frequency.x);
		float yOffset = magnitude.y * Mathf.Sin ((time + startOffset.y) * frequency.y);
		float zOffset = magnitude.z * Mathf.Sin ((time + startOffset.z) * frequency.z);
		Vector3 offset = new Vector3 (xOffset, yOffset, zOffset);

		if (localMotion) {
			transform.localPosition = startPos + offset;
		} else {
			transform.position = startPos + offset;
		}
	}
}
