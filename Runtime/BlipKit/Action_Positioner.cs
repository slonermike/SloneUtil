using UnityEngine;

public class Action_Positioner : MonoBehaviour
{
    [Tooltip("The object to move.")]
    public Transform objToMove;

    [Tooltip("Time to animate the move.")]
    public float moveTime = 0.25f;

    [Tooltip("Position to move it to.")]
    public Vector3 moveToPos;

    [Tooltip("True to move it relative to its parent, false to use world position.")]
    public bool localPosition = true;

    [Tooltip("True if moveToPos is relative to the object's start position, false if absolute.")]
    public bool relativeToStartPosition = true;

    [Tooltip("The blip we will receive that will make it move.")]
    public Blip.Type moveOnBlipType = Blip.Type.ACTIVATE;

    private MoverPositioner _positioner = null;
    private MoverPositioner positioner {
        get {
            if (_positioner == null) {
                _positioner = objToMove.gameObject.AddComponent<MoverPositioner>();
            }
            return _positioner;
        }
    }

    void Awake() {
        // Use self if nothing proivded.
        objToMove = objToMove ? objToMove : transform;

        Vector3 startPosition = localPosition ? objToMove.localPosition : objToMove.position;
        Vector3 destination = (relativeToStartPosition ? startPosition : Vector3.zero) + moveToPos;
        gameObject.ListenForBlips(moveOnBlipType, delegate() {
            positioner.Move(destination, moveTime, true, localPosition);
        });
    }
}
