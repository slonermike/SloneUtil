using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slonersoft.SloneUtil.BlipKit {
    public abstract class Blip {
        public bool isNoOp = false;
        public enum Type {
            DAMAGED,
            DIED,
            DESTROYED,
            CREATED,
            ARRIVED,
            ACTIVATE,
            DEACTIVATE,
            FORCE
        }
    }

    public class BlipDestroy : Blip{
        // We store this object so generic watchers can know who got destroyed (e.g. gameplay watching a bunch of enemies die)
        public GameObject destroyedObject;
        public BlipDestroy(GameObject o) {
            destroyedObject = o;
        }
    }

    public class BlipForce : Blip {
        public Vector3 addedVelocity;

        public BlipForce(Vector3 velToAdd) {
            addedVelocity = velToAdd;
        }
    }

    public class BlipCreate : Blip {
        public GameObject createdObject;
        public BlipCreate(GameObject o) {
            createdObject = o;
        }
    }

    public class BlipListener : MonoBehaviour
    {
        public delegate void Handler(Blip data);
        public delegate void BlindHandler();

        private Dictionary<Blip.Type, Handler> handlers = new Dictionary<Blip.Type, Handler>();
        private Dictionary<Blip.Type, BlindHandler> blindHandlers = new Dictionary<Blip.Type, BlindHandler>();


        public void RegisterHandler(Blip.Type eventName, Handler handler) {
            if (!handlers.ContainsKey(eventName)) {
                handlers[eventName] = handler;
            } else {
                handlers[eventName] += handler;
            }
        }

        public void UnregisterHandler(Blip.Type eventName, Handler handler) {
            if (handlers.ContainsKey(eventName)) {
                handlers[eventName] -= handler;
            }
        }

        public void RegisterHandler(Blip.Type eventName, BlindHandler handler) {
            if (!blindHandlers.ContainsKey(eventName)) {
                blindHandlers[eventName] = handler;
            } else {
                blindHandlers[eventName] += handler;
            }
        }

        public void UnregisterHandler(Blip.Type eventName, BlindHandler handler) {
            if (blindHandlers.ContainsKey(eventName)) {
                blindHandlers[eventName] -= handler;
            }
        }

        public void Send(Blip.Type type, Blip data = null) {
            if (handlers.ContainsKey(type)) {
                handlers[type](data);
            }
            if (blindHandlers.ContainsKey(type)) {
                blindHandlers[type]();
            }
        }

        public void OnDestroy() {
            Send(Blip.Type.DESTROYED);
        }
    }

    public static class BlipListenerUtils {
        public static void SendBlip(this GameObject o, Blip.Type type, Blip data = null) {
            BlipListener listener = o.GetComponent<BlipListener>();
            if (listener) listener.Send(type, data);
        }

        public static void ListenForBlips(this GameObject o, Blip.Type blipType, BlipListener.Handler handler) {
            BlipListener listener = o.GetComponent<BlipListener>();
            if (!listener) listener = o.AddComponent<BlipListener>();
            listener.RegisterHandler(blipType, handler);
        }

        public static void RemoveBlipListener(this GameObject o, Blip.Type blipType, BlipListener.Handler handler) {
            BlipListener listener = o.GetComponent<BlipListener>();
            if (listener) {
                listener.UnregisterHandler(blipType, handler);
            }
        }

        public static void ListenForBlips(this GameObject o, Blip.Type blipType, BlipListener.BlindHandler handler) {
            BlipListener listener = o.GetComponent<BlipListener>();
            if (!listener) listener = o.AddComponent<BlipListener>();
            listener.RegisterHandler(blipType, handler);
        }

        public static void RemoveBlipListener(this GameObject o, Blip.Type blipType, BlipListener.BlindHandler handler) {
            BlipListener listener = o.GetComponent<BlipListener>();
            if (listener) {
                listener.UnregisterHandler(blipType, handler);
            }
        }
    }


}