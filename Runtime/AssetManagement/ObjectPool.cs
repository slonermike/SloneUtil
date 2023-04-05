using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Slonersoft.SloneUtil.Core;

namespace Slonersoft.SloneUtil.AssetManagement {
    public class ObjectPool
    {
        public delegate void ObjectCreatedHandler(GameObject o);

        private List<GameObject> rawPool;
        private LinkedList<GameObject> free;
        private LinkedList<GameObject> allocated;
        private GameObject poolParent;
        private GameObject originalPrefab;
        private int pageSize;
        private ObjectCreatedHandler onObjectCreated;

        public ObjectPool(GameObject prefab, int numToAllocate, ObjectCreatedHandler createdHandler = null) {
            onObjectCreated = createdHandler;
            pageSize = numToAllocate;
            originalPrefab = prefab;
            poolParent = new GameObject($"Object Pool: {prefab.name}");
            rawPool = new List<GameObject>();
            free = new LinkedList<GameObject>();
            allocated = new LinkedList<GameObject>();
        }

        public void AddObjectsToPool(int numObjects) {
            for (int i = 0; i < numObjects; i++) {
                var newChild = CoreUtils.InstantiateChild(poolParent, originalPrefab);
                Debug.Assert(newChild != null, $"Prefab ${originalPrefab.name} for ObjectPool does not have the appropriate behaviour on it.");
                rawPool.Add(newChild);
                newChild.gameObject.SetActive(false);
                free.AddFirst(newChild);
                if (onObjectCreated != null) onObjectCreated(newChild);
            }
        }

        /// <summary>
        /// Gets an item from the pool if one is available.
        /// </summary>
        /// <returns>MonoBehaviour or null</returns>
        public GameObject Allocate(bool setActive = true) {
            if (free.Count == 0) {
                AddObjectsToPool(pageSize);
                Debug.Assert(free.Count >= 1);
            }

            LinkedListNode<GameObject> node = free.First;
            free.RemoveFirst();
            allocated.AddFirst(node);
            node.Value.gameObject.SetActive(setActive);
            node.Value.transform.SetParent(null);
            return node.Value as GameObject;
        }

        /// <summary>
        /// Returns an object to its pool via the pooled monobehaviour.
        /// Note: this is O(n) since it requires a lookup.
        /// TODO: Find a way to get back to O(1)
        /// </summary>
        /// <param name="b">Object to be returned to the pool.</param>
        public void Free(GameObject b) {
            b.gameObject.SetActive(false);
            LinkedListNode<GameObject> node = allocated.Find(b);
            if (node == null) {
                Debug.LogError($"Tried to look up {b.name} in object pool {poolParent.name} but it wasn't there.");
                return;
            }

            node.Value.transform.SetParent(poolParent.transform);

            allocated.Remove(node);
            free.AddFirst(node);
        }

        // IMPORTANT: needs to be called by the pool owner before destroying the pool.
        public void Destroy() {
            GameObject.Destroy(poolParent);
        }
    }
}

