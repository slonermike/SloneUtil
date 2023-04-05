using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Slonersoft.SloneUtil.Core;

namespace Slonersoft.SloneUtil.AssetManagement {
    public class ObjectPool<TBehaviour> where TBehaviour : Behaviour
    {
        private List<TBehaviour> rawPool;
        private LinkedList<TBehaviour> free;
        private LinkedList<TBehaviour> allocated;
        private GameObject poolParent;
        private GameObject originalPrefab;
        private int pageSize;

        public ObjectPool(GameObject prefab, int numToAllocate) {
            pageSize = numToAllocate;
            originalPrefab = prefab;
            poolParent = new GameObject($"Object Pool: {prefab.name}");
            rawPool = new List<TBehaviour>();
            free = new LinkedList<TBehaviour>();
            allocated = new LinkedList<TBehaviour>();
        }

        public void AddObjectsToPool(int numObjects) {
            Debug.Log($"Allocating Children: {numObjects}");
            for (int i = 0; i < numObjects; i++) {
                var newChild = CoreUtils.InstantiateChild(poolParent, originalPrefab).GetComponent<TBehaviour>();
                Debug.Log($"Created Child: {newChild.name}");
                Debug.Assert(newChild != null, $"Prefab ${originalPrefab.name} for ObjectPool does not have the appropriate behaviour on it.");
                rawPool.Add(newChild);
                newChild.gameObject.SetActive(false);
                free.AddFirst(newChild);
            }
        }

        /// <summary>
        /// Gets an item from the pool if one is available.
        /// </summary>
        /// <returns>MonoBehaviour or null</returns>
        public TBehaviour Allocate(bool setActive = true) {
            if (free.Count == 0) {
                AddObjectsToPool(pageSize);
                Debug.Assert(free.Count >= 1);
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

        // IMPORTANT: needs to be called by the owner before deleting the object.
        public void Destroy() {
            GameObject.Destroy(poolParent);
        }
    }
}

