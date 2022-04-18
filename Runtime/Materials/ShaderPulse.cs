using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slonersoft.SloneUtil.Materials {
	public class ShaderPulse : MonoBehaviour {
		public float frequency = 1.0f;
		public AnimationCurve glowCurve;
		public float minGlow = 0.0f;
		public float maxGlow = 1.0f;

		private Renderer _renderer;
		private MaterialPropertyBlock _propBlock;
		private float curTime;

		// Use this for initialization
		void Awake () {
			_propBlock = new MaterialPropertyBlock();
			_renderer = GetComponent<Renderer>();
			if (glowCurve.postWrapMode != WrapMode.Loop && glowCurve.postWrapMode != WrapMode.PingPong)
			{
				glowCurve.postWrapMode = WrapMode.Loop;
			}
		}

		private void OnEnable() {
			curTime = 0f;
		}

		// Update is called once per frame
		void Update () {
			curTime += Time.deltaTime * frequency;
			_renderer.GetPropertyBlock(_propBlock);

			float newGlow = glowCurve.Evaluate(curTime);
			_propBlock.SetFloat("_ScriptedGlow", newGlow);
			_renderer.SetPropertyBlock(_propBlock);
		}
	}

}
