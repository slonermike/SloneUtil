using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slonersoft.SloneUtil.Materials {
  public class SpriteMaterialInstance : MonoBehaviour {
			public List<SpriteRenderer> renderers = new List<SpriteRenderer>();
		Material instancedMaterial;

		public Material GetMaterialInstance ()
		{
			InstanceMaterial ();
			return instancedMaterial;
		}

		void InstanceMaterial()
		{
			if (instancedMaterial != null) {
				return;
			}

			if (renderers.Count == 0) {
				Debug.LogWarning($"Attempting to instance a sprite material with no renderers. Owner: {name}");
				return;
			}

			instancedMaterial = Material.Instantiate (renderers[0].sharedMaterial);

			foreach (Renderer r in renderers) {
				r.sharedMaterial = instancedMaterial;
			}
		}

		void Start()
		{
			InstanceMaterial ();
		}

		public void SetAlpha(float alpha) {
			Color prevColor = GetMaterialInstance().color;
			GetMaterialInstance().color = new Color(prevColor.r, prevColor.g, prevColor.b, alpha);
		}
	}
}