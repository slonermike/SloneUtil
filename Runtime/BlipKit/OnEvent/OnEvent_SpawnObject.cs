using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slonersoft.SloneUtil.BlipKit {
    /// <summary>
    /// Will spawn something when it receives an activation blip.
    /// </summary>
    public class OnEvent_SpawnObject : MonoBehaviour
    {
        public GameObject prefab;
        public Blip.Type spawnOnBlip = Blip.Type.ACTIVATE;

        void Awake() {
            gameObject.ListenForBlips(spawnOnBlip, Spawn);
        }

        void Spawn() {
            GameObject.Instantiate (prefab, transform.position, transform.rotation);
        }
    }
}

