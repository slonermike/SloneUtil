using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slonersoft.SloneUtil.BlipKit {
    public class OnEvent_ToggleEmitter : MonoBehaviour
    {
        [System.Serializable]
        public struct EventPair {
            public Blip.Type eventType;
            public bool enable;
        }

        public List<ParticleSystem> targets;
        public List<EventPair> events;

        void Awake()
        {
            foreach (EventPair pair in events) {
                gameObject.ListenForBlips(pair.eventType, delegate() {
                    if (!pair.enable) {
                        foreach (ParticleSystem target in targets) {
                            target.Stop();
                        }
                    } else {
                        foreach (ParticleSystem target in targets) {
                            target.Play();
                        }
                    }
                });
            }
        }
    }
}

