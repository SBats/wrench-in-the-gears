using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GolemBossStates {
	None,
	Intro,
	Phase1,
	Phase2,
	Phase3,
	Outro,
	Dead
}

public enum GolemBossSubStates {
	None,
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

	public float _damages = 0;
	private GolemBossHeadController _headController;
	private GolemBossHandController _leftHandController;
	private GolemBossHandController _rightHandController;
	private GolemBossStates _state = GolemBossStates.Intro;
	private GolemBossSubStates _subState = GolemBossSubStates.Idle;
	private GolemBossSubStates _previousSubState = GolemBossSubStates.Idle;
	private GolemBossStates _currentStateCoroutine = GolemBossStates.None;
	private GolemBossSubStates _currentSubStateCoroutine = GolemBossSubStates.None;


	private void Awake() {
		this._headController = this.head.GetComponent<GolemBossHeadController>();
		this._leftHandController = this.leftHand.GetComponent<GolemBossHandController>();
		this._rightHandController = this.rightHand.GetComponent<GolemBossHandController>();
	}

	private void FixedUpdate() {
		this._UpdateStates();
	}

	private void _UpdateStates() {
		switch (this._state) {
			case GolemBossStates.Intro:
				if (this._currentStateCoroutine != GolemBossStates.Intro) {
					StartCoroutine(this._Intro());
				}
				break;
			case GolemBossStates.Phase1:
				if (this._currentStateCoroutine != GolemBossStates.Phase1) {
					StartCoroutine(this._Phase1());
				}
				break;
			case GolemBossStates.Phase2:
				if (this._currentStateCoroutine != GolemBossStates.Phase2) {
					StartCoroutine(this._Phase2());
				}
				break;
			case GolemBossStates.Phase3:
				if (this._currentStateCoroutine != GolemBossStates.Phase3) {
					StartCoroutine(this._Phase3());
				}
				break;
			case GolemBossStates.Outro:
				if (this._currentStateCoroutine != GolemBossStates.Outro) {
					StartCoroutine(this._Outro());
				}
				break;
		}
	}

	private void _UpdatePhase1SubState() {
		switch (this._subState) {
			case GolemBossSubStates.Idle:
				if (this._currentSubStateCoroutine != GolemBossSubStates.Idle) {
					StartCoroutine(this._IdlePhase1());
				}
				break;
			case GolemBossSubStates.SmashLeft:
				if (!this._leftHandController.isMoving()) {
					StartCoroutine(this._SmashLeft());
				}
				break;
			case GolemBossSubStates.SmashRight:
				if (!this._rightHandController.isMoving()) {
					StartCoroutine(this._SmashRight());
				}
				break;
		}
	}

	private void _UpdatePhase2SubState() {
		switch (this._subState) {
			case GolemBossSubStates.Idle:
				if (this._currentSubStateCoroutine != GolemBossSubStates.Idle) {
					StartCoroutine(this._IdlePhase2());
				}
				break;
			case GolemBossSubStates.PunchLeft:
				if (!this._leftHandController.isMoving()) {
					StartCoroutine(this._PunchLeft());
				}
				break;
			case GolemBossSubStates.PunchRight:
				if (!this._rightHandController.isMoving()) {
					StartCoroutine(this._PunchRight());
				}
				break;
		}
	}

	private void _UpdatePhase3SubState() {
		switch (this._subState) {
			case GolemBossSubStates.Idle:
				if (this._currentSubStateCoroutine != GolemBossSubStates.Idle) {
					StartCoroutine(this._IdlePhase3());
				}
				break;
			case GolemBossSubStates.SmashLeft:
				if (!this._leftHandController.isMoving()) {
					StartCoroutine(this._SmashLeft());
				}
				break;
			case GolemBossSubStates.SmashRight:
				if (!this._rightHandController.isMoving()) {
					StartCoroutine(this._SmashRight());
				}
				break;
		}
	}

	//
	// States
	//

	private IEnumerator _Intro() {
		this._currentStateCoroutine = GolemBossStates.Intro;
		yield return new WaitForSeconds(4);
		this._state = GolemBossStates.Phase1;
	}

