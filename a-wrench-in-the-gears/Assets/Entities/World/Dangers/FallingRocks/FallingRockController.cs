using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class FallingRockController : MonoBehaviour {

	public float fallSpeed;
	public GameObject rock;
	public GameObject trigger;
	public bool reversed = false;
	public bool active = true;

	private float nextFall = 0;
	private Vector3 top;
	private Vector3 bottom;
	private Vector3 start;
	private Vector3 end;
	private BoxCollider2D hitBox;
	private BoxCollider2D movementContainer;
	private TriggerController triggerController;
	private bool triggerStatus;
	private bool moving = false;
	private Vector3 currentTarget;
	private Vector3 velocity = Vector3.zero;

	private void Awake() {
		this.movementContainer = GetComponent<BoxCollider2D>();
		this.hitBox = this.rock.GetComponentInChildren<BoxCollider2D>();
		this.triggerController = this.trigger.GetComponent<TriggerController>();
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
		this.triggerController.subscribeToStateChange(onTriggerChange);
	}

	private void OnEnable() {
		this.ToggleHitBox(this.triggerStatus);
	}

	private void Update() {
		if (this.active) {
			if (!this.moving) {
				this.currentTarget = this.triggerStatus ? this.end : this.start;
				this.ToggleHitBox(this.triggerStatus);
			}
			this.Move();
		}
	}

	public void onTriggerChange(bool status) {
		this.triggerStatus = status;
	}

	public void ToggleHitBox(bool status) {
		this.hitBox.enabled = this.reversed ? !status : status;
	}

	private void Move() {
		if ((this.currentTarget == this.top && this.rock.transform.position.y >= this.currentTarget.y - .1)
			|| (this.currentTarget == this.bottom && this.rock.transform.position.y <= this.currentTarget.y + .1)) {
			this.moving = false;
		} else {
			this.moving = true;
			this.rock.transform.position = Vector3.SmoothDamp(this.rock.transform.position, this.currentTarget, ref this.velocity, this.fallSpeed);
		}
	}
}
