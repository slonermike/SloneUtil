using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Slonersoft.SloneUtil.BlipKit;

public class DestroyIfStuck : MonoBehaviour
{
    public float afterTime = 1.0f;
    public float minSpeed = 0.01f;
    float stuckTime = 0f;
    Vector3 prevPosition;
    public bool blipOnly = true;

    // Start is called before the first frame update
    void Start()
    {
        prevPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float minDistSqThisFrame = (minSpeed * Time.deltaTime);
        if ((transform.position - prevPosition).sqrMagnitude > minDistSqThisFrame * minDistSqThisFrame) {
            stuckTime = 0f;
        } else {
            stuckTime += Time.fixedDeltaTime;
        }

        if (stuckTime > afterTime) {
            gameObject.SendBlip(Blip.Type.DIED);
            if (!blipOnly) {
                GameObject.Destroy (gameObject);
            }
        } else {
            prevPosition = transform.position;
        }
    }
}
