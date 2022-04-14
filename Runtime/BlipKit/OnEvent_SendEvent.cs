using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEvent_SendEvent : MonoBehaviour
{
    public Blip.Type eventType = Blip.Type.ACTIVATE;

    [Tooltip("The object to receive the event we send.")]
    public GameObject target;

    [Tooltip("The object that will receive the event to start the sequence. (default is self)")]
    public GameObject eventSource;

    [Tooltip("Time after event before we accept the next event.")]
    public float cooldown = 0f;

    [Tooltip("Each time we receive an event, we move down this list, changing the output event.")]
    public List<Blip.Type> eventSequence;

    private int sequenceIndex = 0;
    private float reengageTime = 0f;

    void Awake() {
        if (eventSource == null) eventSource = gameObject;

        eventSource.ListenForBlips(eventType, delegate() {
            if (eventSequence.Count == 0 || target == null) {
                return;
            }

            if (reengageTime > Time.time) {
                return;
            }

            Blip.Type outputEvent = eventSequence[sequenceIndex];
            sequenceIndex = (sequenceIndex + 1) % eventSequence.Count;

            if (cooldown > 0f) {
                reengageTime = Time.time + cooldown;
            }

            target.SendBlip(outputEvent);
        });
    }
}
