using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slonersoft.SloneUtil.WarKit {
	public class ai_Base : MonoBehaviour {

		[Tooltip("Top speed for AI movement.")]
		public float speed = 10.0f;

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

			#if DEBUG
				DebugVerify();
			#endif
		}

		protected virtual void DebugVerify()
		{
			Debug.Assert(gameObject.layer == LayerMask.NameToLayer("NPC"), "AI expects object on NPC layer.");
		}
	}
}
