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

namespace Slonersoft.SloneUtil.BlipKit {
	public class DestroyAfterTime : MonoBehaviour {

		public float lifetime = 1.0f;
		public bool doDeathSpawns = false;

		[Tooltip("True to send the blip, but not actually destroy the object")]
		public bool blipOnly = false;
		Coroutine coroutine;

		private IEnumerator DestroyAfterTime_coroutine()
		{
			yield return new WaitForSeconds (lifetime);

			gameObject.SendBlip(Blip.Type.DIED);

			if (!blipOnly) {
				GameObject.Destroy (gameObject);
			}
		}

		// Use this for initialization
		private void OnEnable() {
			coroutine = StartCoroutine ( DestroyAfterTime_coroutine());
		}

		private void OnDisable() {
			StopCoroutine(coroutine);
		}
	}

	public static class DestroyAfterTime_extensions {
		/// <summary>
		/// Destroys a GameObject after a delay.
		/// </summary>
		/// <param name="gObj">Gameobject to destroy.</param>
		/// <param name="timeSec">Time in seconds after which to destroy it.</param>
		public static void DestroyAfterTime(this GameObject gObj, float timeSec) {
			DestroyAfterTime dat = gObj.AddComponent<DestroyAfterTime> ();
			dat.lifetime = timeSec;
		}
	}
}
