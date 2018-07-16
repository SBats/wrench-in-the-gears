using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DangerController : MonoBehaviour {

	public bool active = false;
	public LeverController lever;

	private UnityAction<bool> statusUpdateActions;
	private Animator animator;

	private void Awake() {
		this.animator = GetComponent<Animator>();
		this.UpdateStatus(this.active);
	}

	private void Start() {
		if (this.lever) {
			this.statusUpdateActions += this.UpdateStatus;
			this.lever.subscribeToStateChange(UpdateStatus);
		}
	}

	private void OnEnable() {
		this.UpdateStatus(this.active);
	}

	public void UpdateStatus(bool status) {
		this.active = status;
		this.animator.SetBool("active", status);
	}

	public void Enable() {
		this.active = true;
		this.animator.SetBool("active", true);
	}

	public void Disable() {
		this.active = false;
		this.animator.SetBool("active", false);
	}
}
