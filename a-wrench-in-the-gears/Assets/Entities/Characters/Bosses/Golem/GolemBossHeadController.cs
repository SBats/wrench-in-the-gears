using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBossHeadController : MonoBehaviour {
	public AnimationCurve animationCurve;

	private CoroutineMover _mover;

	private void Awake() {
		this._mover = new CoroutineMover(animationCurve, this.gameObject);
	}
}
