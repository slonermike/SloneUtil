using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Slonersoft.SloneUtil.Core;

namespace Slonersoft.SloneUtil.WarKit {
	public class Health : Damageable {

		private float _maxHealth;
		public float maxHealth {
			get {
				return _maxHealth;
			}
		}

		public float health = 100.0f;
		public float healRate = 0f;

		protected override void Start()
		{
			base.Start ();
			_maxHealth = health;
		}

		public override float GetHealth()
		{
			return health;
		}

		public override float GetMaxHealth()
		{
			return maxHealth;
		}

		protected override void Update()
		{
			base.Update ();
			if (healRate > 0f && health < maxHealth) {
				health = CoreUtils.AdvanceValue (health, maxHealth, healRate);
			}
		}

		// Returns true if dead.  False if still living.
		public override bool DoDamage(BlipDamage data)
		{
			if (invulnerable) {
				data = new BlipDamage(data, 0f);
			}

			// Do not keep doing damage after death.  If we remove this, be sure to track
			// the killer separately from the attacker.
			//
			if (health <= 0.0f) {
				return true;
			}

			health -= data.damageAmount;

			OnDamageTaken (data);

			bool died = health <= 0.0f;

			if (died) {
				Die (data);
			}

			return died;
		}

		public override void Die(BlipDamage data)
		{
			health = 0.0f;
			base.Die (data);
		}
	}

}
