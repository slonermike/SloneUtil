using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEvent_PlaySound : MonoBehaviour
{
    [Tooltip("The object receiving the event (none for this object)")]
    public GameObject eventReceiver;

    [Tooltip("The event to trigger the sound.")]
    public Blip.Type eventType = Blip.Type.ACTIVATE;

    [Tooltip("(optional) Where to play the sound.")]
    public AudioSource source;

    [Tooltip("(optional) Specific sound clip to play.")]
    public AudioClip clip;

    [Tooltip("(optional) Pitch at which to play sound (0 to not set).")]
    public float pitch = 0f;

    [Tooltip("(optional) Amount to vary the pitch (pitch must be > 0).")]
    public float pitchVariance = 0f;

    void Awake() {
        eventReceiver = eventReceiver == null ? gameObject : eventReceiver;
        Debug.Assert(clip != null || source != null);

        eventReceiver.ListenForBlips(eventType, delegate() {
            AudioSource currentSource = source;
            if (!currentSource) {
                if (clip) currentSource = AssetPool.PlayAudio(clip, gameObject);
            } else {
                if (clip) {
                    currentSource.PlayOneShot(clip);
                } else {
                    currentSource.Play();
                }
            }

            if (pitch > 0f) {
                currentSource.pitch = pitch + Random.Range(-pitchVariance, pitchVariance);
            }
        });
    }
}
