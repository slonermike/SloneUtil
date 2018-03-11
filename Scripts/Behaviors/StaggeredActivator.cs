using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Deactivates the objects, and reactivates them in a staggered fashion,
/// either spread out evenly over time, or distributed randomly.
/// </summary>
public class StaggeredActivator : MonoBehaviour {

	[Tooltip("Minimum time between this object being enabled, and activating the target.")]
	public float minDelay = 1f;

	[Tooltip("Maximum time between this object being enabled, and activating the target.")]
	public float maxDelay = 2f;

	[Tooltip("True to choose timing randomly, false to spread evenly in order.")]
	public bool randomize = false;

	[Tooltip("Objects to activate.")]
	public GameObject[] targets;

	private IEnumerator ActivateAfterTime(GameObject o, float time) {
		yield return new WaitForSeconds(time);
		o.SetActive(true);
	}

	private IEnumerator ActivateEvenly()
	{
		yield return new WaitForSeconds(minDelay);
		float delayBetween = targets.Length > 1 ?
			(maxDelay - minDelay) / (float)(targets.Length-1) :
			0f;
		foreach(GameObject o in targets) {
			o.SetActive(true);
			yield return new WaitForSeconds(delayBetween);
		}
	}

	private void OnEnable() {
		foreach(GameObject o in targets) {
			o.SetActive(false);
		}

		if (targets.Length > 0) {
			if (randomize) {
				foreach(GameObject o in targets) {
					StartCoroutine(ActivateAfterTime(o, Random.Range(minDelay, maxDelay)));
				}
			} else {
				StartCoroutine(ActivateEvenly());
			}
		}
	}
}
