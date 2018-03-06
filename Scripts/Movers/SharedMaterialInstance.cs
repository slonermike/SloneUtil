using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedMaterialInstance : MonoBehaviour {

	public Renderer[] objects;
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

		if (objects.Length <= 0) {
			return;
		}

		instancedMaterial = Material.Instantiate (objects [0].sharedMaterial);

		foreach (Renderer r in objects) {
			r.sharedMaterial = instancedMaterial;
		}
	}

	void Awake()
	{
		InstanceMaterial ();
	}
}
