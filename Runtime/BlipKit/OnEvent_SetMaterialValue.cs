using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEvent_SetMaterialValue : MonoBehaviour
{
    [System.Serializable]
    public struct EventValue {
        public Blip.Type eventName;
        public float materialValue;
    }

    public SpriteRenderer target;
    public string valueName;
    public float changeTime;

    public List<EventValue> values;

    private SpriteMaterialInstance materialInstance;

    private float startTime = -1f;
    private float startValue;
    private float targetValue;

    void Awake() {
        materialInstance = target.GetComponent<SpriteMaterialInstance>();
        if (!materialInstance) {
            materialInstance = target.gameObject.AddComponent<SpriteMaterialInstance>();
            materialInstance.renderers.Add(target);
        }

        foreach (EventValue value in values) {
            gameObject.ListenForBlips(value.eventName, delegate() {
                StartChange(value.materialValue);
            });
        }
    }

    void StartChange(float newValue) {
        startValue = materialInstance.GetMaterialInstance().GetFloat(valueName);
        targetValue = newValue;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTime > 0f) {
            float pct = 1.0f;
            if (changeTime > 0f) {
                pct = Mathf.Clamp01((Time.time - startTime) / changeTime);
            }

            materialInstance.GetMaterialInstance().SetFloat(valueName, Mathf.Lerp(startValue, targetValue, pct));

            // Stop processing once done.
            if (pct >= 1.0f) {
                startTime = -1f;
            }
        }
    }
}
