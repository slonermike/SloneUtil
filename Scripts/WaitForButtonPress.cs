/************************************************************
 * 
 *                 Wait for Button Press
 *                 2016 Slonersoft Games
 *
 * Will set "expired" to false when either the time expires, or
 * one of the buttons in the "buttons" array has been pressed.
 *
 * NOTE: this is intended to be created by code.  Don't add this
 * to your object in the inspector, use GameObject.AddComponent.
 *
 * If you don't know what that means, you should just use
 * SloneUtil.WaitForButtonDown and forget you ever saw this.
 * 
 ************************************************************/

using UnityEngine;
using System.Collections;

public class WaitForButtonPress : MonoBehaviour {

	public bool expired = false;
	public float waitTime = -1.0f;
	public string[] buttons;
	public string pressedButton = "";
	
	// Update is called once per frame
	void Update () {
		if (!expired) {
			if (buttons != null) {
				foreach (string b in buttons) {
					if (Input.GetButtonDown (b)) {
						pressedButton = b;
						expired = true;
					}
				}
			}
			if (waitTime >= 0) {
				waitTime -= Time.deltaTime;

				if (waitTime <= 0.0f) {
					expired = true;
				}
			}
		}
	}
}
