/// <summary>
/// 2017 Slonersoft Games
/// Mover
/// Parent class to movers, allows us to grab all movers on an object.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slonersoft.SloneUtil.Movers {
	// Allows us to find all the movers on an object.
	public abstract class Mover : MonoBehaviour {
		public Transform moverDelegate;
		public Transform moverTransform {
			get {
				return moverDelegate != null ? moverDelegate : transform;
			}
		}
	}

}
