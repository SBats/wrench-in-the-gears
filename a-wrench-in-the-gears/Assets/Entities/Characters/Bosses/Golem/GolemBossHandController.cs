using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBossHandController : MonoBehaviour {
	public float punchSpeed;
	public AnimationCurve animationCurve;

	private CoroutineMover _mover;

	private void Awake() {
		this._mover = new CoroutineMover(animationCurve, this.gameObject);
	}

	public void Punch(Vector3 target) {
		StartCoroutine(this._mover.Move(this.transform.position, target, this.punchSpeed));
	}

	public bool isMoving() {
		return this._mover.moving;
	}
}
