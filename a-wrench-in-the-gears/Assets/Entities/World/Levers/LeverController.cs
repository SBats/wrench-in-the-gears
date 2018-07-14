using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class LeverStatusEvent : UnityEvent<bool> {}

public class LeverController : MonoBehaviour {

	public bool active = false;
	public LeverStatusEvent onStatusChange;

	private Animator animator;

	private void Awake() {
		this.animator = GetComponent<Animator>();
		this.onStatusChange = new LeverStatusEvent();
	}

	private void Start() {
		this.animator.SetBool("status", this.active);
	}

	// private void Update() {
	// 	Debug.Log("Lever active: " + this.active);
	// }

	private void OnDisable() {
		this.onStatusChange.RemoveAllListeners();
	}

	public void subscribeToStateChange(UnityAction<bool> callback) {
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

	private void notifyStatusChange() {
		this.animator.SetBool("status", this.active);
		this.onStatusChange.Invoke(this.active);
	}
}
