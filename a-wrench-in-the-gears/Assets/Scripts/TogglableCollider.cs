﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class TogglableCollider : MonoBehaviour {
	public bool active = false;
	public LeverController lever;

	private Collider2D targetCollider;

	private UnityAction<bool> statusUpdateActions;

	private void Awake() {
		this.targetCollider = GetComponent<Collider2D>();
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
		this.targetCollider.enabled = status;
		this.active = status;
	}
}
