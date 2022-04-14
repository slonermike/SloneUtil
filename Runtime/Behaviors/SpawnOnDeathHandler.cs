using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Override this to perform special behavior when spawned via death.
/// </summary>
public abstract class SpawnOnDeathHandler : MonoBehaviour {
	public abstract void OnSpawn(GameObject parent, GameObject parentKiller);
}
