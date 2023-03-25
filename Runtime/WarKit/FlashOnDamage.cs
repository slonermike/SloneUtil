using UnityEngine;
using Slonersoft.SloneUtil.Materials;
using Slonersoft.SloneUtil.Core;
using Slonersoft.SloneUtil.BlipKit;

namespace Slonersoft.SloneUtil.WarKit {
    public class FlashOnDamage : MonoBehaviour
    {
        public string flashProperty = "_FlashColor";
        public float flashTime = 0.25f;
        public Gradient color = new Gradient() {
            colorKeys = new GradientColorKey[3] {
                new GradientColorKey(new Color(1, 1, 1), 0f),
                new GradientColorKey(new Color(1, 1, 1), 0.5f),
                new GradientColorKey(new Color(1, 0, 0), 1)
            },
            alphaKeys = new GradientAlphaKey[2] {
                new GradientAlphaKey(0.25f, 0.25f),
                new GradientAlphaKey(1f, 0.75f)
            }
        };
        MaterialFlasher flasher;

        void Awake() {
            flasher = gameObject.GetOrAddComponent<MaterialFlasher>();
            flasher.flashColorPropertyName = flashProperty;

            Damageable d = GetComponent<Damageable>();

            gameObject.ListenForBlips(Blip.Type.DAMAGED, (blip) => {
                flasher.DoFlash(color.Evaluate(1f - d.pctHealth), flashTime);
            });
        }
    }
}
