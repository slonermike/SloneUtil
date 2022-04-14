using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slonersoft.SloneUtil.BlipKit {
    public class ObjActivator_Auto : ObjActivator
    {
        public float delay = 0f;

        private IEnumerator ActivateAfterTime(float delay) {
            yield return new WaitForSeconds(delay);
            target.SendBlip(Blip.Type.ACTIVATE);
        }

        void Start()
        {
            if (delay > 0f) {
                StartCoroutine(ActivateAfterTime(delay));
            } else {
                target.SendBlip(Blip.Type.ACTIVATE);
            }
        }
    }

}
