/************************************************************
 *
 *                      Slone Utils
 *                 2016 Slonersoft Games
 *
 * Generic utilities for making games in the Unity engine.
 *
 * Do what you want.  Distributed with WTFPL license.
 *
 ************************************************************/

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class SloneUtil
{
	/// <summary>
	/// If you need a temporary game object, use this one, so we don't keep destroying and re-creating them.
	/// </summary>
	private static GameObject _tempObject;
	public static GameObject tempObject {
		get {
			if (_tempObject == null) {
				_tempObject = new GameObject ("SloneUtil Helper");
			}
			return _tempObject;
		}
	}

	/// <summary>
	/// Pre-made animation curve set to ease in and out.
	/// </summary>
	private static AnimationCurve _easeInOut;
	public static AnimationCurve easeInOut {
		get {
			if (_easeInOut == null) {
				_easeInOut = AnimationCurve.EaseInOut (0f, 0f, 1f, 1f);
				_easeInOut.postWrapMode = WrapMode.Once;
				_easeInOut.preWrapMode = WrapMode.Once;
			}
			return _easeInOut;
		}
	}

	/// <summary>
	/// Pre-made animation curve set to oscillate smoothly.
	/// </summary>
	private static AnimationCurve _easeOscillate;
	public static AnimationCurve easeOscillate {
		get {
			if (_easeOscillate == null) {
				_easeOscillate = AnimationCurve.EaseInOut (0f, 0f, 0.5f, 1f);
				_easeOscillate.postWrapMode = WrapMode.PingPong;
				_easeOscillate.preWrapMode = WrapMode.PingPong;
			}
			return _easeOscillate;
		}
	}

	/// <summary>
	/// Get the ordinal string (1st, 2nd, 3rd, etc) associated with an integer.
	/// </summary>
	/// <param name="number">integer number to convert to an ordinal string.</param>
	/// <returns>Ordinal string (1st, 2nd, 3rd, etc)</returns>
	public static string GetOrdinalString(int number)
	{
		int lastDigit = number % 10;
		int lastTwo = number % 100;

		if (lastTwo > 10 && lastTwo < 20) {
			return number + "th";
		} else if (lastDigit == 1) {
			return number + "st";
		} else if (lastDigit == 2) {
			return number + "nd";
		} else if (lastDigit == 3) {
			return number + "rd";
		} else {
			return number + "th";
		}
	}

	/// <summary>
	/// Get the squared distance between this point and another point.
	/// </summary>
	/// <param name="from">the starting point of the distance.</param>
	/// <param name="to">the ending point of the distance.</param>
	/// <returns>Distance (in units) between points, squared.</returns>
	public static float DistanceSquared(this Vector3 from, Vector3 to)
	{
		Vector3 diff = to - from;
		return (diff.x * diff.x) + (diff.y * diff.y) + (diff.z * diff.z);
	}

	/// <summary>
	/// Get the squared distance between this point and another point.
	/// </summary>
	/// <param name="from">the starting point of the distance.</param>
	/// <param name="to">the ending point of the distance.</param>
	/// <returns>Distance (in units) between points, squared.</returns>
	public static float DistanceSquared(this Vector2 from, Vector2 to)
	{
		Vector2 diff = to - from;
		return (diff.x * diff.x) + (diff.y * diff.y);
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

	/// <summary>
	/// Rotate the point around the specified pivot.
	/// Source: https://answers.unity.com/questions/532297/rotate-a-vector-around-a-certain-point.html
	/// </summary>
	/// <param name="point">position to rotate</param>
	/// <param name="pivot">where to rotate it around</param>
	/// <param name="angles">euler angles to rotate it</param>
	/// <returns>Rotated point.</returns>
	public static Vector3 RotateAround(this Vector3 point, Vector3 pivot, Vector3 angles) {
		Vector3 dir = point - pivot; // get point direction relative to pivot
		dir = Quaternion.Euler(angles) * dir; // rotate it
		point = dir + pivot; // calculate rotated point
		return point; // return it
	}

	/// <summary>
	/// Rotate the point around the specified pivot.
	/// Source: https://answers.unity.com/questions/532297/rotate-a-vector-around-a-certain-point.html
	/// </summary>
	/// <param name="point">position to rotate</param>
	/// <param name="pivot">where to rotate it around</param>
	/// <param name="angles">z-axis degrees of rotation</param>
	/// <returns>Rotated point.</returns>
	public static Vector2 RotateAround(this Vector2 point, Vector2 pivot, float angle) {
		Vector3 newPos = (new Vector3(point.x, point.y, 0f)).RotateAround(pivot, new Vector3(0f, 0f, angle));
		point = newPos;
		return point;
	}

	/// <summary>
	/// If the length of this vector is greater than the provided maximum, cap it
	/// at that length, but maintain the direction.
	/// </summary>
	/// <param name="srcVec">Vector to cap length.</param>
	/// <param name="maxLength">Length to cap the vector to.</param>
	/// <returns>Vector capped to length of maxLength.</returns>
	public static Vector3 CapMagnitude(this Vector3 srcVec, float maxLength)
	{
		if (maxLength < 0f) {
			Debug.LogError ("CapMagnitude was passed a negative max length: " + maxLength);
			return srcVec;
		}

		if (srcVec.sqrMagnitude <= (maxLength * maxLength)) {
			return srcVec;
		}

		return srcVec.normalized * maxLength;
	}

	/// <summary>
	/// If the length of this vector is greater than the provided maximum, cap it
	/// at that length, but maintain the direction.
	/// </summary>
	/// <param name="srcVec">vector to cap.</param>
	/// <param name="maxLength">max length of the vector to be returned.</param>
	/// <returns>Vector capped to length of maxLength</returns>
	public static Vector2 CapMagnitude(this Vector2 srcVec, float maxLength)
	{
		if (maxLength < 0f) {
			Debug.LogError ("CapMagnitude was passed a negative max length: " + maxLength);
			return srcVec;
		}

		if (srcVec.sqrMagnitude <= (maxLength * maxLength)) {
			return srcVec;
		}

		return srcVec.normalized * maxLength;
	}

	/// <summary>
	/// Returns true pctChance percent of the time.  Returns false the rest of the time.
	/// </summary>
	/// <param name="pctChance">percentage (0.0-1.0) that this function will return true.</param>
	/// <returns></returns>
	public static bool RandChance(float pctChance)
	{
		return UnityEngine.Random.Range (0.0f, 1.0f) < pctChance;
	}

	/// <summary>
	/// Generates a random forward vector witin a specified range.
	/// </summary>
	/// <param name="angleCenter">The vector at the center of all possible output.</param>
	/// <param name="angleRange">The maximum range off center of the output.</param>
	/// <returns>A random direction vector.</returns>
	public static Vector3 RandDirection(Vector3 angleCenter, float angleRange = 360.0f)
	{
		if (angleCenter.sqrMagnitude == 0f) {
			angleCenter = Vector3.forward;
		}

		float halfAngle = angleRange * 0.5f;
		angleCenter = Quaternion.AngleAxis (UnityEngine.Random.Range(-halfAngle, halfAngle), Vector3.forward) * angleCenter;
		angleCenter = Quaternion.AngleAxis (UnityEngine.Random.Range(-halfAngle, halfAngle), Vector3.right) * angleCenter;

		return angleCenter;
	}

	/// <summary>
	/// Destroys a GameObject after a delay.
	/// </summary>
	/// <param name="gObj">Gameobject to destroy.</param>
	/// <param name="timeSec">Time in seconds after which to destroy it.</param>
	public static void DestroyAfterTime(this GameObject gObj, float timeSec) {
		DestroyAfterTime dat = gObj.AddComponent<DestroyAfterTime> ();
		dat.lifetime = timeSec;
	}

	public static void DoAfterTime(this GameObject gameObject, float timeSec, DoAfterTime.DelayedAction action) {
		DoAfterTime dat = gameObject.AddComponent<DoAfterTime>();
		dat.lifetime = timeSec;
		dat.action = action;
	}

	/// <summary>
	/// Waits for button(s) press.  Has optional timeout.
	/// </summary>
	/// <param name="buttons">if any of these buttons is pressed, wait will expire.</param>
	/// <param name="timeout">if this time passes, wait will expire.</param>
	/// <returns>Coroutine enumerator.</returns>
	public static IEnumerator WaitForButtonDown(string[] buttons, float timeout = -1.0f)
	{
		GameObject o = new GameObject ();
		WaitForButtonPress w = o.AddComponent<WaitForButtonPress> ();
		w.buttons = buttons;
		w.waitTime = timeout;

		while (!w.expired) {
			yield return new WaitForEndOfFrame ();
		}

		GameObject.Destroy (o);
	}

	/// <summary>
	/// Advances a value at a specified speed, stopping it once it reaches that value.
	/// </summary>
	/// <param name="val">current value</param>
	/// <param name="goal">goal value</param>
	/// <param name="speed">rate of change toward the goal</param>
	/// <returns>Value advanced one frame toward the goal.</returns>
	public static float AdvanceValue( float val, float goal, float speed) {
		if (val < goal) {
			val += speed * Time.deltaTime;
			val = val > goal ? goal : val;
		} else if (val > goal) {
			val -= speed * Time.deltaTime;
			val = val < goal ? goal : val;
		}
		return val;
	}

	// Call this every frame on a transform to turn it such that the forward-vector will
	// eventually face focalPoint.
	//
	// t: transform to rotate toward focalPoint.
	// focalPoint: point which you're rotating toward.
	// degreesPerSec: rate of the turn in degrees/second (default: -1, immediate).
	//
	// returns true when it is facing the object.
	public static bool TurnToPoint(this Transform t, Vector3 focalPoint, float degreesPerSec = -1f)
	{
		// Position of turning object.
		Vector3 pos = t.position;

		// Vector from turning object to its target.
		Vector3 toFocalPoint = focalPoint - pos;

		// Current angle of turning object.
		Vector3 startAngles = t.eulerAngles;

		Quaternion targetRotation = Quaternion.LookRotation (toFocalPoint, t.up);
		Vector3 targetAngles = targetRotation.eulerAngles;

		if (degreesPerSec >= 0f)
			t.eulerAngles = AdvanceEulerAngles (t.eulerAngles, targetAngles, degreesPerSec);
		else
			t.eulerAngles = targetAngles;

		return t.eulerAngles == targetAngles;
	}

	// Advances a set of euler angles at a specified speed, stopping once it reaches the specified angles.
	//
	// val: current angles (degrees)
	// goal: goal angle (degrees)
	// speed: speed of change (in degrees/second)
	//
	public static Vector3 AdvanceEulerAngles( Vector3 val, Vector3 goal, float speed)
	{
		return new Vector3 (
			AdvanceAngle (val.x, goal.x, speed),
			AdvanceAngle (val.y, goal.y, speed),
			AdvanceAngle (val.z, goal.z, speed)
		);
	}

	// Advances an angle at a specified speed, stopping once it reaches the specified angle.
	//
	// val: current angle (degrees)
	// goal: goal angle (degrees)
	// speed: speed of change (in degrees/second)
	//
	public static float AdvanceAngle( float val, float goal, float speed)
	{
		float diff = Mathf.DeltaAngle (val, goal);
		float maxChange = speed * Time.deltaTime;

		if (Mathf.Abs (diff) <= maxChange) {
			val = goal;
		} else {
			val = val + (Mathf.Sign (diff) * maxChange);
		}
		return val;
	}

	// Lerps from one set of Euler angles to another.
	//
	// a: The start angles.
	// b: The end angles.
	// pct: The position [0,1] along the lerp.
	//
	public static Vector3 LerpEulerAngles( Vector3 a, Vector3 b, float pct)
	{
		return new Vector3 (
			Mathf.LerpAngle (a.x, b.x, pct),
			Mathf.LerpAngle (a.y, b.y, pct),
			Mathf.LerpAngle (a.z, b.z, pct));
	}

	/// <summary>
	/// Advances a vector at a specified speed, stopping it once it reaches the goal value.
	/// </summary>
	/// <param name="val">current value</param>
	/// <param name="goal">goal value</param>
	/// <param name="speed">rate of change toward the goal</param>
	/// <returns>Value advanced one frame toward the goal</returns>
	public static Vector2 AdvanceValue( Vector2 val, Vector2 goal, float speed ) {
		return new Vector2( AdvanceValue(val.x, goal.x, speed), AdvanceValue(val.y, goal.y, speed));
	}

	/// <summary>
	/// Advances a vector at a specified speed, stopping it once it reaches the goal value.
	/// </summary>
	/// <param name="val">current value</param>
	/// <param name="goal">goal value</param>
	/// <param name="speed">rate of change toward the goal</param>
	/// <returns>Value advanced one frame toward the goal</returns>
	public static Vector3 AdvanceValue( Vector3 val, Vector3 goal, float speed ) {
		return new Vector3( AdvanceValue(val.x, goal.x, speed), AdvanceValue(val.y, goal.y, speed), AdvanceValue (val.z, goal.z, speed));
	}

	/// <summary>
	/// Advances a color value at a specified speed, stopping it once it reaches the goal color.
	/// </summary>
	/// <param name="val">current value</param>
	/// <param name="goal">goal value</param>
	/// <param name="speed">rate of change toward the goal</param>
	/// <returns>Value advanced one frame toward the goal</returns>
	public static Color AdvanceValue( Color val, Color goal, float speed ) {
		return new Color( AdvanceValue(val.r,goal.r,speed), AdvanceValue(val.g, goal.g, speed), AdvanceValue(val.b, goal.b, speed), AdvanceValue(val.a, goal.a, speed));
	}

	// Lerp a color.
	//
	// from: starting color, returned if pct is 0.0
	// to: ending color, returned if pct is 1.0
	// pct: percentage (0.0-1.0) along the continuum between from and to.
	//
	/// <summary>
	/// Lerps from one color to
	/// </summary>
	/// <param name="from"></param>
	/// <param name="to"></param>
	/// <param name="pct"></param>
	/// <returns></returns>
	public static Color Lerp( Color from, Color to, float pct )
	{
		return new Color (Mathf.Lerp (from.r, to.r, pct), Mathf.Lerp (from.g, to.g, pct), Mathf.Lerp (from.b, to.b, pct), Mathf.Lerp (from.a, to.a, pct));
	}

	// Lerp a float, continuing beyond 100%.
	//
	// from: starting value, returned if pct is 0.0
	// to: ending value, returned if pct is 1.0
	// pct: percentage (0.0-inf) along the continuum between from and to
	//
	public static float LerpUnbounded(float from, float to, float pct)
	{
		return from + ((to - from) * pct);
	}

	// Lerp a vector, continuing beyond 100%.
	//
	// from: starting value, returned if pct is 0.0
	// to: ending value, returned if pct is 1.0
	// pct: percentage (0.0-inf) along the continuum between from and to
	//
	public static Vector2 LerpUnbounded(Vector2 from, Vector2 to, float pct)
	{
		return from + ((to - from) * pct);
	}

	// Lerp a vector, continuing beyond 100%.
	//
	// from: starting value, returned if pct is 0.0
	// to: ending value, returned if pct is 1.0
	// pct: percentage (0.0-inf) along the continuum between from and to
	//
	public static Vector3 LerpUnbounded(Vector3 from, Vector3 to, float pct)
	{
		return from + ((to - from) * pct);
	}

	// Smooth a value between 0 and 1 to be used for smooth lerping.
	//
	// pct: Linear percentage.
	// oscillate: False to cap between 0 and 1, True to oscillate values beyond range of 0-1.
	//
	private static float SmoothValue(float pct, bool oscillate = false)
	{
		if (!oscillate) {
			pct = Mathf.Clamp01 (pct);
		}

		// Bump the curve above the x axis, squish it to half, and invert it.
		return ((-1f * Mathf.Cos(pct * Mathf.PI)) + 1f) * 0.5f;
	}

	// Lerp from one value to another using a smoothed lerp to ease departure and approach.
	//
	// from: starting value, returned if pct is 0.0
	// to: ending value, returned if pct is 1.0
	// pct: percentage (0.0-1.0) along the continuum between from and to
	// oscillate: True if values beyond 0-1 should oscillate back to where they started.
	//
	public static float LerpSmooth(float from, float to, float pct, bool oscillate = false)
	{
		float smoothed = SmoothValue (pct, oscillate);
		return SloneUtil.LerpUnbounded (from, to, smoothed);
	}

	// Lerp from one vector to another using a smoothed lerp to ease departure and approach.
	//
	// from: starting value, returned if pct is 0.0
	// to: ending value, returned if pct is 1.0
	// pct: percentage (0.0-1.0) along the continuum between from and to
	// oscillate: True if values beyond 0-1 should oscillate back to where they started.
	//
	public static Vector2 LerpSmooth(Vector2 from, Vector2 to, float pct, bool oscillate = false)
	{
		float smoothed = SmoothValue (pct, oscillate);
		return SloneUtil.LerpUnbounded (from, to, smoothed);
	}

	// Lerp from one vector to another using a smoothed lerp to ease departure and approach.
	//
	// from: starting value, returned if pct is 0.0
	// to: ending value, returned if pct is 1.0
	// pct: percentage (0.0-1.0) along the continuum between from and to
	// oscillate: True if values beyond 0-1 should oscillate back to where they started.
	//
	public static Vector3 LerpSmooth(Vector3 from, Vector3 to, float pct, bool oscillate = false)
	{
		float smoothed = SmoothValue (pct, oscillate);
		return SloneUtil.LerpUnbounded (from, to, smoothed);
	}

	const int MINUTE_MASK = 0x3f;		// 6 bits (max 63)
	const int HOUR_MASK = 0x7c0;		// 5 bits (max 31)
	const int DAY_MASK = 0xf800;		// 5 bits (max 31)
	const int MONTH_MASK = 0xf0000;		// 4 bits (max 16)
										// year = 8 bits (max 4095)

	const int HOUR_SHIFT = 6;
	const int DAY_SHIFT = 11;
	const int MONTH_SHIFT = 16;
	const int YEAR_SHIFT = 20;

	// Pack a date into an integer.  Accurate to within a minute.
	//
	// NOTE: can accurately sort using these integers.
	//
	// t: time to pack into the integer
	//
	public static int PackDate( System.DateTime t )
	{
		int minute = t.Minute;
		int hour = t.Hour << HOUR_SHIFT;
		int day = t.Day << DAY_SHIFT;
		int month = t.Month << MONTH_SHIFT;
		int year = t.Year << YEAR_SHIFT;

		return minute | hour | day | month | year;
	}

	// Unpack a date packed with PackDate.
	//
	// i: integer that has been created from a packed date
	//
	public static System.DateTime UnpackDate( int i )
	{
		int minute = i & MINUTE_MASK;
		int hour = (i & HOUR_MASK) >> HOUR_SHIFT;
		int day = (i & DAY_MASK) >> DAY_SHIFT;
		int month = (i & MONTH_MASK) >> MONTH_SHIFT;
		int year = i >> YEAR_SHIFT;
		return new System.DateTime (year, month, day, hour, minute, 0);
	}

	// Shuffles an array (in place).
	//
	// shufflePasses: the number of times we should shuffle the array.
	//
	public static void ShuffleArray<T>(T[] array, int shufflePasses = 1)
	{
		for (int i = 0; i < shufflePasses; i++) {
			for (int j = 0; j < array.Length; j++) {
				int other = UnityEngine.Random.Range (0, array.Length - 1);
				if (other != j) {
					T o = array [j];
					array [j] = array [other];
					array [other] = o;
				}
			}
		}
	}

	// Get an enum from the matching string.
	//
	// value: string value matching an enum value in T
	//
	public static T ParseEnum<T>(string value)
	{
		return (T) Enum.Parse(typeof(T), value, true);
	}

	// Instantiate one object as a child of another.
	//
	// parentTransform: The transform to which the new object should be parented.
	// prefab: the prefab from which the object should be created.
	//
	public static GameObject InstantiateChild(Transform parentTransform, GameObject prefab)
	{
		GameObject o = GameObject.Instantiate (prefab, parentTransform.position, parentTransform.rotation) as GameObject;
		o.transform.SetParent (parentTransform);
		return o;
	}

	// Instantiate one object as a child of another.
	//
	// parentObj: The object to which the new object should be parented.
	// prefab: the prefab from which the object should be created.
	//
	public static GameObject InstantiateChild(GameObject parentObj, GameObject prefab)
	{
		return InstantiateChild (parentObj.transform, prefab);
	}

	// Get the viewable width and height (x, y) at the specified distance from the camera.
	//
	// distance: units from camera at which we're checking the view size.
	// cam: Camera on which to check the viewable size.  Defaults to main camera.
	//
	public static Vector2 GetViewportSizeAtDistance(float distance, Camera cam = null)
	{
		if (distance < 0f) {
			Debug.LogError ("Cannot get viewport size at negative distance.");
			return Vector2.zero;
		}

		if (cam == null) {
			cam = Camera.main;
		}

		if (cam.orthographic) {
			return 2f * ((Vector2.up * cam.orthographicSize) + (cam.aspect * cam.orthographicSize * Vector2.right));
		}

		float height = 2.0f * distance * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
		float width = height * cam.aspect;
		return new Vector2 (width, height);
	}

	// Projects a point onto a different camera plane.
	//
	// fromPoint: the point to project.
	// toDistance: the distance from the camera to the new camera plane.
	// cam: the camera whose planes we're using (defaults to Camera.main).
	//
	public static Vector3 ProjectPointToNewCameraPlane(Vector3 fromPoint, float toDistance, Camera cam = null)
	{
		if (cam == null) {
			cam = Camera.main;
		}

		// No math for orthographic.
		if (cam.orthographic) {
			return fromPoint;
		}

		float projectionScalar = GetCameraPlaneProjectionScalar (fromPoint, toDistance, cam);
		Vector3 toPoint = fromPoint - cam.transform.position;

		Vector3 rVec = cam.transform.right.normalized;
		Vector3 uVec = cam.transform.up.normalized;
		Vector3 fVec = cam.transform.forward.normalized;

		float x = Vector3.Dot (toPoint, rVec) * projectionScalar;
		float y = Vector3.Dot (toPoint, uVec) * projectionScalar;

		return cam.transform.position + (x * rVec) + (y * uVec) + (toDistance * fVec);
	}

	// Gets the multiplier used to project a point or scale from one camera distance plane to another.
	//
	// fromPoint: The point being projected.
	// toDistance: The distance from the camera onto which we're projecting.
	// cam: the camera whose planes we're using (defaults to Camera.main).
	//
	public static float GetCameraPlaneProjectionScalar(Vector3 fromPoint, float toDistance, Camera cam = null)
	{
		if (cam == null) {
			cam = Camera.main;
		}

		// Project it onto the forward vector so that we're using the distance to the plane, rather
		// than the distance to the point.
		//
		float fromDistance = Vector3.Dot ((fromPoint - cam.transform.position), cam.transform.forward.normalized);

		if (fromDistance == 0f) {
			return 0f;
		}

		return toDistance / fromDistance;
	}

	// Returns true if the point is on screen.  Has optional variable to allow an overlap
	// to detect things that are almost on screen.
	//
	// pt: The point to check whether it's onscreen.
	// beyondPct: The percentage beyond the edge of the screen that we still consider onscreen.
	//
	public static bool IsPointOnscreen(this Camera cam, Vector3 pt, float beyondPct = 0f)
	{
		Vector3 screenPoint = cam.WorldToViewportPoint(pt);

		// Check x and y.
		for (int i = 0; i < 2; i++) {
			if (screenPoint [i] > 1f + beyondPct || screenPoint [i] < -beyondPct) {
				return false;
			}
		}

		return true;
	}

	// Get the last item in a list.
	//
	public static T Last<T>(this List<T> list)
	{
		if (list.Count == 0) {
			return default(T);
		}

		return list [list.Count - 1];
	}

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

	// Creates a new component on the object copied from original.
	//
	// original: the original component.
	// destination: the object onto which we're instantiating the component.
	//
	// returns: new component.
	//
	public static T CopyNewComponent<T>(T original, GameObject destination) where T : Component
	{
		System.Type type = original.GetType();
		Component copy = destination.AddComponent(type);
		System.Reflection.FieldInfo[] fields = type.GetFields();
		foreach (System.Reflection.FieldInfo field in fields)
		{
			field.SetValue(copy, field.GetValue(original));
		}
		return copy as T;
	}

	// Moves the object to the camera such that it's lined up by the refTransform being where
	// the camera is.
	//
	// t: The transform to move.
	// refTransform: the transform that we use to align to the camera.
	// cam: The camera to use.  Camera.main by default.
	//
	public static void MoveToCameraByReference(Transform t, Transform refTransform, Camera cam = null)
	{
		if (cam == null) {
			cam = Camera.main;
		}

		// This is kind of a dumb way to do this, but it makes all the
		// painful math go away.
		Transform prevParent = t.parent;

		tempObject.transform.position = refTransform.position;
		tempObject.transform.rotation = refTransform.rotation;

		t.SetParent (tempObject.transform);
		tempObject.transform.position = cam.transform.position;
		tempObject.transform.rotation = cam.transform.rotation;

		t.SetParent (prevParent);
	}

	// Check if two values are equal with a tolerance level.
	//
	// val1: first value
	// val2: second value
	// epsilon: maximum acceptable difference between the two.
	//
	public static bool Equals(float val1, float val2, float epsilon)
	{
		return Mathf.Abs (val2 - val1) < epsilon;
	}

	/// <summary>
	/// Take a grayscale color and add hue and saturation from a full-color to it.
	/// </summary>
	/// <returns>fullColor with alpha and brightness from bwColor.</returns>
	/// <param name="bwColor">Grayscale color from which to get alpha and brightness.</param>
	/// <param name="fullColor">Full color from which to get hue and saturation.</param>
	/// <param name="saturation">Negative to use saturation from original color, non-negative to set explicitly.null</param>
	public static Color ConvertColorFromGrayscale(Color bwColor, Color fullColor, float saturation = -1f)
	{
		float fullH, fullS, fullV;
		Color.RGBToHSV (fullColor, out fullH, out fullS, out fullV);

		float h, s, v;
		Color.RGBToHSV (bwColor, out h, out s, out v);

		float finalS = saturation >= 0f ? saturation : fullS;

		Color newColor = Color.HSVToRGB (fullH, finalS, v);
		newColor.a = bwColor.a;

		return newColor;
	}

	public static string[][] ROMAN_NUMERALS = new string[][]
	{
		new string[]{"", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX"}, // ones
		new string[]{"", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC"}, // tens
		new string[]{"", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM"}, // hundreds
		new string[]{"", "M", "MM", "MMM"} // thousands
	};

	/// <summary>
	/// Convert a number from integer to roman numeral string.
	/// Adapted from https://stackoverflow.com/questions/7040289/converting-integers-to-roman-numerals
	/// </summary>
	/// <param name="number">Integer to convert to roman numeral string.</param>
	/// <returns>String of roman numerals representing the value.</returns>
	public static string ToRomanNumeral(this int number)
	{
        // split integer string into array and reverse array
		char[] intArr = number.ToString().ToCharArray();
		Array.Reverse(intArr);
        int len = intArr.Length;
        string romanNumeral = "";
        int i = len;

        // starting with the highest place (for 3046, it would be the thousands
        // place, or 3), get the roman numeral representation for that place
        // and add it to the final roman numeral string
        while (i-- > 0)
        {
            romanNumeral += ROMAN_NUMERALS[i][Int32.Parse(intArr[i].ToString())];
        }

        return romanNumeral;
	}

	/// <summary>
	/// Convert from snake case to upper case.
	/// </summary>
	/// <param name="s">String to convert.</param>
	/// <param name="delimiter">Space by default, specify to change what's between words.</param>
	/// <returns></returns>
	public static string SnakeCaseToUppercase(this string s, string delimiter = " ") {
		string[] strings = s.Split(new [] {"_"}, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < strings.Length; i++) {
			strings[i] = char.ToUpperInvariant(strings[i][0]) + strings[i].Substring(1);
		}
		return String.Join(" ", strings);
	}
}

