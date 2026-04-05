using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Slonersoft.SloneUtil.Core;

namespace Slonersoft.SloneUtil.Movers
{
	// Moves things in local space to a specified point.
	//
	public class MoverPositioner : Mover {

		public bool isMoving {
			get {
				return moveRoutine != null;
			}
		}

		private float _pct;
		public float pct
		{
			get
			{
				return _pct;
			}
		}

		Coroutine moveRoutine = null;

		void OnDisable()
		{
			CancelMove();
		}

		IEnumerator Move_coroutine(Vector3 newPosition, float moveTime, bool smooth = true, bool localMovement = false)
		{
			Vector3 startPosition;
			if (localMovement)
				startPosition = moverTransform.localPosition;
			else
				startPosition = moverTransform.position;

			float startTime = Time.time;

			_pct = 0f;
			if (moveTime > 0)
			{
				while (startTime + moveTime > Time.time)
				{
					yield return new WaitForFixedUpdate();
					_pct = (Time.time - startTime) / moveTime;

					if (localMovement)
					{
						if (smooth)
						{
							moverTransform.localPosition = CoreUtils.LerpSmooth(startPosition, newPosition, _pct);
						}
						else
						{
							moverTransform.localPosition = Vector3.Lerp(startPosition, newPosition, _pct);
						}
					}
					else
					{
						if (smooth)
						{
							moverTransform.position = CoreUtils.LerpSmooth(startPosition, newPosition, _pct);
						}
						else
						{
							moverTransform.position = Vector3.Lerp(startPosition, newPosition, _pct);
						}
					}
				}
			}

			if (localMovement) {
				moverTransform.localPosition = newPosition;
			} else {
				moverTransform.position = newPosition;
			}

			moveRoutine = null;
		}

		public void Move(Vector3 newPosition, float moveTime, bool smooth = true, bool localMovement = false)
		{
			CancelMove();
			moveRoutine = StartCoroutine (Move_coroutine(newPosition, moveTime, smooth, localMovement));
		}

		public void CancelMove()
		{
			if (moveRoutine != null) {
				StopCoroutine (moveRoutine);
				moveRoutine = null;
			}
		}
	}

}
