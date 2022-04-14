﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Send activations in a set of intervals.
/// Must be activated before it will start going.
/// </summary>
public class ObjActivator_Interval : ObjActivator
{
    [System.Serializable]
    public class Interval {
        [Tooltip("(optional) Specific target object for this interval.")]
        public GameObject overrideTarget;

        [Tooltip("Number of activations in this interval.")]
        public int numActivations = 1;

        [Tooltip("Time between activations this interval.")]
        public float timeBetween = 1f;

        [Tooltip("How long to 'cool down' before starting the next interval.")]
        public float timeAfter = 30f;

        [Tooltip("Activation message to send.")]
        public Blip.Type messageType = Blip.Type.ACTIVATE;
    }

    public bool looping = false;
    public List<Interval> intervalList;

    private Coroutine intervalCoroutine;

    protected override void Awake() {
        base.Awake();

        gameObject.ListenForBlips(Blip.Type.ACTIVATE, delegate() {
            if (intervalCoroutine == null) {
                intervalCoroutine = StartCoroutine(RunIntervals());
            }
        });
    }

    // Start is called before the first frame update
    void Start()
    {
#if DEBUG
        foreach (Interval i in intervalList) {
            GameObject t = i.overrideTarget ? i.overrideTarget : target;
            Debug.Assert(t != gameObject, "Interval activator (" + name + ") shouldn't target itself since it needs to be externally activated to start.");
        }
#endif
    }

    IEnumerator RunIntervals() {
        do {
            foreach (Interval interval in intervalList) {
                GameObject t = (interval.overrideTarget ? interval.overrideTarget : target);
                for (int i = 0; i < interval.numActivations; i++) {
                    t.SendBlip(interval.messageType);
                    if (i != interval.numActivations - 1) {
                        yield return new WaitForSeconds(interval.timeBetween);
                    }
                }

                yield return new WaitForSeconds(interval.timeAfter);
            }
        } while (looping);
    }
}
