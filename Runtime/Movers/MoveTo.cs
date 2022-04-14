using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slonersoft.SloneUtil.Movers {
	public class MoveTo : MoverPositioner {

		public float moveTime = 1.0f;
		public Vector3 movePosition;
		public bool localMotion = true;
		public bool smoothMotion = true;
		public bool relativeMotion = false;

		void OnEnable()
		{
			Vector3 finalPos = movePosition;
			if (relativeMotion) {
				if (localMotion)
					finalPos += moverTransform.localPosition;
				else
					finalPos += moverTransform.position;
			}

			Move(finalPos, moveTime, smoothMotion, localMotion);
		}
	}

}
