using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Slonersoft.SloneUtil.Core;

namespace Slonersoft.SloneUtil.WarKit {
    /// <summary>
    /// Finds viable damageable targets based on team assignment.
    /// </summary>
    public class TargetFinder : Targeting
    {
        [Tooltip("Time from one target search till the next")]
        public float timeBetweenSearches = 0.5f;

        [Range(0, 360f)]
        public float searchAngle = 360f;

        [Tooltip("Distance to search, -1 for no limit")]
        public float searchDistance = -1f;

        [Tooltip("True to only target onscreen enemies.  False to allow them to be anywhere.")]
        public bool onscreenTargetsOnly = true;

        [Tooltip("-1 to search whether onscreen or not, otherwise percentage offscreen at which time we start searching.")]
        public float sourceOffscreenPct = 0.25f;

        [Tooltip("True to perform a raycast to validate target visibility.")]
        public bool raycastCheck = false;

        [Tooltip("True if the existing target can be lost via raycast.")]
        public bool loseTargetViaRaycast = false;

        Damageable damageableTarget;
        float nextSearchTime;
        Team team;

        protected override GameObject GetTarget() {
            return targetDamageable ? targetDamageable.gameObject : null;
        }

        protected override Damageable GetTargetDamageable() {
            if (damageableTarget && damageableTarget.pctHealth > 0f)
                return damageableTarget;
            else
                return null;
        }

        void Start() {
            team = TeamUtil.GetTeam(gameObject, true);
            nextSearchTime = Time.time;
        }

        void Update() {
            bool allowCheck = sourceOffscreenPct < 0f || CoreUtils.IsPointOnAnyScreen(transform.position, sourceOffscreenPct);
            if (allowCheck && nextSearchTime <= Time.time) {
                Damageable exemptTarget = loseTargetViaRaycast ? null : damageableTarget;
                damageableTarget = Damageable.GetTarget(transform, team, searchAngle, searchDistance, onscreenTargetsOnly, true, raycastCheck, exemptTarget);

                // Randomize the next time so like enemies don't end up raycasting all at once.
                nextSearchTime = Time.time + (timeBetweenSearches * Random.Range(0.9f, 1f));
            }
        }

        public void ResetTarget() {
            damageableTarget = null;
        }
    }

}
