using UnityEngine;
using System.Collections;

using Slonersoft.SloneUtil.Core;
using Slonersoft.SloneUtil.BlipKit;

namespace Slonersoft.SloneUtil.WarKit {
	/// <summary>
	/// Attach this to an object, then send a "death" message to the object
	/// to spawn objects in its place.
	/// </summary>
	public class SpawnOnDeath : MonoBehaviour {

		[Tooltip("Prefab of the object to spawn")]
		public GameObject spawnObject;

		[Tooltip("Number of objects to spawn.")]
		public int numToSpawn = 1;

		[Tooltip("(optional) Position to point objects away from.")]
		public Transform pointAwayFrom;

		[Tooltip("(optional) Specific positions for spawning.")]
		public Transform[] spawnPositions;

		[Tooltip("Distance from the center where they should spawn.")]
		public float spawnRadius = 0f;

		[Tooltip("Angle at which the leftmost item spawns (away from 'right').")]
		[Range(-180,180)]
		public float minAngle = -180f;

		[Tooltip("Angle at which the rightmost item spawns (away from 'right').")]
		[Range(-180,180)]
		public float maxAngle = 180f;

		void Awake() {
			gameObject.ListenForBlips(Blip.Type.DIED, delegate(Blip data) {
				BlipDamage deathData = data as BlipDamage;
				DoSpawn(deathData.GetAttackerObj());
			});
		}

		/// <summary>
		/// Spawn the objects that appear on death.
		/// </summary>
		public void DoSpawn(GameObject killer)
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

			bool fullcircle = Mathf.DeltaAngle(minAngle, maxAngle) == 0f;

			// HACK: make it possible to rotate the offset around the center.
			if (numToSpawn > 1 && spawnRadius <= 0f) {
				spawnRadius = 0.01f;
			}

			Quaternion spawnOrient = transform.rotation;
			for (int i = 0; i < numToSpawn; i++) {
				Vector3 spawnPosition = transform.position;

				if (spawnPositions.Length > 0) {
					spawnPosition = spawnPositions[i%spawnPositions.Length].position;
					spawnOrient = spawnPositions[i%spawnPositions.Length].rotation;
				} else if (spawnRadius > 0f) {
					float pct = (float)i / (float)(numToSpawn - (fullcircle ? 0 : 1));
					float rotateAngle = numToSpawn <= 1 ? 0 : Mathf.Lerp(minAngle, maxAngle, pct);
					spawnPosition += spawnRadius * (Quaternion.Euler(0f, 0f, rotateAngle) * transform.right);
				}

				if (pointAwayFrom != null) {
					spawnOrient = CoreUtils2D.RotationFromRightVec(spawnPosition - pointAwayFrom.position);
				}

				GameObject o = Instantiate (spawnObject, spawnPosition, spawnOrient);
				SpawnOnDeathHandler handler = o.GetComponent<SpawnOnDeathHandler>();
				if (handler != null) {
					handler.OnSpawn(gameObject, killer);
				}
			}

			// don't repeat it.
			enabled = false;
		}
	}

	/// <summary>
	/// Override this to perform special behavior when spawned via death.
	/// </summary>
	public abstract class SpawnOnDeathHandler : MonoBehaviour {
		public abstract void OnSpawn(GameObject parent, GameObject parentKiller);
	}
}
