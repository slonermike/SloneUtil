/************************************************************
 * 
 *                  Destroy After Time
 *                 2017 Slonersoft Games
 * 
 * Destroy this object after a period of time.
 * 
 * Allows temporary objects, such as explosions, hit fx, etc to
 * clean themselves up automatically.
 * 
 ************************************************************/

using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {

	public float lifetime = 1.0f;

	// Use this for initialization
	void Start () {
		StartCoroutine ( SloneUtil.DestroyAfterTime(gameObject, lifetime));
	}
}
