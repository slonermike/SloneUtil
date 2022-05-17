using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Slonersoft.SloneUtil.Core;
using Slonersoft.SloneUtil.BlipKit;

namespace Slonersoft.SloneUtil.WarKit {
	public class BlipDamage : Blip {
		public Warrior attacker;
		public WeaponDamager damager;
		public float damageAmount;
		public Vector3 damageLocation;
		public Vector3 damageNormal;

		public BlipDamage(float amount = 0f) {
			isNoOp = amount == 0f;
			damageAmount = amount;
			attacker = null;
			damager = null;
			damageLocation = Vector3.zero;
			damageNormal = new Vector3(0, 0, 1);
		}

		public BlipDamage(float amount, WeaponDamager src, Vector3 location, Vector3 normal)
		{
			isNoOp = amount == 0f;
			damageLocation = location;
			damageNormal = normal;
			damageAmount = amount;
			damager = src;
			if (damager != null) {
				attacker = damager.owner;
			} else {
				attacker = null;
			}
		}

		public BlipDamage(float amount, Warrior _attacker, Vector3 location, Vector3 normal)
		{
			isNoOp = amount == 0f;
			damageLocation = location;
			damageNormal = normal;
			damageAmount = amount;
			attacker = _attacker;
			damager = null;
		}

		public BlipDamage(BlipDamage data, float damageMultiplier = 1f)
		{
			isNoOp = data.isNoOp;
			damageLocation = data.damageLocation;
			damageNormal = data.damageNormal;
			damageAmount = data.damageAmount * damageMultiplier;
			attacker = data.attacker;
			damager = data.damager;
		}

		public GameObject GetAttackerObj()
		{
			if (attacker != null) {
				return attacker.gameObject;
			} else if (damager != null) {
				return damager.gameObject;
			} else {
				return null;
			}
		}
	}

	public abstract class Damageable : MonoBehaviour {

		public delegate void OnDeathCallback(Damageable deadGuy, BlipDamage data);
		public delegate void OnDamagedCallback(BlipDamage data);

		private static LinkedList<Damageable> damageables = new LinkedList<Damageable>();

		[Tooltip("True to destroy this object when it dies.")]
		public bool destroyOnDeath = true;

		[Tooltip("True to take damage from touching other objects/enemies.")]
		public bool damageOnTouchOthers = true;

		[Tooltip("True to automatically create damage delegates in children with colliders")]
		public bool takeDamageFromChildColliders = true;

		[Tooltip("Object to spawn at position of taken damage.")]
		public GameObject damageTakenPrefab;

		Team _team = Team.NONE;

		protected virtual Team GetTeam()
		{
			return gameObject.GetTeam ();
		}

		public Team team {
			get {
				if (_team == Team.NONE) {
					_team = GetTeam ();
				}
				return _team;
			}
		}

		public float pctHealth {
			get {
				float health = GetHealth ();
				float maxHealth = GetMaxHealth ();
				if (maxHealth > 0f) {
					return health / maxHealth;
				} else {
					return 1f;
				}
			}
		}

		// Invulnerable until "Start" gets hit.
		protected bool invulnerable = true;
		public bool startInvulnerable = false;

		protected bool _deathPending = false;
		public bool deathPending {
			get {
				return _deathPending;
			}
		}

		public abstract bool DoDamage(BlipDamage data);

		protected virtual void Awake()
		{
			invulnerable = startInvulnerable;
		}

		protected virtual void Start()
		{
			if (takeDamageFromChildColliders) {
				List<GameObject> childObjects = new List<GameObject>();
				if (WarKitSettings.is3D()) {
					Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
					foreach (Collider c in colliders) {
						childObjects.Add(c.gameObject);
					}
				} else {
					Collider2D[] colliders = gameObject.GetComponentsInChildren<Collider2D>();
					foreach (Collider2D c in colliders) {
						childObjects.Add(c.gameObject);
					}
				}

				foreach (GameObject o in childObjects) {
					DamageDelegate d = o.GetOrAddComponent<DamageDelegate>();
					if (d.master == null) {
						d.master = this;
						d.takeDamageFromChildColliders = false;
						d.damageOnTouchOthers = damageOnTouchOthers;
					}
				}
			}
		}

		protected virtual void OnEnable()
		{
			damageables.AddLast (this);
		}

		protected virtual void OnDisable()
		{
			damageables.Remove (this);
		}

		// Respond to collision with another damageable.
		public void OnCollision(GameObject o, Vector3 point, Vector3 normal) {
			Damageable d = o.GetComponent<Damageable> ();
			Warrior attacker = null;
			if (d != null) {
				attacker = d.GetWarrior ();
			} else {
				attacker = o.GetComponent<Warrior> ();
			}

			Team t;
			if (d != null) {
				t = d.GetTeam ();
			} else {
				t = o.GetTeam (false);
			}

			if (t == team || t == Team.NONE) {
				return;
			}

			DoDamage(new BlipDamage(GetHealth(), attacker, point, normal));
		}

		void OnCollisionEnter2D(Collision2D coll) {
			if (damageOnTouchOthers) {
				OnCollision(coll.gameObject, coll.contacts[0].point, coll.contacts[0].normal);
			}
		}

