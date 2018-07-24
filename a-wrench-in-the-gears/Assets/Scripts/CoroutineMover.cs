using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineMover {
	public bool moving = false;

	private AnimationCurve _animationCurve;
	private GameObject _target;

	public CoroutineMover(AnimationCurve animationCurve, GameObject target) {
		this._animationCurve = animationCurve;
		this._target = target;
	}

	public IEnumerator Move(Vector3 start, Vector3 end, float duration) {
		float elapsedTime = 0f;

		this.moving = true;
		while (elapsedTime <= duration) {
			elapsedTime += Time.deltaTime;

			float animationProgression = Mathf.Clamp01(elapsedTime / duration);
			float curvedProgression = this._animationCurve.Evaluate(animationProgression);
			this._target.transform.position = Vector3.Lerp(start, end, curvedProgression);

			yield return null;
		}
		this.moving = false;
	}
}