	private IEnumerator _Phase1() {
		this._currentStateCoroutine = GolemBossStates.Phase1;
		while (this._damages < 1) {
			this._UpdatePhase1SubState();
			yield return null;
		}
		this._state = GolemBossStates.Phase2;
	}

	private IEnumerator _Phase2() {
		this._currentStateCoroutine = GolemBossStates.Phase2;
		while (this._damages < 2) {
			this._UpdatePhase2SubState();
			yield return null;
		}
		this._state = GolemBossStates.Phase3;
	}

	private IEnumerator _Phase3() {
		this._currentStateCoroutine = GolemBossStates.Phase3;
		while (this._damages < 3) {
			this._UpdatePhase3SubState();
			yield return null;
		}
		this._state = GolemBossStates.Outro;
	}

	private IEnumerator _Outro() {
		this._currentStateCoroutine = GolemBossStates.Outro;
		yield return new WaitForSeconds(4);
		this._state = GolemBossStates.Dead;
	}


	//
	// Substates
	//

	private IEnumerator _IdlePhase1() {
		this._currentSubStateCoroutine = GolemBossSubStates.Idle;
		yield return new WaitForSeconds(this.idleCicleDuration);
		if (this._state == GolemBossStates.Phase1) {
			switch (this._previousSubState){
				case GolemBossSubStates.SmashLeft:
					this._subState = GolemBossSubStates.SmashRight;
					break;
				case GolemBossSubStates.Idle:
				case GolemBossSubStates.SmashRight:
				default:
					this._subState = GolemBossSubStates.SmashLeft;
					break;
			}
			this._previousSubState = GolemBossSubStates.Idle;
		}
	}

	private IEnumerator _IdlePhase2() {
		this._currentSubStateCoroutine = GolemBossSubStates.Idle;
		yield return new WaitForSeconds(this.idleCicleDuration);
		if (this._state == GolemBossStates.Phase2) {
			switch (this._previousSubState){
				case GolemBossSubStates.PunchLeft:
					this._subState = GolemBossSubStates.PunchRight;
					break;
				case GolemBossSubStates.Idle:
				case GolemBossSubStates.PunchRight:
				default:
					this._subState = GolemBossSubStates.PunchLeft;
					break;
			}
			this._previousSubState = GolemBossSubStates.Idle;
		}
	}

	private IEnumerator _IdlePhase3() {
		this._currentSubStateCoroutine = GolemBossSubStates.Idle;
		yield return new WaitForSeconds(this.idleCicleDuration);
		if (this._state == GolemBossStates.Phase3) {
			switch (this._previousSubState){
				case GolemBossSubStates.SmashLeft:
					this._subState = GolemBossSubStates.SmashRight;
					break;
				case GolemBossSubStates.Idle:
				case GolemBossSubStates.SmashRight:
				default:
					this._subState = GolemBossSubStates.SmashLeft;
					break;
			}
			this._previousSubState = GolemBossSubStates.Idle;
		}
	}

	private IEnumerator _PunchLeft() {
		this._currentSubStateCoroutine = GolemBossSubStates.PunchLeft;
		yield return this._leftHandController.Punch(this.leftHand.transform.position + Vector3.right * 10);
		this._previousSubState = GolemBossSubStates.PunchLeft;
		this._subState = GolemBossSubStates.Idle;
	}

	private IEnumerator _PunchRight() {
		this._currentSubStateCoroutine = GolemBossSubStates.PunchRight;
		yield return this._rightHandController.Punch(this.rightHand.transform.position + Vector3.left * 10);
		this._previousSubState = GolemBossSubStates.PunchRight;
		this._subState = GolemBossSubStates.Idle;
	}

	private IEnumerator _SmashLeft() {
		this._currentSubStateCoroutine = GolemBossSubStates.SmashLeft;
		yield return this._leftHandController.Punch(this.leftHand.transform.position + Vector3.down * 10);
		this._previousSubState = GolemBossSubStates.SmashLeft;
		this._subState = GolemBossSubStates.Idle;
	}

	private IEnumerator _SmashRight() {
		this._currentSubStateCoroutine = GolemBossSubStates.SmashRight;
		yield return this._rightHandController.Punch(this.rightHand.transform.position + Vector3.down * 10);
		this._previousSubState = GolemBossSubStates.SmashRight;
		this._subState = GolemBossSubStates.Idle;
	}
}
