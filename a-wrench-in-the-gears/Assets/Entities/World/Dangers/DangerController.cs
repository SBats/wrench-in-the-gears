using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DangerController : MonoBehaviour {

	public bool active = false;
	public LeverController lever;

	private UnityAction<bool> statusUpdateActions;

	private void Start() {
		this.statusUpdateActions += this.UpdateStatus;
		this.lever.subscribeToStateChange(UpdateStatus);
	}

	public void UpdateStatus(bool status) {
		this.active = status;
	}

	public void Enable() {
		this.active = true;
	}

	public void Disable() {
		this.active = false;
	}
}
