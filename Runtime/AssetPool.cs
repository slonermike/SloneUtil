/************************************************************
 *
 *                    Asset Pool
 *                 2016 Slonersoft Games
 *
 * Create a list of assets that should be accessible from anywhere
 * in the game/level.  Create a prefab from the object with this pool,
 * and place it in each scene where the list should be accessible.
 *
 * Access the prefabs with:
 * GameWidePrefabs.pools[AssetPoolType].GetPrefab("Prefab Name")
 *
 *
 * Do what you want.  Distributed with WTFPL license.
 *
 ************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AssetPoolType {
	GAMEWIDE,
	LEVEL
}

public class AssetPool : MonoBehaviour {

	public AssetPoolType type = AssetPoolType.LEVEL;
	public GameObject[] prefabList;
	private Dictionary<string, GameObject> dictionary = new Dictionary<string, GameObject> ();

	public AudioClip[] soundAssets;
	public int soundPoolSize = 25;
	private Dictionary<string, AudioClip> audioDictionary = new Dictionary<string, AudioClip> ();

	static Dictionary<AssetPoolType, AssetPool> _pools;
	public static Dictionary<AssetPoolType, AssetPool> pools {
		get {

			if (_pools == null) {
				_pools = new Dictionary<AssetPoolType, AssetPool> ();
			}

			if (_pools.Count == 0) {

				AssetPool[] poolList = GameObject.FindObjectsOfType<AssetPool> ();

				foreach (AssetPool p in poolList) {
					if (_pools.ContainsKey (p.type)) {
						Debug.LogError ("Level contains multiple asset pools of type: " + p.type);
						continue;
					}

					p.Initialize ();

					_pools.Add (p.type, p);
				}
			}
			return _pools;
		}
	}

	private Slonersoft.ObjectPool<AudioSource> audioSourcePool;

	void Awake() {
		if (type == AssetPoolType.GAMEWIDE) {
			GameObject basePrefab = new GameObject("Audio Source");
			basePrefab.AddComponent<AudioSource>();
			audioSourcePool = new Slonersoft.ObjectPool<AudioSource>(basePrefab, soundPoolSize);
			GameObject.Destroy(basePrefab);
		}
	}

	private void Initialize()
	{
		if (dictionary.Count != 0) {
			return;
		}

		foreach (GameObject o in prefabList) {
			dictionary.Add (o.name, o);
		}

		foreach (AudioClip clip in soundAssets) {
			audioDictionary.Add (clip.name, clip);
		}
	}

	void OnDestroy()
	{
		// Remove this pool from the master dictionary.
        if (_pools != null)
		    _pools.Remove (type);
	}

	// Retrieve a gamewide prefab from the list.
	//
	// prefabName: the name in the inspector assigned to the child prefab.
	//
	public GameObject GetPrefab(string prefabName)
	{
		if (dictionary.ContainsKey (prefabName)) {
			return dictionary [prefabName];
		} else {
			return null;
		}
	}

	// Spawn an object from a gamewide prefab in the list.
	//
	// prefabName: the name in the inspector assigned to the prefab.
	//
	public GameObject InstantiatePrefab(string prefabName)
	{
		if (dictionary.ContainsKey (prefabName)) {
			return GameObject.Instantiate (dictionary [prefabName]) as GameObject;
		} else {
			return null;
		}
	}

	// Play an audio asset.
	//
	// soundName: the name of the sound file.
	// onObject (opt): the object on which to play it.
	//
	public AudioSource PlayAudio(string soundName, GameObject onObject = null)
	{
		AudioClip clip = this.audioDictionary [soundName];

		if (clip == null) {
			Debug.LogError ("Could not find audio in AssetPool: " + soundName);
			return null;
		}

		return PlayAudio(clip);
	}

	public static AudioSource PlayAudio(AudioClip clip, GameObject onObject = null) {
		AudioSource s = pools[AssetPoolType.GAMEWIDE].audioSourcePool.Allocate();
		GameObject o = s.gameObject;

		if (onObject == null) {
			onObject = Camera.main.gameObject;
		} else {
			onObject.ListenForBlips(Blip.Type.DIED, delegate() {
				if (o) o.transform.SetParent(null);
			});
		}

		o.transform.SetParent (onObject.transform);
		o.transform.localPosition = Vector3.zero;
		o.transform.rotation = Quaternion.identity;

		s.PlayOneShot (clip);
		o.DoAfterTime (clip.length, delegate() {
			pools[AssetPoolType.GAMEWIDE].audioSourcePool.Free(s);
		});

		return s;
	}
}
