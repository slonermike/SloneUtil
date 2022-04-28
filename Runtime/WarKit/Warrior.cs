using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slonersoft.SloneUtil.WarKit {
	public class Warrior : MonoBehaviour {
        public enum WeaponSlots {
            PRIMARY,
            SECONDARY,

            NUM_TYPES
        };

        [System.Serializable]
        public class WeaponAssignment {
            public WeaponSlots slot = WeaponSlots.PRIMARY;
            public Weapon weapon;
        }

        [Tooltip("Position at which the weapon is attached")]
        public Transform muzzleTransform;

        [Tooltip("Weapon to give the character if he has none.")]
            public List<WeaponAssignment> defaultWeapons;

        protected List<Weapon> weapons;

		[HideInInspector] public Health health;

		private Team _team = Team.NONE;
		public Team team {
			get {
				return _team;
			}
		}

		protected virtual void Awake()
		{
			_team = TeamUtil.GetTeam(gameObject);
			health = GetComponent<Health> ();
		}

        protected virtual void Start() {
            weapons = new List<Weapon> {null, null};
            Debug.Assert(weapons.Count == (int)WeaponSlots.NUM_TYPES);

            foreach (WeaponAssignment wa in defaultWeapons) {
                GiveWeapon(wa.weapon.gameObject, wa.slot);
            }
        }

        // Give the weapon object
        //
        public void GiveWeapon(GameObject weaponPrefab, WeaponSlots slot = WeaponSlots.PRIMARY)
        {
            // Get rid of the old weapon.
            if (weapons[(int)slot] != null) {
                GameObject.Destroy (weapons[(int)slot]);
            }

            if (muzzleTransform == null) {
                muzzleTransform = transform;
            }

            // Create and attach the weapon.
            GameObject weaponObj = GameObject.Instantiate (weaponPrefab, muzzleTransform.position, muzzleTransform.rotation, muzzleTransform);
            weapons[(int)slot] = weaponObj.GetComponentInChildren<Weapon> ();
            TeamAssignment ta = weapons[(int)slot].gameObject.AddComponent<TeamAssignment> ();
            ta.team = team;
            weapons[(int)slot].owner = this;
        }

        public void PullTrigger(WeaponSlots slot = WeaponSlots.PRIMARY)
        {
            if (weapons[(int)slot] != null) {
                weapons[(int)slot].PullTrigger ();
            }
        }

        public void ReleaseTrigger(WeaponSlots slot = WeaponSlots.PRIMARY)
        {
            if (weapons[(int)slot] != null) {
                weapons[(int)slot].ReleaseTrigger ();
            }
        }
	}
}
