/************************************************************
 * 
 *                    Gamewide Prefabs
 *                 2016 Slonersoft Games
 * 
 * Create a list of prefabs that should be accessible from anywhere
 * in the game.  Create a prefab from that object, and place it
 * in each scene where the list should be accessible.
 *
 * Access the prefabs with:
 * GameWidePrefabs.inst.GetPrefab("Prefab Name")
 * 
 ************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GamewidePrefabs : MonoBehaviour {

	public GameObject[] prefabList;
	private Dictionary<string, GameObject> dictionary = new Dictionary<string, GameObject> ();

	static GamewidePrefabs _inst;
	public static GamewidePrefabs inst {
		get {
			if (_inst == null) {
				_inst = GameObject.FindObjectOfType<GamewidePrefabs> ();
				_inst.Initialize ();
			}
			return _inst;
		}
	}

	private void Initialize()
	{
		if (dictionary.Count != 0) {
			return;
		}

		foreach (GameObject o in prefabList) {
			dictionary.Add (o.name, o);
		}
	}

	// Retrieve a gamewide prefab from the list.
	// 
	// prefabName: the name in the inspector assigned to the child prefab.
	// 
	public GameObject GetPrefab(string prefabName)
	{
		if (dictionary.ContainsKey (prefabName)) {
			return dictionary [prefabName];
		} else {
			return null;
		}
	}
}
