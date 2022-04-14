using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEvent_EnableCollision : MonoBehaviour
{
    [System.Serializable]
    public struct EventPair {
        public Blip.Type eventType;
        public bool enabled;
    }

    // TODO: work out 3d
    public Collider2D target;
    public GameObject navMeshObstacle;
    public List<EventPair> events;

    void Awake()
    {
        foreach (EventPair pair in events) {
            gameObject.ListenForBlips(pair.eventType, delegate() {
                target.enabled = pair.enabled;
                if (navMeshObstacle) navMeshObstacle.SetActive(target.enabled);
            });
        }
    }
}
