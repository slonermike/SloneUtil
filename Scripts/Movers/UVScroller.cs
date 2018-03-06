/// <summary>
/// UVScroller
/// 2017 Slonersoft Games
/// Scrolls the UVs of the primary material on a mesh on the GameObject.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVScroller : MonoBehaviour {

	public enum UVScrollerType
	{
		MESH,
		LINE_RENDERER
	}

	// Starting scroll offset.
	public Vector2 startOffset;

	// Speed (per second) of the scroll in each direction.
	public Vector2 scrollSpeed;

	public UVScrollerType type = UVScrollerType.MESH;

	Material material = null;

	// Use this for initialization
	void Start () {

		if (type == UVScrollerType.MESH) {
			MeshRenderer mesh = GetComponent<MeshRenderer> ();
			if (mesh) {
				material = Material.Instantiate (mesh.material);
				mesh.material = material;
			}
		} else if (type == UVScrollerType.LINE_RENDERER) {
			LineRenderer lr = GetComponent<LineRenderer> ();
			if (lr) {
				material = Material.Instantiate (lr.material);
				lr.material = material;
			}
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
