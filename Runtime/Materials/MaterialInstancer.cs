using System.Collections;
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
                var commonMaterial = r.sharedMaterial;
                if (!mapToInstance.ContainsKey(commonMaterial)) {
                    mapToInstance[commonMaterial] = Material.Instantiate(commonMaterial);
                    materials.Add(mapToInstance[commonMaterial]);
                }
                r.sharedMaterial = mapToInstance[commonMaterial];
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
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
