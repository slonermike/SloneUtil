using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slonersoft.SloneUtil.WarKit {
    public class WarKitSettings : MonoBehaviour
    {
        private static WarKitSettings _inst;
        private static WarKitSettings inst {
            get {
                if (!_inst) {
                    _inst = GameObject.FindObjectOfType<WarKitSettings>();
                    if (!_inst) {
                        GameObject o = new GameObject();
                        _inst = o.AddComponent<WarKitSettings>();
                    }
                }

                return _inst;
            }
        }

        public enum Dimensions {
            DIM_2D, DIM_3D
        }

        public Dimensions dimensions = Dimensions.DIM_2D;

        public static bool is3D() {
            return inst.dimensions == Dimensions.DIM_3D;
        }

        public static bool is2D() {
            return inst.dimensions == Dimensions.DIM_2D;
        }
    }

    public struct CastHit {

    }

    public static class WarKit {
        /// <summary>
        /// Gives right vector for 2d, forward vector for 3d games.
        /// </summary>
        public static Vector3 warkitForward(this Transform xForm) {
            if (WarKitSettings.is3D()) {
                return xForm.forward;
            } else {
                return xForm.right;
            }
        }

        public static Vector3 forward {
            get {
                if (WarKitSettings.is3D()) {
                    return Vector3.forward;
                } else {
                    return Vector3.right;
                }
            }
        }
    }
}
