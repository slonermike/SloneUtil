using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjActivator_Auto : ObjActivator
{
    public float delay = 0f;

    void Start()
    {
        if (delay > 0f) {
            gameObject.DoAfterTime(delay, delegate() {
                target.SendBlip(Blip.Type.ACTIVATE);
            });
        } else {
            target.SendBlip(Blip.Type.ACTIVATE);
        }
    }
}
