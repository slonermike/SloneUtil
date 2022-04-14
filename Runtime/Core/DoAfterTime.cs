/************************************************************
 *
 *                  Destroy After Time
 *                 2020 Slonersoft Games
 *
 * Do a thing after some specified amount of time.
 *
 ************************************************************/

using UnityEngine;
using System.Collections;

namespace Slonersoft.SloneUtil.Core {
	public class DoAfterTime : MonoBehaviour {

			public delegate void DelayedAction();

		public float lifetime = 1.0f;
		public DelayedAction action;

		private IEnumerator DoAfterTime_coroutine()
		{
			yield return new WaitForSeconds (lifetime);

			action();

			Destroy(this);
		}

		// Use this for initialization
		void Start () {
			StartCoroutine ( DoAfterTime_coroutine());
		}
	}
}
