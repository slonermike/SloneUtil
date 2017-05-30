/// <summary>
/// UVScroller
/// 2017 Slonersoft Games
/// Scrolls the UVs of the primary material on a mesh on the GameObject.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVScroller : MonoBehaviour {

	// Starting scroll offset.
	public Vector2 startOffset;

	// Speed (per second) of the scroll in each direction.
	public Vector2 scrollSpeed;

	Material material = null;

	// Use this for initialization
	void Start () {
		MeshRenderer mesh = GetComponent<MeshRenderer> ();
		if (mesh) {
			material = Material.Instantiate (mesh.material);
			mesh.material = material;
		}

		if (!material) {
			enabled = false;
		}

		material.mainTextureOffset += startOffset;
	}
	
	// Update is called once per frame
	void Update () {
		material.mainTextureOffset += scrollSpeed * Time.deltaTime;
	}
}
