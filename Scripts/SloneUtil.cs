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
	// Get the ordinal string (1st, 2nd, 3rd, etc) associated with an integer.
	//
	// number: integer number to convert to an ordinal string.
	//
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

	// Get the squared distance between this point and another point.
	//
	// from: the starting point of the distance.
	// to: the ending point of the distance.
	//
	public static float DistanceSquared(this Vector3 from, Vector3 to)
	{
		Vector3 diff = to - from;
		return (diff.x * diff.x) + (diff.y * diff.y) + (diff.z * diff.z);
	}

	// Get the squared distance between this point and another point.
	//
	// from: the starting point of the distance.
	// to: the ending point of the distance.
	//
	public static float DistanceSquared(this Vector2 from, Vector2 to)
	{
		Vector2 diff = to - from;
		return (diff.x * diff.x) + (diff.y * diff.y);
	}

	// Rotate the position around an arbitrary axis.
	//
	// position: the position to do the rotating
	// center: the center around which to rotate
	// axis: the axis around which to rotate.
	// angle: the angle of rotation in degrees.
	//
	public static Vector3 RotateAround(this Vector3 position, Vector3 center, Vector3 axis, float angle)
	{
		Quaternion rot = Quaternion.AngleAxis (angle, axis);
		Vector3 dir = position - center;
		return rot * dir;
	}


	// Returns true if xform is ahead of target (According to transform/fwd vector)
	//
	// xform: source transform
	// target: target target
	// 
	public static bool IsAheadOf(this Transform xform, Transform target) {
		return Vector3.Dot (xform.position - target.position, target.forward) > 0.0f;
	}
	
	// Returns true if ahead is ahead of behind (According to transform/fwd vector)
	//
	// obj: source object
	// target: target object
	// 
	public static bool IsAheadOf(this GameObject obj, GameObject target)  {
		return obj.transform.IsAheadOf (target.transform);
	}

	// If the length of this vector is greater than the provided maximum, cap it
	// at that length, but maintain the direction.
	//
	// srcVec (Vector3): vector to cap.
	// maxLength (float): max length of the vector to be returned.
	//
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

	// If the length of this vector is greater than the provided maximum, cap it
	// at that length, but maintain the direction.
	//
	// srcVec (Vector3): vector to cap.
	// maxLength (float): max length of the vector to be returned.
	//
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

	// Returns true pctChance percent of the time.  Returns false the rest of the time.
	//
	// pctChance: percentage (0.0-1.0) that this function will return true.
	// 
	public static bool RandChance(float pctChance)
	{
		return UnityEngine.Random.Range (0.0f, 1.0f) < pctChance;
	}

	// Destroys a GameObject after a delay.
	//
	// gObj: Gameobject to destroy.
	// timeSec: Time in seconds after which to destroy it.
	// 
	public static IEnumerator DestroyAfterTime(this GameObject gObj, float timeSec) {
		yield return new WaitForSeconds (timeSec);
		GameObject.Destroy(gObj);
	}

	// Waits for button(s) press.  Has optional timeout.
	//
	// buttons: if any of these buttons is pressed, wait will expire.
	// timeout (optional): if this time passes, wait will expire.
	//
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

	// Advances a value at a specified speed, stopping it once it reaches that value.
	//
	// val: current value
	// goal: goal value
	// speed: rate at which to move toward goal.
	//
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

	// Moves a vector at a specified speed, stopping it once it reaches that value.
	//
	// val: current value
	// goal: goal value
	// speed: rate at which to move toward goal.
	//
	public static Vector2 AdvanceValue( Vector2 val, Vector2 goal, float speed ) {
		return new Vector2( AdvanceValue(val.x, goal.x, speed), AdvanceValue(val.y, goal.y, speed));
	}

	// Moves a vector at a specified speed, stopping it once it reaches that value.
	//
	// val: current value
	// goal: goal value
	// speed: rate at which to move toward goal.
	//
	public static Vector3 AdvanceValue( Vector3 val, Vector3 goal, float speed ) {
		return new Vector3( AdvanceValue(val.x, goal.x, speed), AdvanceValue(val.y, goal.y, speed), AdvanceValue (val.z, goal.z, speed));
	}

	// Moves a color at a specified speed, stopping it once it reaches the goal value.
	//
	// val: current value
	// goal: goal value
	// speed: rate at which to move toward goal.
	//
	public static Color AdvanceValue( Color val, Color goal, float speed ) {
		return new Color( AdvanceValue(val.r,goal.r,speed), AdvanceValue(val.g, goal.g, speed), AdvanceValue(val.b, goal.b, speed), AdvanceValue(val.a, goal.a, speed));
	}

	// Lerp a color.
	//
	// from: starting color, returned if pct is 0.0
	// to: ending color, returned if pct is 1.0
	// pct: percentage (0.0-1.0) along the continuum between from and to.
	// 
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
	public static Vector2 LerpUnbounded(Vector3 from, Vector3 to, float pct)
	{
		return from + ((to - from) * pct);
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
}

