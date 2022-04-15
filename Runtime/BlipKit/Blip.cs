using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slonersoft.SloneUtil.BlipKit {
    public abstract class Blip {
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

        private class HandlerSet {
            public HandlerSet(Handler h) {
                handler = h;
                blindHandler = null;
            }
            public HandlerSet(BlindHandler h) {
                blindHandler = h;
                handler = null;
            }
            public Handler handler;
            public BlindHandler blindHandler;
        }

        private Dictionary<Blip.Type, List<HandlerSet>> handlerList = new Dictionary<Blip.Type, List<HandlerSet>>();

        public void RegisterHandler(Blip.Type eventName, Handler handler) {
            if (!handlerList.ContainsKey(eventName)) {
                handlerList[eventName] = new List<HandlerSet>();
            }

            handlerList[eventName].Add(new HandlerSet(handler));
        }

        public void RegisterHandler(Blip.Type eventName, BlindHandler handler) {
            if (!handlerList.ContainsKey(eventName)) {
                handlerList[eventName] = new List<HandlerSet>();
            }

            handlerList[eventName].Add(new HandlerSet(handler));
        }

        public void Send(Blip.Type type, Blip data = null) {
            if (!handlerList.ContainsKey(type)) {
                return;
            }

            handlerList[type].ForEach(delegate(HandlerSet set) {
                if (set.blindHandler != null) {
                    set.blindHandler();
                } else {
                    set.handler(data);
                }
            });
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

        public static void ListenForBlips(this GameObject o, Blip.Type blipType, BlipListener.BlindHandler handler) {
            BlipListener listener = o.GetComponent<BlipListener>();
            if (!listener) listener = o.AddComponent<BlipListener>();
            listener.RegisterHandler(blipType, handler);
        }
    }


}