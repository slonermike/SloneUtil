using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Slonersoft.SloneUtil.BlipKit;
using Slonersoft.SloneUtil.WarKit;

[RequireComponent(typeof(Damageable))]
public class ExpandOnDamage : MonoBehaviour
{
    [Range(1f,3f)]
    public float maxExpansion = 1.5f;
    public float contractRate = 2f;

    [Tooltip("If you want a different object to expand when you damage this, assign that here.")]
    public Transform expansionDelegate;
    Coroutine expansionCorotine;

    IEnumerator Expansion_coroutine(Transform xform, Vector3 baseScale, float expansionAmount) {
        do {
            yield return new WaitForEndOfFrame();
            expansionAmount -= Time.deltaTime * contractRate;
            expansionAmount = Mathf.Max(expansionAmount, 1f);
            xform.localScale = baseScale * expansionAmount;
        } while (expansionAmount > 1f);
        expansionCorotine = null;
    }

    // Start is called before the first frame update
    void Awake()
    {
        Damageable damageable = GetComponent<Damageable>();
        Vector3 baseScale = transform.localScale;

        gameObject.ListenForBlips(Blip.Type.DAMAGED, blip => {
            if (expansionCorotine != null) {
                StopCoroutine(expansionCorotine);
            }
            expansionCorotine = StartCoroutine(
                Expansion_coroutine(
                    expansionDelegate ? expansionDelegate : transform, baseScale,
                    Mathf.Lerp(1f, maxExpansion, 1f - damageable.pctHealth)
                )
            );
        });
    }
}
