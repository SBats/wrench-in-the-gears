using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBossHandController : MonoBehaviour {
	public float punchSpeed;
	public float punchPause;
	public float recoilDistance;
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
		Vector3 preMovePosition = recoilDistance != 0 ? new Vector3(
			target.x - this.transform.position.x != 0 ? this.transform.position.x - Mathf.Sign(target.x - this.transform.position.x) * recoilDistance : 0,
			target.y - this.transform.position.y != 0 ? this.transform.position.y - Mathf.Sign(target.y - this.transform.position.y) * recoilDistance : 0,
			target.z - this.transform.position.z != 0 ? this.transform.position.z - Mathf.Sign(target.z - this.transform.position.z) * recoilDistance : 0
		) : currentPosition;
		if (recoilDistance != 0) {
			yield return StartCoroutine(this._mover.Move(currentPosition, preMovePosition, this.punchSpeed * 0.33f));
			yield return new WaitForSeconds(this.punchPause);
		}
		yield return StartCoroutine(this._mover.Move(preMovePosition, target, this.punchSpeed));
		yield return new WaitForSeconds(this.punchPause);
		this._disableHitbox();
		yield return StartCoroutine(this._mover.Move(target, currentPosition, this.punchSpeed));
		this._moving = false;
	}
}
