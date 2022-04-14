using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientByMotion2D : MonoBehaviour
{
    Vector3 prevPosition;

    public int avgNumFrames = 5;

    private List<Vector3> rvecs;

    // Start is called before the first frame update
    void Start()
    {
        rvecs = new List<Vector3>();
        rvecs.Add(transform.right);
        prevPosition = transform.position;
    }

    void LateUpdate()
    {
        if (transform.position != prevPosition) {
            Vector3 fromPrev = (transform.position - prevPosition).normalized;
            rvecs.Add(fromPrev);
            if (rvecs.Count > avgNumFrames) {
                rvecs.RemoveAt(0);
            }

            Vector3 totalVec = Vector3.zero;
            foreach (Vector3 r in rvecs) {
                totalVec += r;
            }

            Vector3 avgVec = totalVec / (float)rvecs.Count;

            transform.rotation = SloneUtil2D.RotationFromRightVec(avgVec);

            prevPosition = transform.position;
        }
    }
}
