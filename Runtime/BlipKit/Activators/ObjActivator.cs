using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slonersoft.SloneUtil.BlipKit {
    public abstract class ObjActivator : MonoBehaviour
    {
        [Tooltip("The object to send the ACTIVATE message to. Default is self.")]
        public GameObject target;

        protected virtual void Awake() {
            target = target ? target : gameObject;
        }
    }
}
