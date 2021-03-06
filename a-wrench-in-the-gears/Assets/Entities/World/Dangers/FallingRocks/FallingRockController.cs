﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class FallingRockController : MonoBehaviour {
	public GameObject rock;
	public GameObject hitbox;
	public GameObject trigger;
	public float fallDuration;
	public AnimationCurve animationCurve;
	public bool reversed = false;
	public bool active = true;
	public bool autoFall = false;
	public float startDelay = 0f;

	private Vector3 top;
	private Vector3 bottom;
	private Vector3 start;
	private Vector3 end;
	private BoxCollider2D hitBoxCollider;
	private BoxCollider2D movementContainer;
	private TriggerController triggerController;
	private Vector3 currentTarget;
	public bool triggerStatus;
	private CoroutineMover mover;

	private void Awake() {
		this.movementContainer = GetComponent<BoxCollider2D>();
		this.hitBoxCollider = this.hitbox.GetComponent<BoxCollider2D>();
		this.triggerController = this.trigger.GetComponent<TriggerController>();
		this.mover = new CoroutineMover(this.animationCurve, this.rock);
	}

	private void Start() {
		this.top = transform.position + Vector3.up * this.movementContainer.size.y;
		this.bottom = transform.position;
		if (this.reversed) {
			this.start = this.bottom;
			this.end = this.top;
		} else {
			this.start = this.top;
			this.end = this.bottom;
		}
		if (!this.autoFall) {
			this.triggerController.subscribeToStateChange(onTriggerChange);
		}
	}

	private void OnEnable() {
		this.ToggleHitBox(this.triggerStatus);
		if (this.autoFall) {
			InvokeRepeating("RepeatMovement", this.startDelay, fallDuration);
		}
	}

	private void FixedUpdate() {
		if (this.active && !this.autoFall && !this.mover.moving) {
			if (this.triggerStatus && this.rock.transform.position == this.start) {
				this.currentTarget = this.end;
				this.StartMovement();
			}
			if (!this.triggerStatus && this.rock.transform.position == this.end) {
				this.currentTarget = this.start;
				this.StartMovement();
			}
		}
	}

	public void onTriggerChange(bool status) {
		this.triggerStatus = status;
	}

	public void RepeatMovement() {
		this.currentTarget = this.currentTarget == this.end ? this.start : this.end ;
		this.StartMovement();
	}

	public void StartMovement() {
		this.ToggleHitBox(this.currentTarget == this.bottom);
		StartCoroutine(this.mover.Move(this.rock.transform.position, this.currentTarget, fallDuration));
	}

	private void ToggleHitBox(bool status) {
		this.hitBoxCollider.enabled = status;
	}
}
