using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEvent_SetSprite : MonoBehaviour
{
    public Blip.Type eventType = Blip.Type.ACTIVATE;
    public Sprite sprite;
    public SpriteRenderer target;

    void Awake() {
        gameObject.ListenForBlips(eventType, delegate() {
            target.sprite = sprite;
        });
    }
}
