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
				DoSpawn(data != null ? (data as BlipDamage).GetAttackerObj() : null);
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

			// If this is a damager that got killed by a warrior, they now own all damage done by this and subsequent damagers.
			WeaponDamager originalDamager = GetComponent<WeaponDamager>();
			Warrior killerWarrior = originalDamager != null && killer != null ? killer.GetComponent<Warrior>() : null;
			if (originalDamager && killerWarrior) {
				originalDamager.owner = killerWarrior;
			}

			Quaternion spawnOrient = transform.rotation;
			for (int i = 0; i < numToSpawn; i++) {
				Vector3 spawnPosition = transform.position;

				if (spawnPositions.Length > 0) {
					spawnPosition = spawnPositions[i%spawnPositions.Length].position;
					spawnOrient = spawnPositions[i%spawnPositions.Length].rotation;
				} else {
					float pct = (float)i / (float)(numToSpawn - (fullcircle ? 0 : 1));
					float rotateAngle = numToSpawn <= 1 ? 0 : Mathf.Lerp(minAngle, maxAngle, pct);
					if (Slonersoft.SloneUtil.WarKit.WarKitSettings.is2D()) {
						spawnOrient = transform.rotation * Quaternion.Euler(0f, 0f, rotateAngle);
					} else {
						spawnOrient = transform.rotation * Quaternion.Euler(transform.up * rotateAngle);
					}

					if (spawnRadius > 0f) {
						spawnPosition += spawnRadius * (spawnOrient * Vector3.forward);
					}
				}

				GameObject o = Instantiate (spawnObject, spawnPosition, spawnOrient);
				gameObject.SendBlip(Blip.Type.CREATED, new BlipCreate(o));
			}

			// don't repeat it.
			enabled = false;
		}
	}
}