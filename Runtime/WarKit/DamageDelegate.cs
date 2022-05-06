using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slonersoft.SloneUtil.BlipKit;

namespace Slonersoft.SloneUtil.WarKit {
	// Place this below a damageable, and its damage will get passed along.
	public class DamageDelegate : Damageable {

		public float damageMultiplier = 1.0f;
		public Damageable master;
		public bool dieOnParentDeath = true;

		[Tooltip("How long after parent death that we die.")]
		public float deathDelay = 0f;

		protected override void Start()
		{
			base.Start ();
			if (master == null) {
				Debug.LogError ("DamageDelegate " + name + " has no master damageable.  Disabling.");
				enabled = false;
			} else {
					if (dieOnParentDeath) {
							master.gameObject.ListenForBlips(Blip.Type.DIED, delegate(Blip b) {
								BlipDamage damage = b as BlipDamage;
								DieAfterTime(damage, deathDelay);
							});
					}
				}
		}

		// Returns true if dead.  False if still living.
		public override bool DoDamage(BlipDamage data)
		{
			if (master == null) {
				enabled = false;
				return false;
			}

			BlipDamage appliedData = new BlipDamage(data, damageMultiplier);
			master.DoDamage (appliedData);

			OnDamageTaken (data);

			return master.GetHealth() <= appliedData.damageAmount;
		}

		public override float GetHealth()
		{
			if (master == null) {
				return 0f;
			}

			return master.GetHealth ();
		}

		public override float GetMaxHealth()
		{
			if (master == null) {
				return 100f;
			}

			return master.GetMaxHealth ();
		}

		protected override Team GetTeam()
		{
			if (master == null) {
				return Team.NONE;
			}

			return master.gameObject.GetTeam (true);
		}

		public override Damageable GetMaster()
		{
			return master.GetMaster ();
		}

		public override Warrior GetWarrior()
		{
			if (master == null) {
				return null;
			}

			return master.GetWarrior ();
		}
	}
}
