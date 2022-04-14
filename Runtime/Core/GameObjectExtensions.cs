using System;
using UnityEngine;

namespace Slonersoft.SloneUtil.Core {

  public static class SloneUtilExtensions {
    /// <summary>
		/// Get the component if it exists.  If it doesn't, create it, and return that.
		/// </summary>
		/// <param name="gameObject">Gameobject where the component will live.</param>
		/// <typeparam name="T">Type of game object to retrieve.</typeparam>
		/// <returns>Unity Engine Component</returns>
		public static T GetOrAddComponent<T>(this GameObject gameObject) where T : UnityEngine.Component {
			T component = gameObject.GetComponent<T>();
			if (component != null) {
				return component;
			}

			return gameObject.AddComponent<T>();
		}

    /// <summary>
		/// Returns true if xform is ahead of target (According to transform/fwd vector)
		/// </summary>
		/// <param name="xform">source transform</param>
		/// <param name="target">Target transform.</param>
		/// <param name="maxAngle">Maximum angle difference from centered.</param>
		/// <returns>True if it's within the threshold, false otherwise.</returns>
		public static bool IsAheadOf(this Transform xform, Transform target, float maxAngle = 90f) {
			float radians = maxAngle * Mathf.Deg2Rad;
			float cosAngle = Mathf.Cos (radians);
			float dot = Vector3.Dot ((xform.position - target.position).normalized, target.forward.normalized);
			return dot >= cosAngle;
		}

    /// <summary>
		/// Returns true if ahead is ahead of behind (According to transform/fwd vector)
		/// </summary>
		/// <param name="obj">source object</param>
		/// <param name="target">target object</param>
		/// <returns>True if it's within the threshold, false otherwise.</returns>
		public static bool IsAheadOf(this GameObject obj, GameObject target)  {
			return obj.transform.IsAheadOf (target.transform);
		}

    public static void DoAfterTime(this GameObject gameObject, float timeSec, DoAfterTime.DelayedAction action) {
			DoAfterTime dat = gameObject.AddComponent<DoAfterTime>();
			dat.lifetime = timeSec;
			dat.action = action;
		}
  }
}