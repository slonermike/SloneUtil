using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Slonersoft.SloneUtil.WarKit {
	public enum Team {
		NONE,
		NPC,
		PLAYER
	};

	public class TeamAssignment : MonoBehaviour {
		public Team team = Team.NONE;
	}

	public static class TeamUtil {
		public static Team GetTeam(this GameObject o, bool required = false) {
			TeamAssignment t = o.GetComponent<TeamAssignment> ();
			if (t == null) {
				if (required) {
					Debug.LogWarning("Object " + o.name + " needs a team assignment.");
				}
				return Team.NONE;
			}
			return t.team;
		}

		public static void AssignTeam(this GameObject o, Team t)
		{
			TeamAssignment assignment = o.GetComponent<TeamAssignment>();
			if (assignment == null) {
				assignment = o.AddComponent<TeamAssignment>();
			}
			assignment.team = t;
		}
	}

}
