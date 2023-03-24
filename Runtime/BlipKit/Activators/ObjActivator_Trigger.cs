using UnityEngine;
using System.Collections.Generic;
using Slonersoft.SloneUtil.BlipKit;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class ObjActivator_Trigger : ObjActivator
{
    public enum TriggerState {
        ENTER,
        STAY,
        EXIT
    }

    protected override void Awake() {
        base.Awake();

        #if DEBUG
        if (!GetComponent<Rigidbody>().isKinematic) {
            Debug.LogError("Rigid body on trigger must be kinematic.");
        }
        #endif
    }

    [System.Serializable]
    public struct EventPair {
        [Tooltip("The state change that will trigger the blip.")]
        public TriggerState triggerState;

        [Tooltip("The type of blip to send.")]
        public Blip.Type blipToSend;
    }

    public List<EventPair> pairs;

    void OnTriggerEnter(Collider col) {
        foreach (EventPair pair in pairs) {
            if (target && pair.triggerState == TriggerState.ENTER) {
                target.SendBlip(pair.blipToSend);
            }
        }
    }

    void OnTriggerExit(Collider col) {
        foreach (EventPair pair in pairs) {
            if (target && pair.triggerState == TriggerState.EXIT) {
                target.SendBlip(pair.blipToSend);
            }
        }
    }

    void OnTriggerStay(Collider collider) {
        foreach (EventPair pair in pairs) {
            if (target && pair.triggerState == TriggerState.STAY) {
                target.SendBlip(pair.blipToSend);
            }
        }
    }
}
