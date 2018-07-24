using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GolemBossStates {
	Iddle,
	PunchLeft,
	PunchRight,
	SmashLeft,
	SmashRight
}

public class GolemBossController : MonoBehaviour {
	public GameObject head;
	public GameObject leftHand;
	public GameObject rightHand;

	private GolemBossHeadController headController;
	private GolemBossHandController leftHandController;
	private GolemBossHandController rightHandController;
	private GolemBossStates state;

	private void Awake() {
		this.headController = this.head.GetComponent<GolemBossHeadController>();
		this.leftHandController = this.leftHand.GetComponent<GolemBossHandController>();
		this.rightHandController = this.rightHand.GetComponent<GolemBossHandController>();
	}

	private void FixedUpdate() {
		if (!this.leftHandController.isMoving()) {
			this.Attack();
		}
	}

	private void Attack() {
		this.leftHandController.Punch(this.leftHand.transform.position + Vector3.right * 6);
	}
}
