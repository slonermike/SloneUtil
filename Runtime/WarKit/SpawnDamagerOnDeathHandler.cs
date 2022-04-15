using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slonersoft.SloneUtil.BlipKit;

namespace Slonersoft.SloneUtil.WarKit {
	public class SpawnDamagerOnDeathHandler : SpawnOnDeathHandler {
		[Tooltip("True to inherit the team of the one who created it (directly or indirectly).")]
		public bool inheritTeam = true;

		public override void OnSpawn(GameObject parent, GameObject parentKiller)
		{
			WeaponDamager damager = gameObject.GetComponent<WeaponDamager>();
			if (!damager) {
				Debug.LogError(name + " is not a weapon damager.");
				return;
			}

			GameObject killer = parentKiller ? parentKiller : parent;

			ai_Base ai = killer.GetComponentInParent<ai_Base>();
			if (ai != null) {
				damager.owner = ai;
			}

			if (inheritTeam) {
				damager.SetTeam(TeamUtil.GetTeam(killer));
			} else {
				// They own it, but it can still damage them.
				damager.SetTeam(Team.NONE);
			}
		}
	}

}
