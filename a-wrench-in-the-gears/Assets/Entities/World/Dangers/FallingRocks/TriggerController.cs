using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class TriggerStatusEvent : UnityEvent<bool> {}

public class TriggerController : MonoBehaviour {

	public bool active = false;
	public TriggerStatusEvent onStatusChange;

	// private Animator animator;

	private void Awake() {
		// this.animator = GetComponent<Animator>();
		this.onStatusChange = new TriggerStatusEvent();
	}

	private void Start() {
		// this.animator.SetBool("status", this.active);
	}

	// private void Update() {
	// 	Debug.Log("Lever active: " + this.active);
	// }

	private void OnDisable() {
		this.onStatusChange.RemoveAllListeners();
	}

	public void subscribeToStateChange(UnityAction<bool> callback) {
		Debug.Log(this);
		Debug.Log(callback);
		this.onStatusChange.AddListener(callback);
	}

	public void Enable() {
		this.active = true;
		this.notifyStatusChange();
	}

	public void Disable() {
		this.active = false;
		this.notifyStatusChange();
	}

	public void Toggle() {
		this.active = !this.active;
		this.notifyStatusChange();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			this.Enable();
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			this.Disable();
		}
	}

	private void notifyStatusChange() {
		// this.animator.SetBool("status", this.active);
		this.onStatusChange.Invoke(this.active);
	}
}
