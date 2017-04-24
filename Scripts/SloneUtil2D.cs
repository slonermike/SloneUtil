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

public static class SloneUtil2D
{
	// Get axis-aligned bounds of the camera.
	// If camera is not vertical, may include areas not actually onscreen.
	//
	// For orthographic cameras only.
	//
	// cam: the camera to use for the check (default: Camera.main).
	//
	public static void GetCameraBounds (out Vector2 bbmin, out Vector2 bbmax, Camera cam = null)
	{
		if (cam == null) {
			cam = Camera.main;
		}

		if (!cam.orthographic) {
			Debug.LogError ("GetCameraBounds2D: Attempting to get 2d camera bounds of non-ortho camera.");
			bbmin = Vector2.zero;
			bbmax = Vector2.zero;
			return;
		}

		bbmin = -Vector2.one;
		bbmax = Vector2.one;

		if (!cam.orthographic) {
			Debug.LogError ("Attempting to get bounds of non-orthographic camera.");
			return;
		}

		Vector2 halfSize = GetCameraSize (cam) * 0.5f;
		Vector3 cameraCenter = cam.transform.position;

		Vector3 topRight = cameraCenter + (cam.transform.up * halfSize.y) + (cam.transform.right * halfSize.x);
		Vector3 bottomLeft = cameraCenter - (cam.transform.up * halfSize.y) - (cam.transform.right * halfSize.x);

		bbmin = new Vector2 (Mathf.Min (topRight.x, bottomLeft.x), Mathf.Min (topRight.y, bottomLeft.y));
		bbmax = new Vector2 (Mathf.Max (topRight.x, bottomLeft.x), Mathf.Max (topRight.y, bottomLeft.y));
	}

	// Get the width and height (x, y) of the camera.
	//
	// For orthographic cameras only.
	//
	// cam: the camera to use for the check (default: Camera.main).
	//
	public static Vector2 GetCameraSize(Camera cam = null)
	{
		if (!cam.orthographic) {
			Debug.LogError ("GetCameraSize2D: Attempting to get 2d camera bounds of non-ortho camera.");
			return Vector2.zero;
		}

		if (cam == null) {
			cam = Camera.main;
		}

		return 2.0f * (new Vector2(
			cam.orthographicSize * Screen.width / Screen.height,
			cam.orthographicSize
		));
	}

	// Get the world position of the mouse.
	//
	// For orthographic cameras only.
	//
	// cam: the camera to use for the check (default: Camera.main).
	//
	public static Vector2 GetMouseWorldPos(Camera cam = null)
	{
		if (!cam.orthographic) {
			Debug.LogError ("GetMouseWorldPos2D: Attempting to get 2d mouse position of non-ortho camera.");
			return Vector2.zero;
		}

		return GetWorldPosFromScreen (Input.mousePosition, cam);
	}

	// Get a position in the world from a screen position.
	//
	// For orthographic cameras only.
	//
	// screenPos: the position on the screen.
	// cam: the camera to use for the check (default: Camera.main).
	//
	public static Vector2 GetWorldPosFromScreen(Vector2 screenPos, Camera cam = null)
	{
		if (!cam.orthographic) {
			Debug.LogError ("GetWorldPosFromScreen2D: Attempting to get 2d world position of non-ortho camera.");
			return Vector2.zero;
		}

		if (cam == null) {
			cam = Camera.main;
		}

		Vector2 pos = cam.ScreenToWorldPoint (screenPos);
		return pos;
	}

	// Returns true if the specified point is on-screen.  False if off-screen.
	//
	// For orthographic cameras only.
	//
	// cam: defaults to Camera.main.
	public static bool IsPointOnscreen(Vector2 point, Camera cam = null)
	{
		if (!cam.orthographic) {
			Debug.LogError ("IsPointOnscreen2D: Attempting to check 2d on-screen status of point on of non-ortho camera.");
			return false;
		}

		if (cam == null) {
			cam = Camera.main;
		}

		Vector2 camSize = GetCameraSize ();
		Vector2 camPos = cam.transform.position; // Have to convert to V2 first.
		Vector2 toPoint = point - camPos;

		// Off-screen vertically?
		float verticalDist = Mathf.Abs (Vector2.Dot (toPoint, cam.transform.up));
		if (verticalDist > camSize.y * 0.5f) {
			return false;
		}

		float horizontalDist = Mathf.Abs (Vector2.Dot (toPoint, cam.transform.right));
		if (horizontalDist > camSize.x * 0.5f) {
			return false;
		}

		return true;
	}

