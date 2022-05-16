using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slonersoft.SloneUtil.Materials;
using Slonersoft.SloneUtil.Core;

namespace Slonersoft.SloneUtil.BlipKit {
    public class OnEvent_FlashMaterial : MonoBehaviour
    {
        public Blip.Type blipType = Blip.Type.DAMAGED;
        public string flashProperty = "_FlashColor";
        public float flashTime = 0.25f;
        public Color color = Color.white;
        MaterialFlasher flasher;

        void Awake() {
            flasher = gameObject.GetOrAddComponent<MaterialFlasher>();
            flasher.flashColorPropertyName = flashProperty;

            gameObject.ListenForBlips(blipType, _ => {
                flasher.DoFlash(color, flashTime);
            });
        }
    }
}
