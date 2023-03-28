using UnityEngine;
using Slonersoft.SloneUtil.Core;
using Slonersoft.SloneUtil.BlipKit;

namespace Slonersoft.SloneUtil.WarKit {
	// The product of a weapon, such as a bullet or laser.
	public class WeaponDamager : MonoBehaviour {

		public bool friendlyFire = false;
		private Warrior _owner;
		public Warrior owner {
			get {
				return _owner;
			}
			set {
				_owner = value;

				if (value != null) {
					TeamAssignment originalAssignment = _owner.GetComponent<TeamAssignment> ();
					if (originalAssignment != null) {
						SetTeam (originalAssignment.team);
					}
				}
			}
		}

		protected bool CanDamage(Damageable d) {
			return friendlyFire || d.team != ownerTeam.team;
		}

		protected TeamAssignment ownerTeam;
		protected LayerMask layerMask;

		virtual protected void Awake() {
			// Any children we create belong to the same owner.
			gameObject.ListenForBlips(Blip.Type.CREATED, blip => {
				GameObject createdObj = (blip as BlipCreate).createdObject;
				WeaponDamager createdDamager = createdObj.GetComponent<WeaponDamager>();
				if (createdDamager) {
					createdDamager.owner = this.owner;
				}
			});
		}

		public void SetTeam(Team t)
		{
			if (ownerTeam == null) {
				ownerTeam = gameObject.AddComponent<TeamAssignment> ();
			}

			ownerTeam.team = t;
		}

		// Use this for initialization
		protected virtual void Start () {
			if (ownerTeam == null || ownerTeam.team == Team.NONE) {
				TeamAssignment assignment = GetComponent<TeamAssignment> ();
				if (assignment != null) {
					SetTeam (assignment.team);
				}
			}

			if (WarKitSettings.is3D()) {
				layerMask = CoreUtils.GetLayerCollisionMask (Weapon.weaponLayer);
			} else {
				layerMask = Physics2D.GetLayerCollisionMask (Weapon.weaponLayer);
			}
		}

		// Update is called once per frame
		protected virtual void Update () {

		}
	}

}
