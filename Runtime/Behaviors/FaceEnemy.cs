using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceEnemy : MonoBehaviour
{
    public float turnSpeed = 90f;
    const float RE_EVALUATE_TIME = 0.5f;

    TargetFinder finder;

    // Start is called before the first frame update
    void Start()
    {
        finder = gameObject.GetOrAddComponent<TargetFinder>();
    }

    void FixedUpdate() {
        if (finder.targetDamageable) {
            SloneUtil2D.TurnToPoint(transform, finder.targetDamageable.transform.position, turnSpeed);
        }
    }
}