	// Call this every frame on a transform to turn it such that the right-vector will
	// eventually face focalPoint.
	//
	// t: transform to rotate toward focalPoint.
	// focalPoint: point which you're rotating toward.
	// degreesPerSec: rate of the turn in degrees/second.
	//
	// returns true when it is facing the object.
	public static bool TurnToPoint(this Transform t, Vector2 focalPoint, float degreesPerSec)
	{
		// Position of turning object.
		Vector2 pos = t.position;

		// Vector from turning object to its target.
		Vector2 toFocalPoint = focalPoint - pos;

		// Current angle of turning object.
		Vector3 startAngles = t.eulerAngles;

		// Angle between our current orient and our target orient.
		float angleToFocalPt = Vector2.Angle (t.right, toFocalPoint);

		// Determine the maximum angle we can turn this frame.
		float maxAngleChange = degreesPerSec * Time.deltaTime;

		// If we have less angle to go than what we're allowed this frame, we've arrived.
		bool arrived = maxAngleChange <= angleToFocalPt;

		// Apply a cap to the rotation, so we obey the maximum turn speed.
		angleToFocalPt = Mathf.Min (maxAngleChange, angleToFocalPt);

		// Determine whether this is a positive or negative angle.
		if (Vector2.Dot (toFocalPoint, t.up) < 0) {
			angleToFocalPt *= -1.0f;
		}

		// Apply the rotation around the z axis.
		Vector3 newAngles = new Vector3 (startAngles.x, startAngles.y, startAngles.z + angleToFocalPt);

		// Set the new angles.
		t.eulerAngles = newAngles;

		return arrived;
	}

	// Checks to see if one object is facing another in 2 dimensions.
	//
	// pos: position of the object whose heading we're checking.
	// dir: the forward vector of the object whose heading we're checking.
	// targetPos: the object we want to look at.
	// withinAngle: the "view angle" of the object whose heading we're checking (default 180).
	//
	public static bool IsFacing(Vector2 pos, Vector2 dir, Vector2 targetPos, float withinAngle = 180.0f)
	{
		Vector2 toTarget = targetPos - pos;
		float dot = Vector2.Dot (toTarget.normalized, dir);
		float goalDot = Mathf.Cos (Mathf.Deg2Rad * withinAngle * 0.5f);
		return dot > goalDot;
	}

	// Generates a random point within a 2d cone.
	//
	// angleCenter: The normalized vector from the tip of the cone to the center of its curve.
	// angle: the spread of the cone (in degrees -- 360 is a full circle).
	//
	public static Vector2 RandPointInCone(Vector2 angleCenter, float angle)
	{
		return UnityEngine.Random.Range (0.0f, 1.0f) * RandDirection (angleCenter, angle);
	}

	// Generates a random forward vector witin a specified range.
	//
	// angleCenter: The vector at the center of all possible output.
	// angleRange: The maximum range off center of the output.
	//
	public static Vector2 RandDirection(Vector2 angleCenter, float angleRange = 360.0f)
	{
		float halfAngle = angleRange * 0.5f;
		float angleOut = UnityEngine.Random.Range(-halfAngle, halfAngle);
		return Quaternion.AngleAxis (angleOut, Vector3.forward) * angleCenter;
	}

	// Get the world-space vector from this point to the nearest point onscreen that maintains
	// the current distance from the camera.
	//
	// position: position to check.
	// cam: camera on which to check (Deaults to main camera).
	//
	public static Vector2 GetVectorToOnscreen(Vector3 position, Camera cam = null)
	{
		if (cam == null) {
			cam = Camera.main;
		}

		Vector3 toPosition = position - cam.transform.position;

		float zDist = Vector3.Dot (toPosition, cam.transform.forward);

		if (zDist < 0) {
			Debug.LogError ("Cannot find distance offscreen for object behind camera.");
			return Vector2.zero;
		}

		// Get the position relative to the camera.
		float xDist = Vector3.Dot (toPosition, cam.transform.right);
		float yDist = Vector3.Dot (toPosition, cam.transform.up);

		Vector2 halfViewportSize = SloneUtil.GetViewportSizeAtDistance (zDist, cam) * 0.5f;

		// Calculate the distance from each camera-relative direction (x and y) and multiply back into world space.
		//
		Vector2 returnX = Vector2.zero;
		if (Mathf.Abs (xDist) > halfViewportSize.x) {
			float xOffscreen = -1f * Mathf.Sign (xDist) * (Mathf.Abs (xDist) - halfViewportSize.x);
			returnX = xOffscreen * cam.transform.right;
		}

		Vector2 returnY = Vector2.zero;
		if (Mathf.Abs (yDist) > halfViewportSize.y) {
			float yOffscreen = -1f * Mathf.Sign (yDist) * (Mathf.Abs (yDist) - halfViewportSize.y);
			returnY = yOffscreen * cam.transform.up;
		}

		return returnX + returnY;
	}
}