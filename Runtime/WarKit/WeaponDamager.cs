using UnityEngine;
using Slonersoft.SloneUtil.Core;

namespace Slonersoft.SloneUtil.WarKit {
	// The product of a weapon, such as a bullet or laser.
	public class WeaponDamager : MonoBehaviour {

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

		public bool is3D = false;

		protected TeamAssignment ownerTeam;
		protected LayerMask layerMask;
		protected SpawnDamagerOnDeathHandler spawnHandler;

		public void RefreshLayerMask() {
			if (is3D) {
				layerMask = CoreUtils.GetLayerCollisionMask (gameObject.layer);
			} else {
				layerMask = Physics2D.GetLayerCollisionMask (gameObject.layer);
			}
		}

		virtual protected void Awake() {
			spawnHandler = gameObject.AddComponent<SpawnDamagerOnDeathHandler>();
			RefreshLayerMask();
		}

		public void SetTeam(Team t)
		{
			if (ownerTeam == null) {
				ownerTeam = gameObject.AddComponent<TeamAssignment> ();
			}

			ownerTeam.team = t;

			if (ownerTeam != null) {
				int newLayer = ownerTeam.GetWeaponLayer ();
				if (newLayer >= 0) {
					gameObject.layer = newLayer;
				}
			} else {
				Debug.LogError ("WeaponDamager created by owner (" + _owner.name + ") with no team.");
			}

			RefreshLayerMask();
		}

		// Use this for initialization
		protected virtual void Start () {
			if (ownerTeam == null || ownerTeam.team == Team.NONE) {
				TeamAssignment assignment = GetComponent<TeamAssignment> ();
				if (assignment != null) {
					SetTeam (assignment.team);
				}
			}
		}

		// Update is called once per frame
		protected virtual void Update () {

		}
	}

}
