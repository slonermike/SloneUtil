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

		public int GetWeaponLayer()
		{
			return TeamUtil.GetTeamWeaponLayer (team);
		}
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

		private static Dictionary<Team, int> _teamWeaponLayers;
		public static int GetTeamWeaponLayer(Team t)
		{
			// Initialize team layers if we haven't already.
			if (_teamWeaponLayers == null) {
				_teamWeaponLayers = new Dictionary<Team, int> ();

				foreach (Team team in Enum.GetValues(typeof(Team)))
				{
					string layerName = Enum.GetName (typeof(Team), team) + "_WEAPON";
					_teamWeaponLayers [team] = LayerMask.NameToLayer (layerName);

					if (_teamWeaponLayers[team] < 0) {
						_teamWeaponLayers [team] = LayerMask.NameToLayer ("WEAPON");

						if (_teamWeaponLayers[team] < 0) {
							Debug.LogError("Could not find team weapon layer: " + layerName);
						}
					}
				}
			}

			return _teamWeaponLayers [t];
		}
	}

}
