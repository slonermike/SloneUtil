using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Slonersoft.SloneUtil.Core;

namespace Slonersoft.SloneUtil.AssetManagement {
    public class ObjectPool<TBehaviour> where TBehaviour : Behaviour
    {
        private TBehaviour[] rawPool;
        private LinkedList<TBehaviour> free;
        private LinkedList<TBehaviour> allocated;
        private GameObject poolParent;

        public ObjectPool(GameObject prefab, int numToAllocate) {
            poolParent = new GameObject($"Object Pool: {prefab.name}");
            rawPool = new TBehaviour[numToAllocate];
            for (int i = 0; i < numToAllocate; i++) {
                rawPool[i] = CoreUtils.InstantiateChild(poolParent, prefab).GetComponent<TBehaviour>();
                Debug.Assert(rawPool[i] != null, $"Prefab ${prefab.name} for ObjectPool does not have the appropriate behaviour on it.");
                rawPool[i].gameObject.SetActive(false);
            }
            free = new LinkedList<TBehaviour>(rawPool);
            allocated = new LinkedList<TBehaviour>();
        }

        /// <summary>
        /// Gets an item from the pool if one is available.
        /// </summary>
        /// <returns>MonoBehaviour or null</returns>
        public TBehaviour Allocate(bool setActive = true) {
            if (free.Count == 0) {
                return null;
            }

            LinkedListNode<TBehaviour> node = free.First;
            free.RemoveFirst();
            allocated.AddFirst(node);
            node.Value.gameObject.SetActive(setActive);
            node.Value.transform.SetParent(null);
            return node.Value as TBehaviour;
        }

        /// <summary>
        /// Returns an object to its pool via the pooled monobehaviour.
        /// Note: this is O(n) since it requires a lookup.
        /// TODO: Find a way to get back to O(1)
        /// </summary>
        /// <param name="b">Object to be returned to the pool.</param>
        public void Free(TBehaviour b) {
            b.gameObject.SetActive(false);
            LinkedListNode<TBehaviour> node = allocated.Find(b);
            if (node == null) {
                Debug.LogError($"Tried to look up {b.name} in object pool {poolParent.name} but it wasn't there.");
                return;
            }

            node.Value.transform.SetParent(poolParent.transform);

            allocated.Remove(node);
            free.AddFirst(node);
        }

        ~ObjectPool() {
            GameObject.Destroy(poolParent);
        }
    }
}

