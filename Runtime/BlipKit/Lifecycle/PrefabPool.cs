using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slonersoft.SloneUtil.AssetManagement;

namespace Slonersoft.SloneUtil.BlipKit {
    public class PrefabPool : MonoBehaviour
    {
        public int pageSize = 5;
        private Dictionary<GameObject, ObjectPool> pools = new Dictionary<GameObject, ObjectPool>();

        public GameObject AllocatePrefab(GameObject prefab, ObjectPool.ObjectCreatedHandler createdHandler = null) {
            if (!pools.ContainsKey(prefab)) {
                pools[prefab] = new ObjectPool(prefab, pageSize, o => {
                    o.ListenForBlips(Blip.Type.DIED, () => {
                        pools[prefab].Free(o);
                    });
#if DEBUG
                    var selfDestruct = o.GetComponent<DestroyAfterTime>();
                    if (selfDestruct && !selfDestruct.blipOnly) {
                        selfDestruct.blipOnly = true;
                        Debug.LogError($"Prefab {prefab.name} is set to fully self destruct after a time.  Setting blipOnly.");
                    }
#endif
                    if (createdHandler != null) {
                        createdHandler(o);
                    }
                });
            }
            return pools[prefab].Allocate(false);
        }

        public GameObject AllocatePrefab(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null) {
            GameObject o = AllocatePrefab(prefab);
            o.transform.SetPositionAndRotation(position, rotation);
            o.transform.SetParent(parent);
            o.SetActive(true);
            return o;
        }

        private void OnEnable() {
            pools = new Dictionary<GameObject, ObjectPool>();
        }
        private void OnDisable() {
            foreach (KeyValuePair<GameObject, ObjectPool> kvp in pools) {
                kvp.Value.Destroy();
            }
        }
    }
}