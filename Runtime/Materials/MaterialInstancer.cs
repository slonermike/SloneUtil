using System.Collections.Generic;
using UnityEngine;

namespace Slonersoft.SloneUtil.Materials {
    //
    // this will find all common materials in the gameobject tree and give them shared instances.
    //
    public class MaterialInstancer : MonoBehaviour
    {
        private Dictionary<Material, Material> mapToInstance = new Dictionary<Material, Material>();
        private List<Material> materials = new List<Material>();
        private Renderer[] renderers;
        void Awake() {
            renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers) {
                r.sharedMaterial = RegisterMaterial(r.sharedMaterial);
            }
        }

        public Material RegisterMaterial(Material src, bool shareInstances = true) {
            if (shareInstances) {
                if (!mapToInstance.ContainsKey(src)) {
                    mapToInstance[src] = Material.Instantiate(src);
                    materials.Add(mapToInstance[src]);
                }
                return mapToInstance[src];
            } else {
                Material newMaterial = Material.Instantiate(src);
                materials.Add(newMaterial);
                return newMaterial;
            }
        }

        public void SetFloat(string floatName, float val) {
            foreach (Material m in materials) {
                m.SetFloat(floatName, val);
            }
        }

        public void SetColor(string colorName, Color val) {
            foreach (Material m in materials) {
                m.SetColor(colorName, val);
            }
        }

        public void SetSortIndex(int index) {
            foreach (Material m in materials) {
                m.renderQueue = index;
            }
        }
    }
}
