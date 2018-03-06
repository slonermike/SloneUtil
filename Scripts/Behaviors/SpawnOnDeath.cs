using UnityEngine;
using System.Collections;

/// <summary>
/// Attach this to an object, then call SpawnOnDeathUtil.DoSpawnsOnDeath(obj) on the object
/// to spawn objects in its place.
/// </summary>
public class SpawnOnDeath : MonoBehaviour {

	[Tooltip("Prefab of the object to spawn")]
	public GameObject spawnObject;

	[Tooltip("Number of objects to spawn.")]
	public int numToSpawn = 1;

	[Tooltip("(optional) Position to point objects away from.")]
	public Transform pointAwayFrom;

	/// <summary>
	/// Spawn the objects that appear on death.
	/// </summary>
	public void DoSpawn()
	{
		if (!enabled) {
			return;
		}

		if (numToSpawn <= 0) {
			Debug.LogError ("Invalid numToSpawn on SpawnOnDeath: " + numToSpawn);
			return;
		}

		if (spawnObject == null) {
			Debug.LogError ("SpawnOnDeath on " + gameObject.name + " has no spawnObject!");
			return;
		}

		Quaternion spawnOrient = transform.rotation;

		if (pointAwayFrom != null) {
			Vector3 fwd = transform.forward;
			if (transform.position != pointAwayFrom.position) {
				fwd = transform.position - pointAwayFrom.position;
				fwd.Normalize ();
			}
			spawnOrient = Quaternion.LookRotation (fwd);
		}

		for (int i = 0; i < numToSpawn; i++) {
			Instantiate (spawnObject, transform.position, spawnOrient);
		}

		// don't repeat it.
		enabled = false;
	}
}

/// <summary>
/// Manages spawning of objects on death.
/// </summary>
public static class SpawnOnDeathUtil {

	/// <summary>
	/// Find all SpawnOnDeath behaviors on the object and children, and do the spawns.
	/// </summary>
	/// <param name="g">Owner of the SpawnOnDeath behaviors.</param>
	public static void DoSpawnsOnDeath(this GameObject g)
	{
		SpawnOnDeath[] sods = g.GetComponentsInChildren<SpawnOnDeath> ();
		foreach (SpawnOnDeath sod in sods) {
			sod.DoSpawn ();
		}

		DropXP drop = g.GetComponent<DropXP> ();
		if (drop != null) {
			drop.DoDrop ();
		}
	}
}
