/************************************************************
 * 
 *                  Detach from Parent
 *                 2017 Slonersoft Games
 * 
 * Can be used to detach an object from its parent immediately
 * upon spawning.
 * 
 * Allows the child to live on after the parent has been destroyed.
 * 
 ************************************************************/

using UnityEngine;
using System.Collections;

public class DetachFromParent : MonoBehaviour {
	void Start () {
		transform.SetParent (null);
	}
}
