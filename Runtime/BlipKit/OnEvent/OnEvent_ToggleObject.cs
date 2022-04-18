using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slonersoft.SloneUtil.BlipKit {

    public class OnEvent_ToggleObject : MonoBehaviour
    {
        public GameObject eventFromObject;
        public GameObject objectToToggle;

        [System.Serializable]
        public class EventPair {
            [Tooltip("The event to trigger the sound.")]
            public Blip.Type eventType = Blip.Type.ACTIVATE;

            [Tooltip("True to toggle on, false to toggle off.")]
            public bool toggleOn = true;
        }

        public List<EventPair> pairs;

        void Awake() {
            if (eventFromObject == null) {
                eventFromObject = gameObject;
            }

            foreach (EventPair p in pairs) {
                eventFromObject.ListenForBlips(p.eventType, delegate() {
                    objectToToggle.SetActive(p.toggleOn);
                });
            }
        }
    }
}
