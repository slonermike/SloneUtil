using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slonersoft.SloneUtil.Materials;
using Slonersoft.SloneUtil.Core;

namespace Slonersoft.SloneUtil.Materials {
    public class MaterialFlasher : MonoBehaviour
    {
        class ActiveFlash {
            public ActiveFlash(Color _color, float _lifetime) {
                color = _color;
                lifetimeLeft = _lifetime;
                totalLifetime = _lifetime;
            }
            public void Decay(float deltaTime) {
                lifetimeLeft -= deltaTime;
            }
            public Color color;
            public float lifetimeLeft;
            public float totalLifetime;
        }
        public string flashColorPropertyName = "_FlashColor";
        MaterialInstancer materialInstancer;
        List<ActiveFlash> activeFlashes = new List<ActiveFlash>();
        void Awake() {
            materialInstancer = gameObject.GetOrAddComponent<MaterialInstancer>();
        }

        public void DoFlash(Color color, float flashTime) {
            ActiveFlash a = new ActiveFlash(color, flashTime);
            activeFlashes.Add(a);
        }

        // Update is called once per frame
        void Update()
        {
            float r = 0f, g = 0f, b = 0f;
            for(int i = activeFlashes.Count - 1; i >= 0; i--) {
                ActiveFlash a = activeFlashes[i];
                float pct = a.color.a * (a.lifetimeLeft / a.totalLifetime);
                r += a.color.r * pct;
                g += a.color.g * pct;
                b += a.color.b * pct;

                activeFlashes[i].Decay(Time.deltaTime);

                if (a.lifetimeLeft <= 0f) {
                    activeFlashes.RemoveAt(i);
                }
            }
            materialInstancer.SetColor(flashColorPropertyName, new Color(r, g, b, 1f));
        }
    }
}