		void OnCollisionEnter(Collision coll) {
			Debug.Log($"{name} Entered collision with {coll.gameObject.name}");
			if (damageOnTouchOthers) {
				OnCollision(coll.gameObject, coll.contacts[0].point, coll.contacts[0].normal);
			}
		}

		public abstract float GetHealth();
		public abstract float GetMaxHealth();

		public virtual Warrior GetWarrior()
		{
			return gameObject.GetComponent<Warrior> ();
		}

		public virtual Damageable GetMaster()
		{
			return this;
		}

		protected void OnDamageTaken(BlipDamage data)
		{
			if (damageTakenPrefab) {
				GameObject blood = CoreUtils.InstantiateChild(transform, damageTakenPrefab);
				blood.transform.position = data.damageLocation;
				blood.transform.rotation = Quaternion.LookRotation(data.damageNormal, transform.up);
			}
			gameObject.SendBlip(Blip.Type.DAMAGED, data);
		}

		protected virtual void Update()
		{

		}

		public void DieAfterTime(BlipDamage data, float delay) {
			_deathPending = true;
			StartCoroutine(DieAfterTimeCoroutine(data, delay));
		}

		private IEnumerator DieAfterTimeCoroutine(BlipDamage data, float delay) {
			yield return new WaitForSeconds(delay);
			Die(data);
		}

		public virtual void Die(BlipDamage data)
		{
			gameObject.SendBlip(Blip.Type.DIED, data);

			if (destroyOnDeath) {
				GameObject.Destroy (gameObject);
			}
		}

		public bool IsVulnerable()
		{
			return !invulnerable;
		}

		public virtual void MakeVulnerable()
		{
			invulnerable = false;
		}

		public virtual void MakeInvulnerable(float invulnerableTime = -1f)
		{
			invulnerable = true;
			if (invulnerableTime >= 0f) {
				Invoke ("MakeVulnerable", invulnerableTime);
			}
		}

		public virtual bool StopUnstoppable() {
			return false;
		}

		// TODO: Split lists up by teams.
		//

		/// <summary>
		/// Find a suitable target.
		/// </summary>
		/// <param name="t">Where the check is coming from.</param>
		/// <param name="attackerTeam">The team the attacker is on.</param>
		/// <param name="angleThreshold">The full angle inside which targets are valid</param>
		/// <param name="maxDistance">The maxiumum radius in which targets are valid</param>
		/// <param name="onscreenOnly">True if only onscreen targets count.</param>
		/// <param name="ignoreNeutral">True to ignore TEAM_NONE</param>
		/// <param name="raycastCheck">True to use raycast to validate targets</param>
		/// <param name="raycastExemptTarget">Target that can ignore raycast check.</param>
		/// <returns></returns>
		public static Damageable GetTarget(
			Transform t,
			Team attackerTeam,
			float angleThreshold = 360f,
			float maxDistance = -1f,
			bool onscreenOnly = true,
			bool ignoreNeutral = false,
			bool raycastCheck = false,
			Damageable raycastExemptTarget = null
		)
		{
			float minDot = Mathf.Cos (Mathf.Deg2Rad * angleThreshold * 0.5f);
			Damageable nearest = null;
			float nearestDistSq = -1.0f;
			float maxDistanceSq = maxDistance * maxDistance;
			Vector3 pos = t.position;

			// TODO: does this need to work in 3d?
			foreach (Damageable d in damageables) {
				Vector3 dPos = d.transform.position;
				Vector3 toDamageable = dPos - pos;

				// No friendly fire.
				if (d.team == attackerTeam) {
					continue;
				}

				if (ignoreNeutral && d.team == Team.NONE) {
					continue;
				}

				// Ignore things far offscreen (if applicable).
				if (onscreenOnly && !CoreUtils.IsPointOnAnyScreen (dPos, 0.5f)) {
					continue;
				}

				Vector3 myFvec = WarKitSettings.is2D() ? t.right : t.forward;

				// Check angle threshold if appropriate.
				if (minDot >= 0f) {
					float dot = Vector3.Dot (toDamageable.normalized, myFvec);
					if (dot < minDot) {
						continue;
					}
				}

				float distSq = toDamageable.sqrMagnitude;
				if (maxDistance > 0f && distSq > maxDistanceSq) {
					continue;
				}

				// Not better than the existing best target?
				if (nearest != null && distSq >= nearestDistSq) {
					continue;
				};

				// No raycast checks for existing targets.
				if (raycastCheck && d != raycastExemptTarget) {
					if (WarKitSettings.is2D()) {
						int weaponLayerMask = Physics2D.GetLayerCollisionMask(TeamUtil.GetTeamWeaponLayer(attackerTeam));
						Vector3 toTarget = d.transform.position - t.position;
						float toTargetLength = toTarget.magnitude;

						// TODO: Make this also work in 3d.
						RaycastHit2D hit = Physics2D.Raycast(t.position, toTarget / toTargetLength, toTargetLength, weaponLayerMask);
						if (!hit) {
							continue;
						}

						Damageable hitDamageable = hit.collider.gameObject.GetComponent<Damageable>();
						if (hitDamageable == null || d != hitDamageable) {
							continue;
						}
					}
				}

				// Found a new best valid target.
				nearest = d;
				nearestDistSq = distSq;
			}

			return nearest;
		}
	}

}
