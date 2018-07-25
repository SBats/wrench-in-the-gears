using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBossHandController : MonoBehaviour {
	public float punchSpeed;
	public float punchPause;
	public AnimationCurve animationCurve;
	public GameObject hitboxContainer;

	private CoroutineMover _mover;
	private BoxCollider2D _hitbox;
	private bool _moving;

	//
	// Lifecycle
	//

	private void Awake() {
		this._mover = new CoroutineMover(animationCurve, this.gameObject);
		this._hitbox = this.hitboxContainer.GetComponent<BoxCollider2D>();
	}

	private void OnEnable() {
		this._disableHitbox();
	}

	//
	// Public
	//

	public Coroutine Punch(Vector3 target) {
		return StartCoroutine(this._PunchCoroutine(target));
	}

	public bool isMoving() {
		return this._moving || this._mover.moving;
	}

	//
	// Private
	//

	private void _enableHitbox() {
		this._hitbox.enabled = true;
	}

	private void _disableHitbox() {
		this._hitbox.enabled = false;
	}

	private IEnumerator _PunchCoroutine(Vector3 target) {
		this._moving = true;
		this._enableHitbox();
		Vector3 currentPosition = this.transform.position;
		yield return StartCoroutine(this._mover.Move(currentPosition, target, this.punchSpeed));
		yield return new WaitForSeconds(this.punchPause);
		this._disableHitbox();
		yield return StartCoroutine(this._mover.Move(target, currentPosition, this.punchSpeed));
		this._moving = false;
	}
}
