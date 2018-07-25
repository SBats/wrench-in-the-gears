using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GolemBossStates {
	Idle,
	PunchLeft,
	PunchRight,
	SmashLeft,
	SmashRight
}

public class GolemBossController : MonoBehaviour {
	public GameObject head;
	public GameObject leftHand;
	public GameObject rightHand;
	public float idleCicleDuration;

	private bool _isAlive = true;
	private GolemBossHeadController _headController;
	private GolemBossHandController _leftHandController;
	private GolemBossHandController _rightHandController;
	private GolemBossStates _state = GolemBossStates.Idle;
	private GolemBossStates _previousState = GolemBossStates.Idle;
	private bool _idleCoroutineIsRunning = false;


	private void Awake() {
		this._headController = this.head.GetComponent<GolemBossHeadController>();
		this._leftHandController = this.leftHand.GetComponent<GolemBossHandController>();
		this._rightHandController = this.rightHand.GetComponent<GolemBossHandController>();
	}

	private void FixedUpdate() {
		switch (this._state) {
			case GolemBossStates.Idle:
				if (!this._idleCoroutineIsRunning) {
					StartCoroutine(this._Idle());
				}
				break;
			case GolemBossStates.PunchLeft:
				if (!this._leftHandController.isMoving()) {
					StartCoroutine(this._PunchLeft());
				}
				break;
			case GolemBossStates.PunchRight:
				if (!this._rightHandController.isMoving()) {
					StartCoroutine(this._PunchRight());
				}
				break;
		}
	}

	private IEnumerator _Idle() {
		this._idleCoroutineIsRunning = true;
		yield return new WaitForSeconds(this.idleCicleDuration);
		switch (this._previousState){
			case GolemBossStates.Idle:
			case GolemBossStates.PunchRight:
				this._state = GolemBossStates.PunchLeft;
				break;
			case GolemBossStates.PunchLeft:
				this._state = GolemBossStates.PunchRight;
				break;
		}
		this._idleCoroutineIsRunning = false;
		this._previousState = GolemBossStates.Idle;
	}

	private IEnumerator _PunchLeft() {
		yield return this._leftHandController.Punch(this.leftHand.transform.position + Vector3.right * 6);
		this._previousState = GolemBossStates.PunchLeft;
		this._state = GolemBossStates.Idle;
	}

	private IEnumerator _PunchRight() {
		yield return this._rightHandController.Punch(this.rightHand.transform.position + Vector3.left * 6);
		this._previousState = GolemBossStates.PunchRight;
		this._state = GolemBossStates.Idle;
	}
}
