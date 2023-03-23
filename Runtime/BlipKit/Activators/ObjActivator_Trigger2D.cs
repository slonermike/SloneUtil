using UnityEngine;
using System.Collections.Generic;
using Slonersoft.SloneUtil.BlipKit;

[RequireComponent(typeof(Collider2D))]
public class ObjActivator_Trigger2D : ObjActivator
{
    public enum TriggerState {
        ENTER,
        STAY,
        EXIT
    }

    [System.Serializable]
    public struct EventPair {
        [Tooltip("The state change that will trigger the blip.")]
        public TriggerState triggerState;

        [Tooltip("The type of blip to send.")]
        public Blip.Type blipToSend;
    }

    public List<EventPair> pairs;

    void OnTriggerEnter2D(Collider2D col) {
        Debug.Log("entered trigger");
        foreach (EventPair pair in pairs) {
            if (target && pair.triggerState == TriggerState.ENTER) {
                target.SendBlip(pair.blipToSend);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        foreach (EventPair pair in pairs) {
            if (target && pair.triggerState == TriggerState.EXIT) {
                target.SendBlip(pair.blipToSend);
            }
        }
    }

    void OnTriggerStay2D(Collider2D collider) {
        foreach (EventPair pair in pairs) {
            if (target && pair.triggerState == TriggerState.STAY) {
                target.SendBlip(pair.blipToSend);
            }
        }
    }
}
