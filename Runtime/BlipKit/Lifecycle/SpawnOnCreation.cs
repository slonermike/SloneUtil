using UnityEngine;
using System.Collections;

namespace Slonersoft.SloneUtil.BlipKit {
    public class SpawnOnCreation : MonoBehaviour {

        [Tooltip("Prefab of the object to spawn")]
        public GameObject spawnObject;

        [Tooltip("True to parent the new object to this one.")]
        public bool attachOnCreation = true;

        [Tooltip("Set to destroy after a specified time (<= 0 means do not destroy)")]
        public float destroyAfterTime = -1f;

        /// <summary>
        /// /// Spawns the objects when the thing is created.
        /// </summary>
        void Start() {
            if (spawnObject == null) {
                Debug.LogError ("SpawnOnCreation on " + gameObject.name + " has no spawnObject!");
                return;
            }

            GameObject o = null;
            if (attachOnCreation) {
                o = GameObject.Instantiate (spawnObject, transform.position, transform.rotation) as GameObject;
			    o.transform.SetParent (transform);
            } else {
                o = GameObject.Instantiate(spawnObject, transform.position, transform.rotation);
            }

            if (o && destroyAfterTime > 0f) {
                DestroyAfterTime dat = o.AddComponent<DestroyAfterTime>();
                dat.lifetime = destroyAfterTime;
            }
        }
    }
}
