using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slonersoft.SloneUtil.WarKit {
	public abstract class Weapon : MonoBehaviour {
		public string displayName = "Untitled Weapon";
		public GameObject crosshair;
		protected TeamAssignment ownerTeam;

		static int _weaponLayer = -1;
		public static int weaponLayer {
			get {
				if (_weaponLayer < 0) {
					_weaponLayer = LayerMask.NameToLayer ("WEAPON");
				}
				return _weaponLayer;
			}
		}

		private Warrior _owner;
		public Warrior owner {
			get {
				return _owner;
			}
			set {
				_owner = value;

				if (value != null) {
					TeamAssignment originalAssignment = _owner.GetComponent<TeamAssignment> ();
					ownerTeam = gameObject.AddComponent<TeamAssignment> ();

					if (originalAssignment == null) {
						Debug.LogError ("Weapon assigned to owner (" + _owner.name + ") with no team.");
					} else {
						ownerTeam.team = originalAssignment.team;
					}
				}
			}
		}

		private bool _triggerDown = false;
		public bool triggerDown {
			get {
				return _triggerDown;
			}
		}

		void OnDestroy()
		{
			if (_triggerDown) {
				ReleaseTrigger ();
			}
		}

		public virtual void PullTrigger() {
			if (_triggerDown) {
				ReleaseTrigger ();
			}
			_triggerDown = true;
		}

		public virtual void ReleaseTrigger() {
			_triggerDown = false;
		}

		protected virtual void Awake() {}

		protected virtual void Start() {}

		protected virtual void Update() {}
	}

}
